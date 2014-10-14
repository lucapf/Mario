using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Web.WebPages.OAuth;
using mediatori.Models.Anagrafiche;
using WebMatrix.WebData;

namespace mediatori
{
    public static class AuthConfig
    {
        public static void RegisterAuth()
        {
            if (!WebSecurity.Initialized)
                WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserId", "UserName", autoCreateTables: true);
            // Per consentire agli utenti di questo sito di accedere utilizzando account di altri siti quali Microsoft, Facebook e Twitter,
            // è necessario eseguire l'aggiornamento del sito. Per ulteriori informazioni, visitare http://go.microsoft.com/fwlink/?LinkID=252166

            //OAuthWebSecurity.RegisterMicrosoftClient(
            //    clientId: "",
            //    clientSecret: "");

            //OAuthWebSecurity.RegisterTwitterClient(
            //    consumerKey: "",
            //    consumerSecret: "");

            //OAuthWebSecurity.RegisterFacebookClient(
            //    appId: "",
            //    appSecret: "");

            //OAuthWebSecurity.RegisterGoogleClient();
        }
    }
}
