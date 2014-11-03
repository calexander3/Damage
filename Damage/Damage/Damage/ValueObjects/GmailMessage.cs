using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;

namespace Damage.ValueObjects
{
    public class GmailMessage
    {
        public long Uid { get; set; }
        public DateTime MessageDate { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public Flags MessageFlags { get; set; }
        public string FromAddress { get; set; }
        public string FromDisplayName { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public GmailMessage()
        {
        }

        public GmailMessage(AE.Net.Mail.MailMessage message)
        {
            Uid = long.Parse(message.Uid);
            Headers = message.Headers.ToDictionary(p => p.Key, p=> p.Value.Value);
            MessageDate = message.Date;
            MessageFlags = (Flags)(int)message.Flags;
            FromAddress = message.From.Address;
            FromDisplayName = message.From.DisplayName.Length > 0
                ? message.From.DisplayName
                : message.From.Address.Split("@".ToCharArray())[0];
            Subject = message.Subject;
            Body = message.Body ?? string.Empty;
        }

        [Flags]
        public enum Flags
        {
            None = 0,
            Seen = 1,
            Answered = 2,
            Flagged = 4,
            Deleted = 8,
            Draft = 16,
        }
    }
}