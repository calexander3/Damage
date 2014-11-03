using System.Diagnostics;
using System.Net.Mail;
using System.Runtime.Serialization.Formatters;
using System.Web.Caching;
using AE.Net.Mail;
using Damage.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Damage.Utilities;
using Damage.ValueObjects;
using Newtonsoft.Json;

namespace Damage.Controllers
{
    public class GmailController : BaseController
    {
        public JsonResult GetMail(int? timezoneOffset, bool showUnreadOnly, bool showPreview, string folderName)
        {
            var successful = false;
            var output = new List<GmailThread>();
            var unreadCount = 0;
            var folders = new List<string>();

            if (Request.IsAuthenticated)
            {
                using (var uow = new UnitOfWork(GlobalConfig.ConnectionString))
                {
                    var user = uow.UserRepository.GetUserByUsername(User.Identity.Name);

                    if (DateTime.Compare(DateTime.Now, user.OAuthAccessTokenExpiration) < 0)
                    {
                        using (
                            var imap = new ImapClient("imap.gmail.com", user.EmailAddress,
                                user.CurrentOAuthAccessToken, AuthMethods.SaslOAuth, 993, true,
                                true))
                        {
                            imap.SelectMailbox(folderName);

                            var listMailboxes = imap.ListMailboxes(string.Empty, "*");

                            foreach (var listMailbox in listMailboxes)
                            {
                                if (!listMailbox.Name.StartsWith("[Gmail]") &&
                                    String.Compare(listMailbox.Name, folderName, StringComparison.OrdinalIgnoreCase) != 0)
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

                            var uidCollection = imap.Search(searchCondition);

                            var t = new Stopwatch();
                            t.Start();
                            var messages = new List<GmailMessage>();
                            foreach (var uid in uidCollection)
                            {
                                var cacheKey = "gmail_" + uid + (showPreview ? "_WPrev" : "");
                                var mailMessageBytes = HttpContext.Cache.Get(cacheKey) as byte[];
                                if (mailMessageBytes == null)
                                {
                                    var mailMessage = new GmailMessage(imap.GetMessage(uid, !showPreview, false));
                                    var encryptedMessage = GlobalConfig.Encryptor.EncryptString(JsonConvert.SerializeObject(mailMessage));
                                    HttpContext.Cache.Insert(cacheKey, encryptedMessage);
                                    messages.Add(mailMessage);
                                }
                                else
                                {
                                    var decryptedMessage = GlobalConfig.Encryptor.DecryptString(mailMessageBytes);
                                    var mailMessage = JsonConvert.DeserializeObject<GmailMessage>(decryptedMessage);
                                    messages.Add(mailMessage);
                                }
                            }
                            t.Stop();
                            var threads = new Dictionary<long, GmailMessage>();
                            var threadMessages = new Dictionary<long, List<long>>();
                            var threadCounts = new Dictionary<long, int>();
                            foreach (var m in messages.OrderByDescending(m => m.MessageDate))
                            {
                                var headers = m.Headers;
                                var gmailThreadId = long.Parse(headers["X-GM-THRID"]);

                                if (!threads.ContainsKey(gmailThreadId))
                                {
                                    threads.Add(gmailThreadId, m);
                                    threadCounts.Add(gmailThreadId, 1);
                                    threadMessages.Add(gmailThreadId, new List<long> {m.Uid});
                                }
                                else
                                {
                                    threadCounts[gmailThreadId] += 1;
                                    threadMessages[gmailThreadId].Add(m.Uid);
                                }
                            }


                            //Bundle threads
                            foreach (var thread in threads)
                            {
                                var messageDate = (thread.Value.MessageDate.Ticks > 0
                                    ? (timezoneOffset.HasValue
                                        ? thread.Value.MessageDate.ToUniversalTime().AddMinutes(timezoneOffset.Value)
                                        : thread.Value.MessageDate.ToUniversalTime())
                                    : new DateTime(1900, 1, 1));
                                var messageDateString = (DateTime.Compare(messageDate.Date, DateTime.Now.Date) == 0
                                    ? messageDate.ToShortTimeString()
                                    : messageDate.ToShortDateString());
                                var unread = !(thread.Value.MessageFlags.HasFlag(GmailMessage.Flags.Seen));
                                if (unread)
                                {
                                    unreadCount++;
                                }
                                output.Add(new GmailThread
                                {
                                    Subject = thread.Value.Subject,
                                    From =
                                        thread.Value.FromDisplayName +
                                        (threadCounts[thread.Key] > 1 ? " (" + threadCounts[thread.Key] + ")" : ""),
                                    ThreadIdHex = thread.Key.ToString("X").ToLower(),
                                    ThreadId = thread.Key,
                                    ThreadMessageIds = string.Join(",", threadMessages[thread.Key].ToArray()),
                                    Date = messageDateString,
                                    Preview = (showPreview ? getPreview(thread.Value.Body) : ""),
                                    Unread = unread,
                                    Important =
                                        (thread.Value.Headers.ContainsKey("X-GM-LABELS") &&
                                         thread.Value.Headers["X-GM-LABELS"].Equals("\"\\\\Important\""))
                                });
                            }
                            successful = true;
                        }
                    }
                }
            }
            return Json(
                new {Result = successful, Data = output, UnreadCount = unreadCount, Folders = folders.ToArray()},
                JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult MoveMail(string[] messageIds, string originalFolderName, string folderName)
        {
            var successful = false;
            if (Request.IsAuthenticated)
            {
                using (var uow = new UnitOfWork(GlobalConfig.ConnectionString))
                {
                    var user = uow.UserRepository.GetUserByUsername(User.Identity.Name);

                    if (DateTime.Compare(DateTime.Now, user.OAuthAccessTokenExpiration) < 0)
                    {
                        using (
                            var imap = new ImapClient("imap.gmail.com", user.EmailAddress,
                                user.CurrentOAuthAccessToken, AuthMethods.SaslOAuth, 993, true,
                                true))
                        {
                            imap.SelectMailbox(originalFolderName);
                            foreach (var messageId in messageIds)
                            {
                                imap.MoveMessage(messageId, folderName);
                            }
                            successful = true;
                        }
                    }
                }
            }
            return Json(new {Result = successful});
        }

        [HttpPost]
        public JsonResult DeleteMail(string[] messageIds, string folderName)
        {
            var successful = false;
            if (Request.IsAuthenticated)
            {
                using (var uow = new UnitOfWork(GlobalConfig.ConnectionString))
                {
                    var user = uow.UserRepository.GetUserByUsername(User.Identity.Name);

                    if (DateTime.Compare(DateTime.Now, user.OAuthAccessTokenExpiration) < 0)
                    {
                        using (
                            var imap = new ImapClient("imap.gmail.com", user.EmailAddress,
                                user.CurrentOAuthAccessToken, AuthMethods.SaslOAuth, 993, true,
                                true))
                        {
                            imap.SelectMailbox(folderName);
                            foreach (var messageId in messageIds)
                            {
                                try
                                {
                                    imap.MoveMessage(messageId, "[Gmail]/Trash");
                                }
                                // ReSharper disable once EmptyGeneralCatchClause
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

            return Json(new {Result = successful});
        }

        [HttpPost]
        public JsonResult ArchiveMail(string[] messageIds, string folderName)
        {
            var successful = false;
            if (Request.IsAuthenticated)
            {
                using (var uow = new UnitOfWork(GlobalConfig.ConnectionString))
                {
                    var user = uow.UserRepository.GetUserByUsername(User.Identity.Name);

                    if (DateTime.Compare(DateTime.Now, user.OAuthAccessTokenExpiration) < 0)
                    {
                        using (
                            var imap = new ImapClient("imap.gmail.com", user.EmailAddress,
                                user.CurrentOAuthAccessToken, AuthMethods.SaslOAuth, 993, true,
                                true))
                        {
                            imap.SelectMailbox(folderName);
                            foreach (var messageId in messageIds)
                            {
                                imap.MoveMessage(messageId, "[Gmail]/All Mail");
                            }
                            imap.Expunge();
                            successful = true;
                        }
                    }
                }
            }

            return Json(new {Result = successful});
        }

        [HttpPost]
        public JsonResult ReportAsSpam(string[] messageIds, string folderName)
        {
            var successful = false;
            if (Request.IsAuthenticated)
            {
                using (var uow = new UnitOfWork(GlobalConfig.ConnectionString))
                {
                    var user = uow.UserRepository.GetUserByUsername(User.Identity.Name);

                    if (DateTime.Compare(DateTime.Now, user.OAuthAccessTokenExpiration) < 0)
                    {
                        using (
                            var imap = new ImapClient("imap.gmail.com", user.EmailAddress,
                                user.CurrentOAuthAccessToken, AuthMethods.SaslOAuth, 993, true,
                                true))
                        {
                            imap.SelectMailbox(folderName);
                            foreach (var messageId in messageIds)
                            {
                                try
                                {
                                    imap.MoveMessage(messageId, "[Gmail]/Spam");
                                }
                                // ReSharper disable once EmptyGeneralCatchClause
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

            return Json(new {Result = successful});
        }


        private string getPreview(string body)
        {
            return HttpUtility.HtmlEncode(body.Length > 80 ? body.Substring(0, 80) : body);
        }
    }
}