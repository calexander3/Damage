using Newtonsoft.Json;
using System;
using System.ServiceModel.Syndication;
using System.Xml;

namespace RSSReader
{
    public class RSSReader : Damage.IGadget
    {
        public string RenderHTML()
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
                return "Feed Title";
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
    }
}
