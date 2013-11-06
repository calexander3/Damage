using Damage.DataAccess;
using ImapX;
using ImapX.Authentication;
using ImapX.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Damage.Controllers
{
    public class GmailController : BaseController
    {
        public JsonResult GetMail(int? timezoneOffset, bool showUnreadOnly, string folderName)
        {
            var t = new System.Diagnostics.Stopwatch();
            t.Start();
            var successful = false;
            var output = new List<GmailMessage>();

            if (Request.IsAuthenticated)
            {
                using (var uow = new UnitOfWork(GlobalConfig.ConnectionString))
                {
                    var user = uow.UserRepository.GetUserByUsername(User.Identity.Name);

                    if (DateTime.Compare(DateTime.Now, user.OAuthAccessTokenExpiration) < 0)
                    {
                        var client = new ImapX.ImapClient("imap.gmail.com", true, true);
                        client.Behavior.MessageFetchMode = MessageFetchMode.Headers | MessageFetchMode.Tiny | MessageFetchMode.GMailExtendedData | MessageFetchMode.InternalDate;
                        if (client.Connect())
                        {
                            var credentials = new OAuth2Credentials(user.EmailAddress, user.CurrentOAuthAccessToken);
                            if (ExecuteWithTimeLimit(new TimeSpan(0, 0, 5), () => { client.Login(credentials); }))
                            {
                                if (client.IsAuthenticated)
                                {
                                    var unreadCount = 0;
                                    var _messages = client.Folders.Single(f => f.Name.ToLower() == folderName.ToLower()).Search((showUnreadOnly ? "UNSEEN" : "ALL"), client.Behavior.MessageFetchMode, 100).OrderByDescending(m => (m.InternalDate ?? m.Date).Value).ToList();
                                    var threads = new Dictionary<long, ImapX.Message>();
                                    var threadMessages = new Dictionary<long, List<long>>();
                                    var threadCounts = new Dictionary<long, int>();
                                    foreach (var m in _messages)
                                    {
                                        if (!threads.ContainsKey(m.GmailThread.Id))
                                        {
                                            threads.Add(m.GmailThread.Id, m);
                                            threadCounts.Add(m.GmailThread.Id, 1);
                                            threadMessages.Add(m.GmailThread.Id, new List<long>() { m.UId });
                                        }
                                        else
                                        {
                                            threadCounts[m.GmailThread.Id] += 1;
                                            threadMessages[m.GmailThread.Id].Add(m.UId);
                                        }
                                    }

                                    foreach (var thread in threads)
                                    {
                                        var messageDate = (timezoneOffset.HasValue ? thread.Value.InternalDate.Value.AddMinutes(timezoneOffset.Value) : thread.Value.InternalDate.Value.AddMinutes(timezoneOffset.Value));
                                        var messageDateString = (DateTime.Compare(messageDate.Date, DateTime.Now.Date) == 0 ? messageDate.ToShortTimeString() : messageDate.ToShortDateString());
                                        var unread = !thread.Value.Seen;
                                        if (unread)
                                        {
                                            unreadCount++;
                                        }
                                        output.Add(new GmailMessage()
                                        {
                                            Subject = thread.Value.Subject,
                                            From = (thread.Value.From.DisplayName.Length > 0 ? thread.Value.From.DisplayName : thread.Value.From.Address) + (threadCounts[thread.Key] > 1 ? " (" + threadCounts[thread.Key] + ")" : ""),
                                            ThreadIdHex = thread.Value.GmailThread.Id.ToString("X").ToLower(),
                                            ThreadId = thread.Value.GmailThread.Id,
                                            ThreadMessageIds = string.Join(",", threadMessages[thread.Value.GmailThread.Id].ToArray()),
                                            Date = messageDateString,
                                            Preview = getPreview(thread.Value.Body),
                                            Unread = unread,
                                            Important = (thread.Value.Labels.Any() && thread.Value.Labels[0].Equals("////Important"))
                                        });
                                    }

                                }
                                successful = true;
                            }
                        }
                    }
                }
            }
            t.Stop();
            return Json(new { Result = successful, Data = output, UnreadCount = unreadCount }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteMail(string[] MessageIds)
        {
            var t = new System.Diagnostics.Stopwatch();
            t.Start();
            var successful = false;
            if (Request.IsAuthenticated)
            {
                using (var uow = new UnitOfWork(GlobalConfig.ConnectionString))
                {
                    var user = uow.UserRepository.GetUserByUsername(User.Identity.Name);

                    if (DateTime.Compare(DateTime.Now, user.OAuthAccessTokenExpiration) < 0)
                    {
                        var client = new ImapX.ImapClient("imap.gmail.com", true, true);
                        client.Behavior.MessageFetchMode = MessageFetchMode.Headers | MessageFetchMode.GMailExtendedData;
                        if (client.Connect())
                        {
                            var credentials = new OAuth2Credentials(user.EmailAddress, user.CurrentOAuthAccessToken);
                            if (ExecuteWithTimeLimit(new TimeSpan(0, 0, 5), () => { client.Login(credentials); }))
                            {
                                if (client.IsAuthenticated)
                                {
                                    var MessageIdSequence = string.Join(",", MessageIds);

                                    var _messages = client.Folders.Single(f => f.Name.ToLower() == "inbox").Search("uid " + MessageIdSequence);
                                    foreach (var m in _messages)
                                    {
                                        var trash = client.Folders.Single(f => f.Name.ToLower() == "[gmail]").SubFolders.Single(f => f.Name.ToLower() == "trash");
                                        foreach (var tm in m.GmailThread.Messages)
                                        {
                                            tm.MoveTo(trash);
                                        }
                                    }
                                    successful = true;
                                }
                            }
                        }
                    }
                }
            }

            return Json(new { Result = successful });
        }

        [HttpPost]
        public JsonResult ArchiveMail(string[] MessageIds)
        {
            var successful = false;
            if (Request.IsAuthenticated)
            {
                using (var uow = new UnitOfWork(GlobalConfig.ConnectionString))
                {
                    var user = uow.UserRepository.GetUserByUsername(User.Identity.Name);

                    if (DateTime.Compare(DateTime.Now, user.OAuthAccessTokenExpiration) < 0)
                    {
                        var client = new ImapX.ImapClient("imap.gmail.com", true, true);
                        client.Behavior.MessageFetchMode = MessageFetchMode.Headers | MessageFetchMode.GMailExtendedData | MessageFetchMode.InternalDate;
                        if (client.Connect())
                        {
                            var credentials = new OAuth2Credentials(user.EmailAddress, user.CurrentOAuthAccessToken);
                            if (ExecuteWithTimeLimit(new TimeSpan(0, 0, 5), () => { client.Login(credentials); }))
                            {
                                if (client.IsAuthenticated)
                                {
                                    var MessageIdSequence = string.Join(",", MessageIds);
                                    var _messages = client.Folders.Single(f => f.Name.ToLower() == "inbox").Search("uid " + MessageIdSequence);
                                    foreach (var m in _messages)
                                    {
                                        foreach (var tm in m.GmailThread.Messages)
                                        {
                                            tm.Remove();
                                        }
                                    }

                                    successful = true;
                                }
                            }
                        }
                    }
                }
            }

            return Json(new { Result = successful });
        }

        static Regex _htmlRegex = new Regex("<.*?>", RegexOptions.Compiled | RegexOptions.Singleline);

        private string getPreview(MessageBody body)
        {
            if (body.HasText)
            {
                return HttpUtility.HtmlEncode(body.Text.Length > 80 ? body.Text.Substring(0, 80) : body.Text);
            }
            else if (body.HasHtml)
            {
                var text = _htmlRegex.Replace(body.Html, "");
                return HttpUtility.HtmlEncode(text.Length > 80 ? text.Substring(0, 80) : text);
            }
            else
            {
                return "";
            }
        }

        private class GmailMessage
        {
            public string From { get; set; }
            public string Preview { get; set; }
            public string Subject { get; set; }
            public string ThreadIdHex { get; set; }
            public long ThreadId { get; set; }
            public string ThreadMessageIds { get; set; }
            public string Date { get; set; }
            public bool Unread { get; set; }
            public bool Important { get; set; }
        }
    }
}
