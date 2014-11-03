namespace Damage.ValueObjects
{
    public class GmailThread
    {
        // ReSharper disable UnusedAutoPropertyAccessor.Local
        public string From { get; set; }
        public string Preview { get; set; }
        public string Subject { get; set; }
        public string ThreadIdHex { get; set; }
        public long ThreadId { get; set; }
        public string ThreadMessageIds { get; set; }
        public string Date { get; set; }
        public bool Unread { get; set; }
        public bool Important { get; set; }
        // ReSharper restore UnusedAutoPropertyAccessor.Local 
    }
}