using Damage.Gadget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.ServiceModel.Syndication;
using Newtonsoft.Json;

namespace GoogleCalendar
{
    public class GoogleCalendar : IGadget
    {
        string _output = "";
        public void Initialize()
        {
            var settings = JsonConvert.DeserializeObject<GoogleCalendarOptions>(UserGadget.GadgetSettings);

            var sb = new System.Text.StringBuilder("<div style='max-height:600px; margin-left:5px; margin-right:5px;'>");
            Calendar calendar = null;
            var startTime = DateTime.Now.ToString("yyyy-MM-ddT00:00:00Z");
            var endTime = DateTime.Now.AddMonths(settings.MonthsToDisplay).ToString("yyyy-MM-ddT00:00:00Z");
            var request = (HttpWebRequest)WebRequest.Create("https://www.googleapis.com/calendar/v3/calendars/" + UserGadget.User.EmailAddress + "/events?singleEvents=true&orderBy=startTime&sanitizeHtml=true&timeMin=" + startTime + "&timeMax=" + endTime);
            request.Headers["Authorization"] = string.Format("Bearer {0}", UserGadget.User.CurrentOAuthAccessToken);
            using (var response = request.GetResponse())
            {
                calendar = JsonConvert.DeserializeObject<Calendar>(new System.IO.StreamReader(response.GetResponseStream()).ReadToEnd());
            }

            var previousDate = "";
            foreach (var calItem in calendar.items)
            {
                var eventDate = System.DateTime.Parse(calItem.start.date ?? calItem.start.dateTime).ToShortDateString();
                if (previousDate != eventDate)
                {
                    sb.Append("<div style='white-space:nowrap;clear:both;font-weight:bold'>" + eventDate  + "</div>");
                    previousDate = eventDate;
                }
                sb.Append("<div style='margin-left:10px;white-space:nowrap;font-size:0.8em;clear:both'><a target='_blank' title='" + (calItem.location != null ? System.Security.SecurityElement.Escape(calItem.location + Environment.NewLine) : "") + 
                    System.Security.SecurityElement.Escape(calItem.description ?? calItem.summary) +
                    "' href='" + calItem.htmlLink + "' >" + calItem.summary + "</a><div style='float:right;white-space:nowrap;'>" +
                    (calItem.start.dateTime != null ? System.DateTime.Parse(calItem.start.dateTime).ToString("hh:mmtt") : "All Day") +
                    (calItem.end.dateTime != null ? " - " + System.DateTime.Parse(calItem.end.dateTime).ToString("hh:mmtt") : "") +
                    "</div></div>");
            }

            sb.Append("</div>");
            _output = sb.ToString();
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
            get { return JsonConvert.SerializeObject(new GoogleCalendarOptions() { MonthsToDisplay = 6 }); }
        }

        public List<GadgetSettingField> SettingsSchema
        {
            get
            {
                return new List<GadgetSettingField>() 
                { 
                    new GadgetSettingField() {
                        DisplayName="Months to Display", 
                        FieldName="MonthsToDisplay", 
                        DataType= SettingDataTypes.Number, 
                        Validators= Validators.Number | Validators.Required
                    }
                };
            }
        }

        public Damage.DataAccess.Models.UserGadget UserGadget { get; set; }

        public bool InBeta
        {
            get { return false; }
        }

        public bool RequiresValidGoogleAccessToken
        {
            get { return true; }
        }
    }
}
