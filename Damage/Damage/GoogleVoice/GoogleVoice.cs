using Damage.Gadget;
using System.Collections.Generic;


namespace GoogleVoice
{
    public class GoogleVoice : IGadget
    {
        string _output = "";
        public void Initialize()
        {

        }

        public string HTML
        {
            get { return _output; }
        }

        public string Title
        {
            get { return "Google Voice"; }
        }

        public string Description
        {
            get { return "View your Google Voice messages."; }
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
            get { return true; }
        }
    }
}
