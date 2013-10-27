using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Damage.Gadget;

namespace Weather
{
    public class Weather : IGadget
    {
        public void Initialize()
        {

        }

        public string HTML
        {
            get { return ""; }
        }

        public string Title
        {
            get { return "Weather"; }
        }

        public string Description
        {
            get { return "View the local weather from the location of your choosing."; }
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
