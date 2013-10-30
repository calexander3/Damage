using Damage.Gadget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCalendar
{
    public class GoogleCalendar : IGadget
    {
        string _output = "";
        public void Initialize()
        {
            var request = (HttpWebRequest)WebRequest.Create("https://www.googleapis.com/calendar/v3/calendars/craig.d.alexander@gmail.com/events?timeMin=2013-04-26T00:00:00Z&access_token=ya29.AHES6ZRhOr8kzt7lO0D-BdQJZSMZX-VZIewKRzOgsXPbKbO-yOjCGA");
            using (var response = request.GetResponse())
            {
                _output = new System.IO.StreamReader(response.GetResponseStream()).ReadToEnd();
            }

        }

        public string HTML
        {
            get { return _output; }
        }

        public string Title
        {
            get { return "Google Calendar"; }
        }

        public string Description
        {
            get { return "An agenda of your upcoming calendar events."; }
        }

        public string DefaultSettings
        {
            get { return ""; }
        }

        public List<GadgetSettingField> SettingsSchema
        {
            get { return new List<GadgetSettingField>(); }
        }

        public Damage.DataAccess.Models.UserGadget UserGadget { get; set; }
    }
}
