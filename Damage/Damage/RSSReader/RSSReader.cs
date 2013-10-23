using Newtonsoft.Json;
using System;
using System.ServiceModel.Syndication;
using System.Xml;

namespace RSSReader
{
    public class RSSReader : Damage.IGadget
    {
        string _title = "RSS Reader";

        string _output = "";

        public void Initialize()
        {
            _output = GenerateHTML();
        }

        private string GenerateHTML()
        {
            if (UserGadget.GadgetSettings != null && UserGadget.GadgetSettings.Length > 0)
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
                        if (counter >= settings.FeedsToDisplay)
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
                    FeedsToDisplay = 3,
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
    }
}
