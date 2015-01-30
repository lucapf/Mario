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
        //  private string _urlSimulazioneFinanziaria;

        private mediatori.Models.Istituto _istituto;
        private MyUsers.Models.MyCredenziali _credenziali;


        public SessionData(long userId, string dominio, string connectionString)
            : base(userId)
        {
            _dominio = dominio;
            _connectionString = connectionString;
            //            _urlSimulazioneFinanziaria = System.Configuration.ConfigurationManager.AppSettings["pcc.url"];
        }

        public string ConnectionString { get { return _connectionString; } }

        public string Dominio { get { return _dominio; } }
        public mediatori.Models.Istituto Istituto
        {
            get { return _istituto; }
            set { _istituto = value; }
        }


        public MyUsers.Models.MyCredenziali CredenzialiCreditoLab
        {
            get { return _credenziali; }
            set { _credenziali = value; }
        }

        // public string UrlSimulazioneFinanziaria { get { return _urlSimulazioneFinanziaria; } }

        public override void LogOff()
        {
            base.LogOff();

            _dominio = "";
            _istituto = null;
            _credenziali = null;
            //_urlSimulazioneFinanziaria = "";
            // _connectionString = "";
        }
    }
}
