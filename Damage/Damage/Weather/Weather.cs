using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Damage.Gadget;
using Newtonsoft.Json;

namespace Weather
{
    public class Weather : IGadget
    {
        string _output = "";
        public void Initialize()
        {
            var settings = JsonConvert.DeserializeObject<WeatherOptions>(UserGadget.GadgetSettings);
            var units = "";
            if (settings.USUnits)
            {
                units = "us";
            }
            else if (settings.UKUnits)
            {
                units = "uk";
            }

            if ((!settings.Latitude.HasValue || !settings.Longitude.HasValue) && settings.CityName.Length > 0)
            {
                double lat = 0;
                double lng = 0;
                var location = "";

                if (HttpContext.Current.Cache[settings.CityName] != null)
                {
                    var latlng = (string[])HttpContext.Current.Cache[settings.CityName];
                    lat = double.Parse(latlng[0]);
                    lng = double.Parse(latlng[1]);
                    location = latlng[2];
                }
                else
                {
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://maps.googleapis.com/maps/api/geocode/json?address=" + settings.CityName + "&sensor=false");
                    using (var response = (HttpWebResponse)httpWebRequest.GetResponse())
                    {
                        using (var streamReader = new StreamReader(response.GetResponseStream(), true))
                        {
                            var addresses = JsonConvert.DeserializeObject<GoogleGeoCodeResponse>(streamReader.ReadToEnd());
                            if (addresses.status == "OK" && addresses.results.Length > 0)
                            {
                                location = HttpUtility.HtmlEncode(addresses.results[0].formatted_address);
                                lat = double.Parse(addresses.results[0].geometry.location.lat);
                                lng = double.Parse(addresses.results[0].geometry.location.lng);
                                HttpContext.Current.Cache.Insert(settings.CityName, new string[] { lat.ToString(), lng.ToString(), location }, null,
                                                                 System.Web.Caching.Cache.NoAbsoluteExpiration,
                                                                 new TimeSpan(365, 0, 0, 0));
                            }
                        }
                    }
                }


                if (lat != 0 && lng != 0)
                {
                    _output = "<iframe id='forecast_embed' type='text/html' frameborder='0' height='245' width='99%' src='https://forecast.io/embed/#lat=" + lat + "&lon=" + lng + "&name=" + location + "&font=Segoe%20UI&units=" + units + "'> </iframe>";
                }
                else
                {
                    _output = "Please select a valid location.";
                }
            }
            else if (settings.Latitude.HasValue && settings.Longitude.HasValue)
            {
                var location = HttpUtility.HtmlEncode(settings.Latitude.Value.ToString() + " by " + settings.Longitude.Value.ToString());
                _output = "<iframe id='forecast_embed' type='text/html' frameborder='0' height='245' width='99%' src='https://forecast.io/embed/#lat=" + settings.Latitude.Value + "&lon=" + settings.Longitude.Value + "&name=" + location + "&font=Segoe%20UI&units=" + units + "'> </iframe>";
            }
            else
            {
                _output = "Please select a valid location.";
            }
        }

        public string HTML
        {
            get { return _output; }
        }

        public string Title
        {
            get { return "Weather"; }
        }

        public string Description
        {
            get { return "View the local weather from the location of your choosing. Powered by <a href='https://forecast.io/'>Forcast.IO</a>"; }
        }

        public string DefaultSettings
        {
            get
            {
                var defaults = new WeatherOptions()
                {
                    CityName = "Atlanta, GA",
                    Latitude = null,
                    Longitude = null,
                    USUnits = true,
                    UKUnits = false
                };

                return JsonConvert.SerializeObject(defaults);
            }
        }

        public List<GadgetSettingField> SettingsSchema
        {
            get
            {
                return new List<GadgetSettingField>()
                    {
                        new GadgetSettingField(){FieldName="CityName", DisplayName="City Name, State", DataType= SettingDataTypes.Text },
                        new GadgetSettingField(){FieldName="Latitude", DisplayName="Latitude", DataType= SettingDataTypes.Number, Validators = Validators.Number },
                        new GadgetSettingField(){FieldName="Longitude", DisplayName="Longitude", DataType= SettingDataTypes.Number, Validators = Validators.Number},
                        new GadgetSettingField(){FieldName="USUnits", DisplayName="Fahrenheit", DataType= SettingDataTypes.Radio},
                        new GadgetSettingField(){FieldName="UKUnits", DisplayName="Celsius", DataType= SettingDataTypes.Radio}
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
            get { return false; }
        }

        public class GoogleGeoCodeResponse
        {

            public String status;
            public results[] results;
        }

        public class results
        {
            public String formatted_address;
            public geometry geometry;
            public String[] types;
            public address_component[] address_components;
        }

        public class geometry
        {
            public bounds bounds;
            public String location_type;
            public location location;
            public bounds viewport;
        }

        public class bounds
        {

            public location northeast;
            public location southwest;
        }

        public class location
        {
            public String lat;
            public String lng;
        }

        public class address_component
        {
            public String long_name;
            public String short_name;
            public String[] types;
        }
    }
}
