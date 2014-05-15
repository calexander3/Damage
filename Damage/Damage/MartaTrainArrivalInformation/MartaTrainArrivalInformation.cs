using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Damage.Gadget;
using Newtonsoft.Json;

namespace MartaTrainArrivalInformation
{
    public class MartaTrainArrivalInformation : IGadget
    {
        private string _html;


        public void Initialize()
        {


            List<TrainSchedule> scheduleList;

            if (HttpContext.Current.Cache[UserGadget.UserGadgetId + "_martaSchedule"] != null)
            {
                scheduleList = (List<TrainSchedule>)HttpContext.Current.Cache[UserGadget.UserGadgetId + "_martaSchedule"];
            }
            else
            {
                var scheduleRequest = (HttpWebRequest)WebRequest.Create("http://developer.itsmarta.com/NextTrainService/RestServiceNextTrain.svc/GetNextTrain/five%20points");

                using (var scheduleResponce = scheduleRequest.GetResponse())
                {
                    scheduleList = JsonConvert.DeserializeObject<List<TrainSchedule>>(new System.IO.StreamReader(scheduleResponce.GetResponseStream()).ReadToEnd());
                    HttpContext.Current.Cache.Insert(UserGadget.UserGadgetId + "_martaSchedule", scheduleList, null, DateTime.Now.AddSeconds(60), System.Web.Caching.Cache.NoSlidingExpiration);
                }
            }

            var outputBuilder = new StringBuilder("<div style='margin-left:2px;margin-right:2px;margin-top:4px;margin-bottom:1px;overflow:hidden;'><div style='font-weight:bold;font-size:2em;margin-left:4px;'>Five Points</div>");
            if (scheduleList != null)
            {
                outputBuilder.Append("<div style='float:left'><table style='margin: 0; border-collapse: separate; border-spacing: 3px;'>");
                foreach (var schedule in scheduleList.Take(8))
                {
                    int min;
                    outputBuilder.Append("<tr><td style='padding:0px 50px 0px 3px;margin:1px 15px 1px 3px;font-weight:bold;background-color:" + schedule.ROUTE.ToLower() + "'>" + schedule.HEAD_SIGN + "</td><td style='padding-left:3px;'>" + (int.TryParse(schedule.WAITING_TIME, out min) ? (schedule.WAITING_TIME + " Minutes") : schedule.WAITING_TIME) + "</td></tr>");
                }
                outputBuilder.Append("</table></div>");
                if (scheduleList.Count > 8)
                {
                    outputBuilder.Append("<div style='float:left'><table style='margin: 0; border-collapse: separate; border-spacing: 3px;'>");
                    foreach (var schedule in scheduleList.Skip(8))
                    {
                        int min;
                        outputBuilder.Append("<tr><td style='padding:0px 50px 0px 3px;margin:1px 15px 1px 3px;font-weight:bold;background-color:" + schedule.ROUTE.ToLower() + "'>" + schedule.HEAD_SIGN + "</td><td style='padding-left:3px;'>" + (int.TryParse(schedule.WAITING_TIME, out min) ? (schedule.WAITING_TIME + " Minutes") : schedule.WAITING_TIME) + "</td></tr>");
                    }
                    outputBuilder.Append("</table></div>");
                }
            }


            outputBuilder.Append("<div style='clear:both'></div></div>");
            _html = outputBuilder.ToString();
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
            get { return ""; }
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
            get { return ""; }
        }

        public List<GadgetSettingField> SettingsSchema
        {
            get { return new List<GadgetSettingField>(); }
        }

        public Damage.DataAccess.Models.UserGadget UserGadget { get; set; }
    }
}
