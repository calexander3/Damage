using System.Collections.Generic;
using Damage.Gadget;
using Newtonsoft.Json;
using System;
using System.ServiceModel.Syndication;
using System.Xml;
using System.Web;
using System.IO;

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

                        reader = XmlReader.Create(settings.FeedURL);
                        var document = new XmlDocument();
                        document.Load(reader);
                        HttpContext.Current.Cache.Insert(settings.FeedURL, document.InnerXml, null, DateTime.Now.AddMinutes(30), System.Web.Caching.Cache.NoSlidingExpiration);
                        reader.Close();
                        reader.Dispose();
                        reader = XmlReader.Create(new StringReader(document.InnerXml));
                    }



                    SyndicationFeed feed =  SyndicationFeed.Load(reader);
                    reader.Close();

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
                        output.Append("<div><a href='" + item.Links[0].Uri.ToString() + "' target='_blank' >" + item.Title.Text + "</a></div>");
                        if (item.Content != null)
                        {
                            output.Append("<div style='display:none'>" + ((TextSyndicationContent)item.Content).Text + "</div>");
                        }
                        else if (item.Summary != null)
                        {
                            output.Append("<div style='display:none'>" + item.Summary.Text + "</div>");
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
                        new GadgetSettingField(){FieldName="ItemsToDisplay", DisplayName="Items To Display", DataType= SettingDataTypes.Number,  Validators = Validators.Required | Validators.Number },
                        new GadgetSettingField(){FieldName="ExpandItemsByDefault", DisplayName="Expand Items By Default", DataType= SettingDataTypes.Checkbox }
                    };
            }
        }


        public string Description
        {
            get { return "A simple RSS/ATOM reader gadget."; }
        }
    }
}
