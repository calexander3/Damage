using Damage.Gadget;
using ImapX;
using ImapX.Authentication;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Gmail
{
    public class Gmail : IGadget
    {
        string _output = "";
        public void Initialize()
        {


            //var emailaddress = "craig.d.alexander@gmail.com";
            //var oauthtoken = "xxx";

            //var client = new ImapClient("imap.gmail.com", true);

            //if (client.Connect())
            //{

            //    var credentials = new OAuth2Credentials(emailaddress, oauthtoken);

            //    if (client.Login(credentials))
            //    {
            //        _output = "login successful";
            //    }

            //}
            //else
            //{
            //    _output = "connection not successful";
            //}
        }

        public string HTML
        {
            get { return _output; }
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

        public bool InBeta
        {
            get { return true; }
        }
    }
}
