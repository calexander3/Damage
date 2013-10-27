using Damage.Gadget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodayInHistory
{
    public class TodayInHistory : IGadget
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
            get { return "Today In History"; }
        }

        public string Description
        {
            get { return "Expand your knowledge with fun facts from this day in History."; }
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
