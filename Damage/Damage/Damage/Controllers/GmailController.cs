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
        public JsonResult GetMail(int? timezoneOffset)
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
                        client.Behavior.MessageFetchMode = MessageFetchMode.Basic | MessageFetchMode.GMailExtendedData | MessageFetchMode.InternalDate;
                        if (client.Connect())
                        {
                            var credentials = new OAuth2Credentials(user.EmailAddress, user.CurrentOAuthAccessToken);
                            if (ExecuteWithTimeLimit(new TimeSpan(0, 0, 5), () => { client.Login(credentials); }))
                            {
                                if (client.IsAuthenticated)
                                {
                                    var _messages = client.Folders.Single(f => f.Name.ToLower() == "inbox").Search().OrderByDescending(m => (m.InternalDate ?? m.Date).Value).ToList();
                                    foreach (var m in _messages)
                                    {
                                        var messageDate =  (timezoneOffset.HasValue ? m.InternalDate.Value.AddMinutes(timezoneOffset.Value) : m.InternalDate.Value.AddMinutes(timezoneOffset.Value));
                                        var messageDateString = (DateTime.Compare(messageDate.Date,DateTime.Now.Date) == 0 ? messageDate.ToShortTimeString() : messageDate.ToShortDateString());
                                        output.Add(new GmailMessage()
                                        {
                                            Subject = m.Subject,
                                            From = (m.From.DisplayName.Length > 0 ? m.From.DisplayName : m.From.Address),
                                            MessageIdHex = m.GmailThread.Id.ToString("X").ToLower(),
                                            MessageId =  m.GmailThread.Id,
                                            Date = messageDateString,
                                            Preview = getPreview(m.Body),
                                            Unread = !m.Seen,
                                            Important = (m.Labels.Count() > 0 && m.Labels[0].Equals(""))
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

        private string getPreview(MessageBody body)
        {
            if (body.HasText)
            {
                var text = Regex.Replace(body.Text, "<.*?>", "", RegexOptions.Singleline);
                return (text.Length > 80 ? text.Substring(0, 80) : text);
            }
            else if (body.HasHtml)
            {
                var text = Regex.Replace(body.Html, "<.*?>", "", RegexOptions.Singleline);
                return (text.Length > 80 ? text.Substring(0, 80) : text);
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
            public string Date { get; set; }
            public bool Unread { get; set; }
            public bool Important { get; set; }
        }
    }
}
