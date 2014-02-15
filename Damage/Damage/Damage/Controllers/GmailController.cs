using AE.Net.Mail;
using Damage.DataAccess;
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
        public JsonResult GetMail(int? timezoneOffset, bool showUnreadOnly, bool showPreview, string folderName)
        {
            var successful = false;
            var output = new List<GmailMessage>();
            var unreadCount = 0;
            var folders = new List<string>();

            if (Request.IsAuthenticated)
            {
                using (var uow = new UnitOfWork(GlobalConfig.ConnectionString))
                {
                    var user = uow.UserRepository.GetUserByUsername(User.Identity.Name);

                    if (DateTime.Compare(DateTime.Now, user.OAuthAccessTokenExpiration) < 0)
                    {
                        using (var imap = new AE.Net.Mail.ImapClient("imap.gmail.com", user.EmailAddress, user.CurrentOAuthAccessToken, AE.Net.Mail.ImapClient.AuthMethods.SaslOAuth, 993, true, true))
                        {
                            imap.SelectMailbox(folderName);

                            var listMailboxes = imap.ListMailboxes(string.Empty, "*");

                            foreach (var listMailbox in listMailboxes)
                            {
                                if (!listMailbox.Name.StartsWith("[Gmail]") && listMailbox.Name.ToLower().CompareTo(folderName.ToLower()) != 0)
                                {
                                    folders.Add(listMailbox.Name);
                                }
                            }


                            var searchCondition = SearchCondition.Undeleted();
                            if (showUnreadOnly)
                            {
                                searchCondition = searchCondition.And(SearchCondition.Unseen());
                            }

                            //Get messages and organize into threads
                            var messages = imap.SearchMessages(searchCondition, !showPreview, false).OrderByDescending(m => m.Value.Date).ToList();
                            var threads = new Dictionary<long, AE.Net.Mail.MailMessage>();
                            var threadMessages = new Dictionary<long, List<long>>();
                            var threadCounts = new Dictionary<long, int>();
                            foreach (var m in messages)
                            {
                                var messageId = long.Parse(m.Value.Uid);
                                var headers = m.Value.Headers;
                                var gmailThreadId = long.Parse(headers["X-GM-THRID"].Value);

                                if (!threads.ContainsKey(gmailThreadId))
                                {
                                    threads.Add(gmailThreadId, m.Value);
                                    threadCounts.Add(gmailThreadId, 1);
                                    threadMessages.Add(gmailThreadId, new List<long>() { messageId });
                                }
                                else
                                {
                                    threadCounts[gmailThreadId] += 1;
                                    threadMessages[gmailThreadId].Add(messageId);
                                }
                            }


                            //Bundle threads
                            foreach (var thread in threads)
                            {
                                var messageDate = (thread.Value.Date.Ticks > 0 ? (timezoneOffset.HasValue ? thread.Value.Date.AddMinutes(timezoneOffset.Value) : thread.Value.Date) : new DateTime(1900, 1, 1));
                                var messageDateString = (DateTime.Compare(messageDate.Date, DateTime.Now.Date) == 0 ? messageDate.ToShortTimeString() : messageDate.ToShortDateString());
                                var unread = !(thread.Value.Flags.HasFlag(Flags.Seen));
                                if (unread)
                                {
                                    unreadCount++;
                                }
                                output.Add(new GmailMessage()
                                {
                                    Subject = thread.Value.Subject,
                                    From = (thread.Value.From.DisplayName.Length > 0 ? thread.Value.From.DisplayName : thread.Value.From.Address.Split("@".ToCharArray())[0]) + (threadCounts[thread.Key] > 1 ? " (" + threadCounts[thread.Key] + ")" : ""),
                                    ThreadIdHex = thread.Key.ToString("X").ToLower(),
                                    ThreadId = thread.Key,
                                    ThreadMessageIds = string.Join(",", threadMessages[thread.Key].ToArray()),
                                    Date = messageDateString,
                                    Preview = (showPreview ? getPreview(thread.Value.Body) : ""),
                                    Unread = unread,
                                    Important = (thread.Value.Headers.ContainsKey("X-GM-LABELS") && thread.Value.Headers["X-GM-LABELS"].Value.Equals("\"\\\\Important\""))
                                });
                            }
                            successful = true;
                        }
                    }
                }
            }
            return Json(new { Result = successful, Data = output, UnreadCount = unreadCount, Folders = folders.ToArray() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult MoveMail(string[] MessageIds, string originalFolderName, string FolderName)
        {
            var successful = false;
            if (Request.IsAuthenticated)
            {
                using (var uow = new UnitOfWork(GlobalConfig.ConnectionString))
                {
                    var user = uow.UserRepository.GetUserByUsername(User.Identity.Name);

                    if (DateTime.Compare(DateTime.Now, user.OAuthAccessTokenExpiration) < 0)
                    {
                        using (var imap = new AE.Net.Mail.ImapClient("imap.gmail.com", user.EmailAddress, user.CurrentOAuthAccessToken, AE.Net.Mail.ImapClient.AuthMethods.SaslOAuth, 993, true, true))
                        {
                            imap.SelectMailbox(originalFolderName);
                            foreach (var messageId in MessageIds)
                            {
                                imap.MoveMessage(messageId, FolderName);
                            }
                            successful = true;
                        }
                    }
                }
            }
            return Json(new { Result = successful });
        }

        [HttpPost]
        public JsonResult DeleteMail(string[] MessageIds, string folderName)
        {
            var successful = false;
            if (Request.IsAuthenticated)
            {
                using (var uow = new UnitOfWork(GlobalConfig.ConnectionString))
                {
                    var user = uow.UserRepository.GetUserByUsername(User.Identity.Name);

                    if (DateTime.Compare(DateTime.Now, user.OAuthAccessTokenExpiration) < 0)
                    {
                        using (var imap = new AE.Net.Mail.ImapClient("imap.gmail.com", user.EmailAddress, user.CurrentOAuthAccessToken, AE.Net.Mail.ImapClient.AuthMethods.SaslOAuth, 993, true, true))
                        {
                            imap.SelectMailbox(folderName);
                            foreach (var messageId in MessageIds)
                            {
                                try
                                {
                                    imap.MoveMessage(messageId, "[Gmail]/Trash");
                                }
                                catch (Exception) //Deleting always throws excptions
                                {
                                }
                                finally
                                {
                                    imap.Expunge();
                                }
                            }
                            successful = true;
                        }
                    }
                }
            }

            return Json(new { Result = successful });
        }

        [HttpPost]
        public JsonResult ArchiveMail(string[] MessageIds, string folderName)
        {
            var successful = false;
            if (Request.IsAuthenticated)
            {
                using (var uow = new UnitOfWork(GlobalConfig.ConnectionString))
                {
                    var user = uow.UserRepository.GetUserByUsername(User.Identity.Name);

                    if (DateTime.Compare(DateTime.Now, user.OAuthAccessTokenExpiration) < 0)
                    {
                        using (var imap = new AE.Net.Mail.ImapClient("imap.gmail.com", user.EmailAddress, user.CurrentOAuthAccessToken, AE.Net.Mail.ImapClient.AuthMethods.SaslOAuth, 993, true, true))
                        {
                            imap.SelectMailbox(folderName);
                            foreach (var messageId in MessageIds)
                            {
                                imap.MoveMessage(messageId, "[Gmail]/All Mail");
                            }
                            imap.Expunge();
                            successful = true;
                        }
                    }
                }
            }

            return Json(new { Result = successful });
        }

        [HttpPost]
        public JsonResult ReportAsSpam(string[] MessageIds, string folderName)
        {
            var successful = false;
            if (Request.IsAuthenticated)
            {
                using (var uow = new UnitOfWork(GlobalConfig.ConnectionString))
                {
                    var user = uow.UserRepository.GetUserByUsername(User.Identity.Name);

                    if (DateTime.Compare(DateTime.Now, user.OAuthAccessTokenExpiration) < 0)
                    {
                        using (var imap = new AE.Net.Mail.ImapClient("imap.gmail.com", user.EmailAddress, user.CurrentOAuthAccessToken, AE.Net.Mail.ImapClient.AuthMethods.SaslOAuth, 993, true, true))
                        {
                            imap.SelectMailbox(folderName);
                            foreach (var messageId in MessageIds)
                            {
                                try
                                {
                                    imap.MoveMessage(messageId, "[Gmail]/Spam");
                                }
                                catch (Exception) //Deleting always throws excptions
                                {
                                }
                                finally
                                {
                                    imap.Expunge();
                                }
                            }
                            successful = true;
                        }
                    }
                }
            }

            return Json(new { Result = successful });
        }


        private string getPreview(string body)
        {
            return HttpUtility.HtmlEncode(body.Length > 80 ? body.Substring(0, 80) : body);
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
