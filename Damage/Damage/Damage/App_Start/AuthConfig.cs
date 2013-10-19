using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Web.WebPages.OAuth;
using Damage.Models;

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
