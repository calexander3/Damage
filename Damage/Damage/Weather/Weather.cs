using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Damage.Gadget;
using Newtonsoft.Json;

namespace Weather
{
    public class Weather : IGadget
    {
        public void Initialize()
        {

        }

        public string HTML
        {
            get
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
                return "<iframe id='forecast_embed' type='text/html' frameborder='0' height='245' width='100%' src='http://forecast.io/embed/#lat=42.3583&lon=-71.0603&name=Downtown%20Boston&font=Segoe%20UI&units=" + units + "'> </iframe>";
            }
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
                    ZipCode = 30020,
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
                        new GadgetSettingField(){FieldName="ZipCode", DisplayName="Zip Code", DataType= SettingDataTypes.Text, Validators = Validators.Number },
                        new GadgetSettingField(){FieldName="Latitude", DisplayName="Latitude", DataType= SettingDataTypes.Number, Validators = Validators.Number },
                        new GadgetSettingField(){FieldName="Longitude", DisplayName="Longitude", DataType= SettingDataTypes.Number, Validators = Validators.Number},
                        new GadgetSettingField(){FieldName="USUnits", DisplayName="US Units", DataType= SettingDataTypes.Radio},
                        new GadgetSettingField(){FieldName="UKUnits", DisplayName="UK Units", DataType= SettingDataTypes.Radio}
                    };
            }
        }

        public Damage.DataAccess.Models.UserGadget UserGadget { get; set; }
    }
}
