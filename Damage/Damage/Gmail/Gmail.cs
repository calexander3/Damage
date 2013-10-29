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
            //var oauthtoken = "AItOawm4BG1g1pWjNFn5f917IqLWZhEj27PpylY";

            //var client = new ImapClient("imap.gmail.com", true);

            //if (client.Connect())
            //{

            //    var credentials = new OAuth2Credentials(emailaddress, getAccessToken(oauthtoken));

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


        private string getAccessToken(string OAuthToken)
        {

            byte[] buffer = Encoding.ASCII.GetBytes("?code=" + OAuthToken + "&client_id=14362457062.apps.googleusercontent.com&client_secret=AAHirbGW44QCmGG1VUSqvV2c&redirect_uri=http://damage.io&grant_type=authorization_code");
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://accounts.google.com/o/oauth2/token");
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            req.ContentLength = buffer.Length;

            Stream strm = req.GetRequestStream();
            strm.Write(buffer, 0, buffer.Length);
            strm.Close();

            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            StreamReader responseStreamReader = new StreamReader(resp.GetResponseStream());
            var result = responseStreamReader.ReadToEnd();
            return "";
        }
    }
}
