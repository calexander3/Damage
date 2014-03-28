using System.Collections.Generic;

namespace Damage.Gadget
{
    public class RequiresOAuthGadget : IGadget
    {
        public string HTML
        {
            get
            {
                return
                    @"<div>This gadget requires a linked Google account. You can link your Google account to your Damage account <a href='/Account/Manage' >here</a>.</div>";
            }
        }

        public DataAccess.Models.UserGadget UserGadget { get; set; }


        public string Title
        {
            get { return UserGadget.Gadget.GadgetTitle; }
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

        public bool InBeta
        {
            get { return false; }
        }

        public bool RequiresValidGoogleAccessToken
        {
            get { return false; }
        }
    }
}