using System.Collections.Generic;
using Damage.Gadget;
using Newtonsoft.Json;
using System;
using System.ServiceModel.Syndication;
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

                    XmlReader reader = XmlReader.Create(settings.FeedURL);
                    SyndicationFeed feed = SyndicationFeed.Load(reader);
                    reader.Close();

                    _title = feed.Title.Text;

                    var counter = 0;
                    foreach (SyndicationItem item in feed.Items)
                    {
                        output.Append("<div>" + item.Title.Text + "</div>");
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
                        new GadgetSettingField(){FieldName="FeedURL", DisplayName="Feed Url", DataType= SettingDataTypes.Url },
                        new GadgetSettingField(){FieldName="ItemsToDisplay", DisplayName="Items To Display", DataType= SettingDataTypes.Number },
                        new GadgetSettingField(){FieldName="ExpandItemsByDefault", DisplayName="Expand Items By Default", DataType= SettingDataTypes.Checkbox }
                    };
            }
        }


        public string Description
        {
            get { return "A simple RSS/ATOM reader gadget"; }
        }
    }
}
