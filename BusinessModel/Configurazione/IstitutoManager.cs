using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.Configurazione
{
    public class IstitutoManager : MyManagerCSharp.ManagerDB
    {
        public IstitutoManager(string connectionName)
            : base(connectionName)
        {

        }

        public IstitutoManager(System.Data.Common.DbConnection connection)
            : base(connection)
        {

        }


        public mediatori.Models.Istituto getIstitutoByUserId(long userId)
        {
            _strSQL = "select t1.* " +
                " from istituto as t1 " +
                    " join Utente as t2 on t1.id = t2.istituto_id " +
                    " where t2.user_id = " + userId;


            _dt = _fillDataTable(_strSQL);

            if (_dt.Rows.Count == 0)
            {
                return null;
            }

            if (_dt.Rows.Count > 1)
            {
                throw new MyManagerCSharp.MyException("id > 0");
            }

            return _getIstituto(_dt.Rows[0]);
        }


        public MyUsers.Models.MyCredenziali getCredenziali(long userId, long istitutoId)
        {
            _strSQL = "select t1.* " +
                " from [UtenteCredenzialiIstituto] as t1 " +
                    " where t1.user_id = " + userId + " and t1.istituto_id = " + istitutoId;

            _dt = _fillDataTable(_strSQL);

            if (_dt.Rows.Count == 0)
            {
                return null;
            }

            if (_dt.Rows.Count > 1)
            {
                throw new MyManagerCSharp.MyException("id > 0");
            }

            return new MyUsers.Models.MyCredenziali(_dt.Rows[0]["login"].ToString(), _dt.Rows[0]["password"].ToString());
        }


        public bool insertCredenziali(MyUsers.Models.MyCredenziali credenziali, long userId, long istitutoId)
        {
            _strSQL = "INSERT INTO UtenteCredenzialiIstituto ( date_added , user_id ,istituto_id ,[login]  ,[password] )";
            _strSQL += " VALUES ( GetDate() , " + userId + ", " + istitutoId + ", @MY_LOGIN , @MY_PASSWORD )";

            System.Data.Common.DbCommand command;
            command = _connection.CreateCommand();

            _addParameter(command, "@MY_LOGIN", credenziali.Login.Trim());
            _addParameter(command, "@MY_PASSWORD", credenziali.Password.Trim());

            command.CommandText = _strSQL;

            _executeNoQuery(command);

            return true;
        }

        public bool deleteCredenziali(long userId, long istitutoId)
        {
            _strSQL = "DELETE UtenteCredenzialiIstituto WHERE user_id = " + userId + " AND istituto_id =  " + istitutoId ;
            
            System.Data.Common.DbCommand command;
            command = _connection.CreateCommand();

            command.CommandText = _strSQL;

            _executeNoQuery(command);

            return true;
        }



        public List<mediatori.Models.Istituto> getList()
        {
            List<mediatori.Models.Istituto> risultato;
            risultato = new List<mediatori.Models.Istituto>();

            _strSQL = "SELECT * FROM ISTITUTO ORDER BY NOME";

            _dt = _fillDataTable(_strSQL);

            foreach (DataRow row in _dt.Rows)
            {
                risultato.Add(_getIstituto(row));
            }

            return risultato;
        }

        private mediatori.Models.Istituto _getIstituto(DataRow row)
        {
            mediatori.Models.Istituto istituto;
            istituto = new mediatori.Models.Istituto();

            istituto.id = int.Parse(row["id"].ToString());
            istituto.applicativo = row["applicativo"].ToString();
            istituto.nome = row["nome"].ToString();
            istituto.url = row["url"].ToString();
            istituto.dataInserimento = DateTime.Parse(row["dataInserimento"].ToString());

            return istituto;
        }

    }
}
