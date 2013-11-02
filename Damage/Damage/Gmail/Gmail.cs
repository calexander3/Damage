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
            var client = new ImapClient("imap.gmail.com", true);

            if (client.Connect())
            {

                var credentials = new OAuth2Credentials(UserGadget.User.EmailAddress, UserGadget.User.CurrentOAuthAccessToken);

                if (ExecuteWithTimeLimit(new TimeSpan(0,0,5), () => { client.Login(credentials);}))
                {
                    if (client.IsAuthenticated)
                    {
                        _output = "login successful";
                    }
                    else
                    {
                        _output = "connection not successful";
                    }
                }
                else
                {
                    _output = "connection not successful";
                }
            }
            else
            {
                _output = "connection not successful";
            }
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

        public bool RequiresValidGoogleAccessToken
        {
            get { return true; }
        }

        public static bool ExecuteWithTimeLimit(TimeSpan timeSpan, Action codeBlock)
        {
            try
            {
                Task task = Task.Factory.StartNew(() => codeBlock());
                task.Wait(timeSpan);
                return task.IsCompleted;
            }
            catch (AggregateException ae)
            {
                throw ae.InnerExceptions[0];
            }
        }
    }
}
