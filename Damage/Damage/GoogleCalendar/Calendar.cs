using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCalendar
{
    internal class DefaultReminder
    {
        public string method { get; set; }
        public int minutes { get; set; }
    }

    internal class Creator
    {
        public string email { get; set; }
        public string displayName { get; set; }
        public bool self { get; set; }
    }

    internal class Organizer
    {
        public string email { get; set; }
        public string displayName { get; set; }
        public bool self { get; set; }
    }

    internal class Start
    {
        public string date { get; set; }
        public string dateTime { get; set; }
        public string timeZone { get; set; }
    }

    internal class End
    {
        public string date { get; set; }
        public string dateTime { get; set; }
        public string timeZone { get; set; }
    }

    internal class Override
    {
        public string method { get; set; }
        public int minutes { get; set; }
    }

    internal class Reminders
    {
        public bool useDefault { get; set; }
        public List<Override> overrides { get; set; }
    }

    internal class Attendee
    {
        public string email { get; set; }
        public string displayName { get; set; }
        public bool organizer { get; set; }
        public bool self { get; set; }
        public string responseStatus { get; set; }
    }

    internal class Source
    {
        public string url { get; set; }
        public string title { get; set; }
    }

    internal class Item
    {
        public string kind { get; set; }
        public string etag { get; set; }
        public string id { get; set; }
        public string status { get; set; }
        public string htmlLink { get; set; }
        public string created { get; set; }
        public string updated { get; set; }
        public string summary { get; set; }
        public Creator creator { get; set; }
        public Organizer organizer { get; set; }
        public Start start { get; set; }
        public End end { get; set; }
        public List<string> recurrence { get; set; }
        public string transparency { get; set; }
        public string iCalUID { get; set; }
        public int sequence { get; set; }
        public Reminders reminders { get; set; }
        public string location { get; set; }
        public List<Attendee> attendees { get; set; }
        public string description { get; set; }
        public bool? guestsCanInviteOthers { get; set; }
        public bool? privateCopy { get; set; }
        public Source source { get; set; }
    }

    internal class Calendar
    {
        public string kind { get; set; }
        public string etag { get; set; }
        public string summary { get; set; }
        public string updated { get; set; }
        public string timeZone { get; set; }
        public string accessRole { get; set; }
        public List<DefaultReminder> defaultReminders { get; set; }
        public List<Item> items { get; set; }
    }
}
