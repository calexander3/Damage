using DotNetOpenAuth.GoogleOAuth2;
using Microsoft.Web.WebPages.OAuth;
using System.Collections.Generic;

namespace Damage
{
    public static class AuthConfig
    {
        public static void RegisterAuth()
        {
            //Register Google OAuth
           // OAuthWebSecurity.RegisterGoogleClient();
            var client = new GoogleOAuth2Client("14362457062.apps.googleusercontent.com", "AAHirbGW44QCmGG1VUSqvV2c", 
                new[] { "userinfo.profile", 
                        "userinfo.email", 
                        "https://mail.google.com/mail/feed/atom",
                        "https://mail.google.com/", 
                        "calendar.readonly",
                        "drive.metadata.readonly"});
            var extraData = new Dictionary<string, object>();
            OAuthWebSecurity.RegisterClient(client, "Google", extraData);
        }
    }
}
