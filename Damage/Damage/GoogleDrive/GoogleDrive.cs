using System;
using System.Globalization;
using System.Net;
using Damage.Gadget;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Web;
using System.Linq;

namespace GoogleDrive
{
    public class GoogleDrive : IGadget
    {
        string _output = "";
        string _title = "Google Drive";
        public void Initialize()
        {
            _title = "<a href='https://drive.google.com/?tab=wo&authuser=0' target='_blank' >Google Drive</a>";
            var settings = JsonConvert.DeserializeObject<GoogleDriveOptions>(UserGadget.GadgetSettings);

            var outputBuilder = new System.Text.StringBuilder("<div style='margin-left:2px;margin-right:2px;margin-top:4px;margin-bottom:1px;overflow:hidden;'>");


            outputBuilder.Append("<div style='margin-bottom:2px;white-space:nowrap;'><img src='https://ssl.gstatic.com/docs/doclist/images/icon_11_document_list.png' style='margin-right:2px;' /><a style='position:relative;top:-3px;' href='https://docs.google.com/document/' target='_blank'>Create New Document</a></div>");
            outputBuilder.Append("<div style='margin-bottom:2px;white-space:nowrap;'><img src='https://ssl.gstatic.com/docs/doclist/images/icon_11_spreadsheet_list.png' style='margin-right:2px;' /><a style='position:relative;top:-3px;' href='https://docs.google.com/spreadsheet' target='_blank'>Create New Spreadsheet</a></div>");

            if (settings.DisplayRecentFiles && settings.FilesToDisplay > 0)
            {
                FileList fileList = null;

                if (HttpContext.Current.Cache[UserGadget.User.UserId + "_drvFiles_" + settings.FilesToDisplay] != null)
                {
                    fileList = (FileList)HttpContext.Current.Cache[UserGadget.User.UserId + "_drvFiles_" + settings.FilesToDisplay];
                }
                else
                {
                    var recentFilesRequest = (HttpWebRequest)WebRequest.Create("https://www.googleapis.com/drive/v2/files?maxResults=" + (settings.FilesToDisplay + 1).ToString() + "&q=mimeType!%3D%27application/vnd.google-apps.folder%27and%20trashed=false");
                    recentFilesRequest.Headers["Authorization"] = string.Format("Bearer {0}", UserGadget.User.CurrentOAuthAccessToken);
                    using (var recentFilesResponse = recentFilesRequest.GetResponse())
                    {
                        fileList = JsonConvert.DeserializeObject<FileList>(new System.IO.StreamReader(recentFilesResponse.GetResponseStream()).ReadToEnd());
                        HttpContext.Current.Cache.Insert(UserGadget.User.UserId + "_drvFiles_" + settings.FilesToDisplay, fileList, null, DateTime.Now.AddMinutes(5), System.Web.Caching.Cache.NoSlidingExpiration);
                    }
                }

                if (fileList != null)
                {
                    foreach (var file in fileList.items.Take(settings.FilesToDisplay))
                    {
                        outputBuilder.Append("<div style='margin-bottom:2px;white-space:nowrap;'><img src='" + file.iconLink + "' style='margin-right:2px;' /><a style='position:relative;top:-3px;' href='" + file.alternateLink + "' target='_blank' title='" + file.title + "'>" + file.title + "</a></div>");
                    }
                }
            }


            UsageInfo usageInfo = null;

            if (HttpContext.Current.Cache[UserGadget.User.UserId + "_usage"] != null)
            {
                usageInfo = (UsageInfo)HttpContext.Current.Cache[UserGadget.User.UserId + "_usage"];
            }
            else
            {
                var usageRequest = (HttpWebRequest)WebRequest.Create("https://www.googleapis.com/drive/v2/about?maxChangeIdCount=1");
                usageRequest.Headers["Authorization"] = string.Format("Bearer {0}", UserGadget.User.CurrentOAuthAccessToken);
                using (var usageResponse = usageRequest.GetResponse())
                {
                    usageInfo = JsonConvert.DeserializeObject<UsageInfo>(new System.IO.StreamReader(usageResponse.GetResponseStream()).ReadToEnd());
                    HttpContext.Current.Cache.Insert(UserGadget.User.UserId + "_usage", usageInfo, null, DateTime.Now.AddMinutes(60), System.Web.Caching.Cache.NoSlidingExpiration);
                }
            }

            if (usageInfo != null)
            {
                outputBuilder.Append("<div style='margin-top:4px;font-size:80%;'>" +
                                     Math.Round(
                                         ((double.Parse(usageInfo.quotaBytesUsedAggregate) /
                                           double.Parse(usageInfo.quotaBytesTotal)) * 100), 2) + "% Space Used</div>");
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
            get { return _title; }
        }

        public string Description
        {
            get { return "Access your recent documents and create new ones."; }
        }

        public string DefaultSettings
        {
            get { return JsonConvert.SerializeObject(new GoogleDriveOptions() { DisplayRecentFiles = true, FilesToDisplay = 5 }); }
        }

        public List<GadgetSettingField> SettingsSchema
        {
            get
            {
                return new List<GadgetSettingField>() 
                { 
                    new GadgetSettingField() {
                        DisplayName="Display Recent Files", 
                        FieldName="DisplayRecentFiles", 
                        DataType= SettingDataTypes.Checkbox, 
                        Validators= Validators.None
                    },
                    new GadgetSettingField() {
                        DisplayName="Recent Files to Display", 
                        FieldName="FilesToDisplay", 
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
