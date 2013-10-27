using Damage.Gadget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCalendar
{
    public class GoogleCalendar: IGadget
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
            get { return "Google Calendar"; }
        }

        public string Description
        {
            get { return "An agenda of your upcoming calendar events."; }
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
