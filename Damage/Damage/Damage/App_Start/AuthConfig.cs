using Microsoft.Web.WebPages.OAuth;

namespace Damage
{
    public static class AuthConfig
    {
        public static void RegisterAuth()
        {
            //Register Google OAuth
            OAuthWebSecurity.RegisterGoogleClient();
        }
    }
}
