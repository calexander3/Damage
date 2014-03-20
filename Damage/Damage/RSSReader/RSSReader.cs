using System.Net;
using Damage;
using Damage.Gadget;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel.Syndication;
using System.Web;
using System.Xml;

namespace RSSReader
{
    public class RSSReader : IGadget
    {
        string _title = "RSS Reader";

        string _output = "";

        public void Initialize()
        {
            _output = GenerateHTML();
        }

        private string GenerateHTML()
        {
            if (!string.IsNullOrEmpty(UserGadget.GadgetSettings))
            {
                var settings = JsonConvert.DeserializeObject<RSSOptions>(UserGadget.GadgetSettings);
                if (Uri.IsWellFormedUriString(settings.FeedURL, UriKind.Absolute))
                {
                    var output = new System.Text.StringBuilder();

                    XmlReader reader = null;
                    if (HttpContext.Current.Cache[settings.FeedURL] != null)
                    {
                        reader = XmlReader.Create(new StringReader((string)HttpContext.Current.Cache[settings.FeedURL]));
                    }
                    else
                    {
                        WebRequest request = WebRequest.Create(settings.FeedURL);
                        request.Timeout = 1500;

                        using (WebResponse response = request.GetResponse())
                        {
                        reader = XmlReader.Create(response.GetResponseStream());
                        var document = new XmlDocument();
                        document.Load(reader);
                        HttpContext.Current.Cache.Insert(settings.FeedURL, document.InnerXml, null, DateTime.Now.AddMinutes(30), System.Web.Caching.Cache.NoSlidingExpiration);
                        reader.Close();
                        reader.Dispose();
                        reader = XmlReader.Create(new StringReader(document.InnerXml));
                        }
                    }

                    SyndicationFeed feed = SyndicationFeed.Load(reader);
                    reader.Close();
                    reader.Dispose();

                    if (feed.Links.Count > 0)
                    {
                        _title = "<a href = '" + feed.Links[0].Uri.ToString() + "' target='_blank' >" + feed.Title.Text + "</a>";
                    }
                    else
                    {
                        _title = feed.Title.Text;
                    }

                    var counter = 0;
                    foreach (SyndicationItem item in feed.Items)
                    {
                        ShortGuid sg = ShortGuid.NewGuid();
                        output.Append(@"<div style='background-repeat:no-repeat;background-position:center;background-image:url(/Content/Images/" + (settings.ExpandItemsByDefault ? "CollapseArrow" : "ExpandArrow") + @".png); width:10px;height:12px;float:left;position:relative;top:7px;left:4px;cursor:pointer;' onclick='if($(""#" + sg.ToString() + @""").css(""display"") == ""none"") {$(""#" + sg.ToString() + @""").css(""display"",""block""); $(this).css(""background-image"",""url(/Content/Images/CollapseArrow.png)"");}else{$(""#" + sg.ToString() + @""").css(""display"",""none""); $(this).css(""background-image"",""url(/Content/Images/ExpandArrow.png)"");}' ></div>");

                        output.Append("<div style='margin-left:15px;margin-bottom:2px;margin-top:3px;'><a style='padding:0px;' href='" + item.Links[0].Uri.ToString() + "' target='_blank' >" + item.Title.Text + "</a></div>");
                        if (item.Content != null)
                        {
                            output.Append("<div id='" + sg.ToString() + "' style='margin-left:14px;display:" + (settings.ExpandItemsByDefault ? "block" : "none") + @"'>" + ((TextSyndicationContent)item.Content).Text + "</div>");
                        }
                        else if (item.Summary != null)
                        {
                            output.Append("<div id='" + sg.ToString() + "' style='margin-left:14px;display:" + (settings.ExpandItemsByDefault ? "block" : "none") + @"'>" + item.Summary.Text + "</div>");
                        }
                        counter++;
                        if (counter >= settings.ItemsToDisplay)
                        {
                            break;
                        }
                    }

                    return output.ToString();
                }
            }

            return "Open setting to configure feed.";
        }

        public Damage.DataAccess.Models.UserGadget UserGadget { get; set; }

        public string Title
        {
            get
            {
                return _title;
            }
        }

        public string DefaultSettings
        {
            get
            {
                var defaults = new RSSOptions()
                {
                    ItemsToDisplay = 3,
                    ExpandItemsByDefault = false,
                    FeedURL = ""
                };

                return JsonConvert.SerializeObject(defaults);
            }
        }


        public string HTML
        {
            get { return _output; }
        }


        public List<GadgetSettingField> SettingsSchema
        {
            get
            {
                return new List<GadgetSettingField>()
                    {
                        new GadgetSettingField(){FieldName="FeedURL", DisplayName="Feed Url", DataType= SettingDataTypes.Url, Validators = Validators.Required | Validators.Url },
                        new GadgetSettingField(){FieldName="ItemsToDisplay", DisplayName="Items To Display", DataType= SettingDataTypes.Number,  Validators = Validators.Required | Validators.Integer },
                        new GadgetSettingField(){FieldName="ExpandItemsByDefault", DisplayName="Expand Items By Default", DataType= SettingDataTypes.Checkbox }
                    };
            }
        }


        public string Description
        {
            get { return "A simple RSS/ATOM reader gadget."; }
        }

        public bool InBeta
        {
            get { return false; }
        }

        public bool RequiresValidGoogleAccessToken
        {
            get { return false; }
        }
    }
}
