using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mediatori
{
    public class SessionData : MyManagerCSharp.MySessionData
    {

        private string _dominio;
        private string _connectionString;
        private string _urlSimulazioneFinanziaria;

        public SessionData(long userId, string dominio, string connectionString)
            : base(userId)
        {
            _dominio = dominio;
            _connectionString = connectionString;
            _urlSimulazioneFinanziaria = System.Configuration.ConfigurationManager.AppSettings["pcc.url"];
        }

        public string ConnectionString { get { return _connectionString; } }

        public string Dominio { get { return _dominio; } }

        public string UrlSimulazioneFinanziaria { get { return _urlSimulazioneFinanziaria; } }

        public override void LogOff()
        {
            base.LogOff();

            _dominio = "";
            _urlSimulazioneFinanziaria = "";
           // _connectionString = "";
        }
    }
}
