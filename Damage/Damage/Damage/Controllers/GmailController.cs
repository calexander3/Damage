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
                                    var _messages = client.Folders.Single(f => f.Name.ToLower() == folderName.ToLower()).Search((showUnreadOnly ? "UNSEEN" : "ALL"), client.Behavior.MessageFetchMode, 100).OrderByDescending(m => (m.InternalDate ?? m.Date).Value).ToList();
                                    var threads = new Dictionary<long, ImapX.Message>();
                                    var threadCounts = new Dictionary<long, int>();
                                    foreach (var m in _messages)
                                    {
                                        if (!threads.ContainsKey(m.GmailThread.Id))
                                        {
                                            threads.Add(m.GmailThread.Id, m);
                                            threadCounts.Add(m.GmailThread.Id, 1);
                                        }
                                        else
                                        {
                                            threadCounts[m.GmailThread.Id] += 1;
                                        }
                                    }

                                    foreach (var thread in threads)
                                    {
                                        var messageDate = (timezoneOffset.HasValue ? thread.Value.InternalDate.Value.AddMinutes(timezoneOffset.Value) : thread.Value.InternalDate.Value.AddMinutes(timezoneOffset.Value));
                                        var messageDateString = (DateTime.Compare(messageDate.Date, DateTime.Now.Date) == 0 ? messageDate.ToShortTimeString() : messageDate.ToShortDateString());
                                        output.Add(new GmailMessage()
                                        {
                                            Subject = thread.Value.Subject,
                                            From = (thread.Value.From.DisplayName.Length > 0 ? thread.Value.From.DisplayName : thread.Value.From.Address) + (threadCounts[thread.Key] > 1 ? " (" +  threadCounts[thread.Key] + ")" : ""),
                                            MessageIdHex = thread.Value.GmailThread.Id.ToString("X").ToLower(),
                                            MessageId = thread.Value.GmailThread.Id,
                                            ImapId = thread.Value.UId,
                                            Date = messageDateString,
                                            Preview = getPreview(thread.Value.Body),
                                            Unread = !thread.Value.Seen,
                                            Important = (thread.Value.Labels.Count() > 0 && thread.Value.Labels[0].Equals(""))
                                        });
                                    }

                                }
                                successful = true;
                            }
                        }
                    }
                }
            }
            return Json(new { Result = successful, Data = output }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteMail(string[] MessageIds)
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
                                    var MessageIdSequence = "";
                                    foreach (var messageId in MessageIds)
                                    {
                                        MessageIdSequence += messageId + " ";
                                    }
                                    var _message = client.Folders.Single(f => f.Name.ToLower() == "inbox").Search("uid " + MessageIdSequence, MessageFetchMode.GMailExtendedData).Single();
                                    successful = _message.Remove();
                                }
                            }
                        }
                    }
                }
            }

            return Json(new { Result = successful });
        }

        private string getPreview(MessageBody body)
        {
            if (body.HasText)
            {
                return HttpUtility.HtmlEncode(body.Text.Length > 80 ? body.Text.Substring(0, 80) : body.Text);
            }
            else if (body.HasHtml)
            {
                var text = Regex.Replace(body.Html, "<.*?>", "", RegexOptions.Singleline);
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
            public string MessageIdHex { get; set; }
            public long MessageId { get; set; }
            public long ImapId { get; set; }
            public string Date { get; set; }
            public bool Unread { get; set; }
            public bool Important { get; set; }
        }
    }
}
