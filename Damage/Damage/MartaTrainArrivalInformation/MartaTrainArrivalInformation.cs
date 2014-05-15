using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using Damage.Gadget;
using Newtonsoft.Json;

namespace MartaTrainArrivalInformation
{
    public class MartaTrainArrivalInformation : IGadget
    {
        private string _html;

        private readonly Dictionary<string, string> _stationOptions = new Dictionary<string, string>
        {
            {"arts%20center","Arts Center"},
            {"five%20points","Five Points"}
        };


        public void Initialize()
        {
            var settings = JsonConvert.DeserializeObject<MartaOptions>(UserGadget.GadgetSettings);

            List<TrainSchedule> scheduleList;

            if (HttpContext.Current.Cache[settings.SelectedStation + "_martaSchedule"] != null)
            {
                scheduleList = (List<TrainSchedule>)HttpContext.Current.Cache[UserGadget.UserGadgetId + "_martaSchedule"];
            }
            else
            {
                var scheduleRequest = (HttpWebRequest)WebRequest.Create("http://developer.itsmarta.com/NextTrainService/RestServiceNextTrain.svc/GetNextTrain/" + settings.SelectedStation);

                using (var scheduleResponce = scheduleRequest.GetResponse())
                {
                    scheduleList = JsonConvert.DeserializeObject<List<TrainSchedule>>(new System.IO.StreamReader(scheduleResponce.GetResponseStream()).ReadToEnd());
                    HttpContext.Current.Cache.Insert(UserGadget.UserGadgetId + "_martaSchedule", scheduleList, null, DateTime.Now.AddSeconds(60), System.Web.Caching.Cache.NoSlidingExpiration);
                }
            }

            var outputBuilder = new StringBuilder("<div style='margin-left:2px;margin-right:2px;margin-top:4px;margin-bottom:1px;overflow:hidden;'><div style='font-weight:bold;font-size:2em;margin-left:4px;'>" + _stationOptions[settings.SelectedStation] + "</div>");
            if (scheduleList != null)
            {
                outputBuilder.Append("<div style='float:left'><table style='margin: 0; border-collapse: separate; border-spacing: 3px;'>");
                foreach (var schedule in scheduleList.Take(8))
                {
                    int min;
                    outputBuilder.Append("<tr><td style='padding:0px 50px 0px 3px;margin:1px 15px 1px 3px;font-weight:bold;background-color:" + GetColor(schedule.ROUTE) + "'>" + schedule.HEAD_SIGN + " " + GetArrow(schedule.DIRECTION) + "</td><td style='padding-left:3px;'>" + (int.TryParse(schedule.WAITING_TIME, out min) ? (schedule.WAITING_TIME + " Minutes") : schedule.WAITING_TIME) + "</td></tr>");
                }
                outputBuilder.Append("</table></div>");
                if (scheduleList.Count > 8)
                {
                    outputBuilder.Append("<div style='float:left'><table style='margin: 0; border-collapse: separate; border-spacing: 3px;'>");
                    foreach (var schedule in scheduleList.Skip(8))
                    {
                        int min;
                        outputBuilder.Append("<tr><td style='padding:0px 50px 0px 3px;margin:1px 15px 1px 3px;font-weight:bold;background-color:" + GetColor(schedule.ROUTE) + "'>" + schedule.HEAD_SIGN + " " + GetArrow(schedule.DIRECTION) + "</td><td style='padding-left:3px;'>" + (int.TryParse(schedule.WAITING_TIME, out min) ? (schedule.WAITING_TIME + " Minutes") : schedule.WAITING_TIME) + "</td></tr>");
                    }
                    outputBuilder.Append("</table></div>");
                }
            }


            outputBuilder.Append("<div style='clear:both'></div></div>");
            _html = outputBuilder.ToString();
        }

        private string GetColor(string railLine)
        {
            switch (railLine.ToLower())
            {
                case "blue":
                    return "#005EFF";
                case "green":
                case "red":
                case "gold":
                    return railLine;
                default:
                    return railLine;
            }
        }

        private string GetArrow(string direction)
        {
            switch (direction.ToLower())
            {
                case "n":
                    return "&uarr;";
                case "s":
                    return "&darr;";
                case "e":
                    return "&rarr;";
                case "w":
                    return "&larr;";
                default:
                    return "";
            }
        }

        public string HTML
        {
            get { return _html; }
        }

        public string Title
        {
            get { return "Marta Train Arrival Information"; }
        }

        public string Description
        {
            get { return "Always know when to leave to make your train on time. This gadget will display the next train arrivals for the entire Atlanta rail transit system."; }
        }

        public bool InBeta
        {
            get { return false; }
        }

        public bool RequiresValidGoogleAccessToken
        {
            get { return false; }
        }

        public string DefaultSettings
        {
            get { return JsonConvert.SerializeObject(new MartaOptions { SelectedStation = "five%20points" }); }
        }

        public List<GadgetSettingField> SettingsSchema
        {
            get
            {
                return new List<GadgetSettingField>
                {
                    new GadgetSettingField
                    {
                        DataType = SettingDataTypes.Select,
                        DisplayName = "Marta Station",
                        FieldName = "SelectedStation",
                        SelectOptions = _stationOptions
                    }
                };
            }
        }

        public Damage.DataAccess.Models.UserGadget UserGadget { get; set; }
    }
}
