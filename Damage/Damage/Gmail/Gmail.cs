using Damage.Gadget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gmail
{
    public class Gmail : IGadget
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
            get { return "Gmail"; }
        }

        public string Description
        {
            get { return "A smaller version of your Gmail inbox."; }
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
