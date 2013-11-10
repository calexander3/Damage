using Damage.Gadget;
using System.Collections.Generic;

namespace GoogleDrive
{
    public class GoogleDrive:IGadget
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
            get { return "Google Drive"; }
        }

        public string Description
        {
            get { return "Access your recent documents and create new ones."; }
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
