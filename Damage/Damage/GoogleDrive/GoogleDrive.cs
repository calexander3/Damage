using System;
using System.Globalization;
using System.Net;
using Damage.Gadget;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GoogleDrive
{
    public class GoogleDrive:IGadget
    {
        string _output = "";
        public void Initialize()
        {
            var outputBuilder = new System.Text.StringBuilder("<div>");


            outputBuilder.Append("<div><img src='/Content/Images/GoogleDriveDocument.png'><a href='https://docs.google.com/document/' target='_blank'>Create New Document</a></div>");

            UsageInfo usageInfo = null;
            var usageRequest = (HttpWebRequest) WebRequest.Create("https://www.googleapis.com/drive/v2/about");
            usageRequest.Headers["Authorization"] = string.Format("Bearer {0}", UserGadget.User.CurrentOAuthAccessToken);
            using (var usageResponse = usageRequest.GetResponse())
            {
                usageInfo = JsonConvert.DeserializeObject<UsageInfo>(new System.IO.StreamReader(usageResponse.GetResponseStream()).ReadToEnd());
            }

            if (usageInfo != null)
            {
                outputBuilder.Append("<div>" +
                                     Math.Round(
                                         ((double.Parse(usageInfo.quotaBytesUsedAggregate)/
                                           double.Parse(usageInfo.quotaBytesTotal))*100), 2) + "% Space Used</div>");
            }


            outputBuilder.Append("</div>");
            _output = outputBuilder.ToString();
        }

        public string HTML
        {
            get { return _output; }
        }

        public string Title
        {
            get { return "Google Drive"; }
        }

        public string Description
        {
            get { return "Access your recent documents and create new ones."; }
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
