using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.Sicurezza
{
    public class UtentiManager   : MyManagerCSharp.ManagerDB
    {
        public UtentiManager(string connectionName)
            : base(connectionName)
        {

        }

        public UtentiManager(System.Data.Common.DbConnection connection)
            : base(connection)
        {

        }


        public bool delete(long userId)
        {
            _strSQL = "DELETE FROM UtenteGruppo WHERE user_id = " + userId;
            _executeNoQuery(_strSQL);


            _strSQL = "DELETE FROM UtenteProfilo WHERE user_id = " + userId;
            _executeNoQuery(_strSQL);

            //Cancello l'integrità referenziale sull DB tra la tabella dei Logs e la tabella Utente in modo da poter conservare i log degli utenti eliminati
            //_strSQL = "DELETE FROM MyLogUser WHERE user_id = " + userId;
            //_executeNoQuery(_strSQL);



            //Aggiungo le tabelle di TechAgent!!

            _strSQL = "DELETE FROM UtenteCredenzialiIstituto WHERE user_id = " + userId;
            _executeNoQuery(_strSQL);


            _strSQL = "DELETE FROM UTENTE WHERE user_id = " + userId;
            return _executeNoQuery(_strSQL) == 1;
        }


    }
}
