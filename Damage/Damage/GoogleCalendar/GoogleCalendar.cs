using System.Globalization;
using Damage.Gadget;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web;

namespace GoogleCalendar
{
    public class GoogleCalendar : IGadget
    {
        string _output = "";
        string _title = "Google Calendar";
        public void Initialize()
        {
            _title = "<a href='https://www.google.com/calendar' target='_blank' >Google Calendar</a>";
            var settings = JsonConvert.DeserializeObject<GoogleCalendarOptions>(UserGadget.GadgetSettings);

            var sb = new System.Text.StringBuilder("<div style='max-height:400px;overflow-y:auto;margin-left:5px;margin-right:5px;margin-top:2px;margin-bottom:2px;'>");
            Calendar calendar;

            if (HttpContext.Current.Cache[UserGadget.User.UserId + "_cal"] != null)
            {
                calendar = (Calendar)HttpContext.Current.Cache[UserGadget.User.UserId + "_cal"];
            }
            else
            {
                var startTime = DateTime.Now.ToString("yyyy-MM-ddT00:00:00Z");
                var endTime = DateTime.Now.AddMonths(settings.MonthsToDisplay).ToString("yyyy-MM-ddT00:00:00Z");
                var request = (HttpWebRequest)WebRequest.Create("https://www.googleapis.com/calendar/v3/calendars/" + UserGadget.User.EmailAddress + "/events?singleEvents=true&orderBy=startTime&sanitizeHtml=true&timeMin=" + startTime + "&timeMax=" + endTime);
                request.Headers["Authorization"] = string.Format("Bearer {0}", UserGadget.User.CurrentOAuthAccessToken);
                using (var response = request.GetResponse())
                {
                    calendar = JsonConvert.DeserializeObject<Calendar>(new System.IO.StreamReader(response.GetResponseStream()).ReadToEnd());
                    HttpContext.Current.Cache.Insert(UserGadget.User.UserId + "_cal", calendar, null, DateTime.Now.AddMinutes(1), System.Web.Caching.Cache.NoSlidingExpiration);
                }
            }

            var previousDateKey = "";
            foreach (var calItem in calendar.items)
            {
                var eventDate = DateTime.Parse(calItem.start.date ?? calItem.start.dateTime, new DateTimeFormatInfo(), DateTimeStyles.AdjustToUniversal);
                var eventDateKey = eventDate.ToShortDateString();
                var eventDisplayDate = eventDate.ToString(@"MM/dd/yyyy hh:mm tt");
                if (previousDateKey != eventDateKey)
                {
                    sb.Append("<div style='white-space:nowrap;clear:both;font-weight:bold;font-size:0.8em;margin-top:4px;'><span class='UTCDate'>" + eventDisplayDate + "</div>");
                    previousDateKey = eventDateKey;
                }
                sb.Append("<div style='margin-left:10px;font-size:0.8em;clear:both'><a target='_blank' title='" + (calItem.location != null ? System.Security.SecurityElement.Escape(calItem.location + Environment.NewLine) : "") +
                    System.Security.SecurityElement.Escape(calItem.description ?? calItem.summary ?? "") +
                    "' href='" + calItem.htmlLink + "' >" + (calItem.summary ?? "No Title") + "</a><div style='float:right;'>" +
                    (calItem.start.dateTime != null ? "<span class='UTCTime'>" + DateTime.Parse(calItem.start.dateTime, new DateTimeFormatInfo(), DateTimeStyles.AdjustToUniversal).ToString(@"MM/dd/yyyy hh:mm tt") + "</span>" : "All Day") +
                    (calItem.end.dateTime != null ? " - <span class='UTCTime'>" + DateTime.Parse(calItem.end.dateTime, new DateTimeFormatInfo(), DateTimeStyles.AdjustToUniversal).ToString(@"MM/dd/yyyy hh:mm tt") + "</span>" : "") +
                    "</div></div>");
            }

            sb.Append("<div style='margin-top:13px;'><a style='padding:0px;' href='https://www.google.com/calendar/render?action=TEMPLATE' target='_blank'>Create New Event</a></div></div>");
            _output = sb.ToString();
        }

        public string HTML
        {
            get { return _output; }
        }

        public string Title
        {
            get { return _title; }
        }

        public string Description
        {
            get { return "An agenda of your upcoming calendar events."; }
        }

        public string DefaultSettings
        {
            get { return JsonConvert.SerializeObject(new GoogleCalendarOptions { MonthsToDisplay = 6 }); }
        }

        public List<GadgetSettingField> SettingsSchema
        {
            get
            {
                return new List<GadgetSettingField>
                { 
                    new GadgetSettingField {
                        DisplayName="Months to Display", 
                        FieldName="MonthsToDisplay", 
                        DataType= SettingDataTypes.Number, 
                        Validators= Validators.Integer | Validators.Required
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
