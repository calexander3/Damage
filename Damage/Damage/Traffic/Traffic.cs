using Damage.Gadget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Traffic
{
    public class Traffic : IGadget
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
            get { return "Traffic"; }
        }

        public string Description
        {
            get { return "Check to see if the way home is clear."; }
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
            get { return true; }
        }

        public bool RequiresValidGoogleAccessToken
        {
            get { return false; }
        }
    }
}
