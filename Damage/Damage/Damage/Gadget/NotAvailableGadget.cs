using System.Collections.Generic;

namespace Damage.Gadget
{
    public class NotAvailableGadget : IGadget
    {
        public string HTML
        {
            get { return @"<div>Gadget Is Unavaliable</div>"; }
        }

        public DataAccess.Models.UserGadget UserGadget { get; set; }


        public string Title
        {
            get
            { 
                return UserGadget.Gadget.GadgetTitle; 
            }
        }


        public string DefaultSettings
        {
            get { return ""; }
        }

        public void Initialize()
        {
        }


        public List<GadgetSettingField> SettingsSchema
        {
            get { return new List<GadgetSettingField>(); }
        }


        public string Description
        {
            get { return ""; }
        }
    }
}