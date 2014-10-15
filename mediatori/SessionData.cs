using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mediatori
{
    public class SessionData
    {
        public string Login { get; set; }

        private System.Net.Mail.MailAddress _email;
        public string Email
        {
            set
            {
                _email = new System.Net.Mail.MailAddress(value);
            }

            get
            {
                return _email.ToString();
            }
        }

        public string Roles { get; set; }

        public string ProfiloId { get; set; }

        public long UserId
        {
            set
            {
                _userId = value;
            }
            get
            {
                return _userId;
            }
        }

        public System.Security.Principal.SecurityIdentifier SID { get { return _sid; } }

        private long _userId = -1;
        private System.Security.Principal.SecurityIdentifier _sid;

        public SessionData()
        {

        }

        public SessionData(long userId)
        {
            _userId = userId;
        }

        public SessionData(System.Security.Principal.SecurityIdentifier sid)
        {
            _sid = sid;
        }

        public bool IsAuthenticated
        {
            get
            {
                return (_userId != -1) || (_sid != null);
            }
        }

        public bool IsInRole(string role)
        {
            if (String.IsNullOrEmpty(Roles))
            {
                return false;
            }

            bool esito;
            esito = Roles.IndexOf(role.ToUpper() + ";") != -1;

            //Debug.WriteLine("MySessionData IsInRole: " + role + ": " + esito);
            return esito;
        }





        public void LogOff()
        {
            _userId = -1;
            _sid = null;
            Roles = "";
            Login = "";

            ProfiloId = "";

        }

    }
}
