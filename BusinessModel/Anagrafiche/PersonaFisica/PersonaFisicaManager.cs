using BusinessModel.Segnalazione;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.Anagrafiche.PersonaFisica
{
    public class PersonaFisicaManager : MyManagerCSharp.ManagerDB
    {
        public enum TipoPersonaFisica
        {
            Contatto,
            Cedente
        }

        public PersonaFisicaManager(string connectionName)
            : base(connectionName)
        {

        }

        public PersonaFisicaManager(System.Data.Common.DbConnection connection)
            : base(connection)
        {
        
        }

        public void getList(SearchPersonaFisica model)
        {
            List<mediatori.Models.Anagrafiche.PersonaFisica> risultato;
            risultato = new List<mediatori.Models.Anagrafiche.PersonaFisica>();

            _strSQL = " FROM PERSONA_FISICA as t1 ";
            _strSQL += " WHERE t1.tipoPersonaFisica = '" + model.tipoPersonaFisica.ToString() + "'";

            System.Data.Common.DbCommand command;
            command = _connection.CreateCommand();

            string temp;
            //paginazione
            if (model.PageSize > 0 && (_connection is System.Data.SqlClient.SqlConnection))
            {
                temp = "SELECT COUNT(*) " + _strSQL;
                command.CommandText = temp;

                model.TotalRows = int.Parse(_executeScalar(command));
            }


            //SELECT con ORDINAMENTO
            temp = "SELECT * " + _strSQL;

            if (String.IsNullOrEmpty(model.Sort))
            {
                temp += " ORDER BY t1.cognome ";
            }
            else
            {
                temp += " ORDER BY " + model.Sort + " " + model.SortDir;
            }


            if (model.PageSize > 0 && (_connection is System.Data.SqlClient.SqlConnection))
            {
                temp += " OFFSET " + ((model.PageNumber - 1) * model.PageSize) + " ROWS FETCH NEXT " + model.PageSize + " ROWS ONLY";
            }

            command.CommandText = temp;
            command.Connection = _connection;

            _dt = _fillDataTable(command);

            if (model.PageSize > 0 && !(_connection is System.Data.SqlClient.SqlConnection))
            {
                model.TotalRows = _dt.Rows.Count;

                // apply paging
                IEnumerable<DataRow> rows = _dt.AsEnumerable().Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize);
                foreach (DataRow row in rows)
                {
                    risultato.Add(getPersonaFisica(row));
                }
            }
            else
            {
                foreach (DataRow row in _dt.Rows)
                {
                    risultato.Add(getPersonaFisica(row));
                }
            }

            model.PersoneFisiche = risultato;

        }


        private mediatori.Models.Anagrafiche.PersonaFisica getPersonaFisica(DataRow row)
        {
            mediatori.Models.Anagrafiche.PersonaFisica personaFisica;
            personaFisica = new mediatori.Models.Anagrafiche.Contatto();

            personaFisica.id = int.Parse(row["id"].ToString());
            personaFisica.codiceFiscale = row["codiceFiscale"].ToString();
            personaFisica.nome = row["nome"].ToString();
            personaFisica.cognome = row["cognome"].ToString();
            personaFisica.dataNascita = DateTime.Parse(row["dataNascita"].ToString());
            
            return personaFisica;
        }


        public int deleteAllPersoneFisiche()
        {
            _strSQL = "SELECT ID FROM PERSONA_FISICA";

            _dt = _fillDataTable(_strSQL);
            int conta = 0;

            foreach (System.Data.DataRow row in _dt.Rows)
            {
                if (delete(long.Parse(row["ID"].ToString())))
                {
                    conta++;
                }
            }

            return conta;
        }




        public bool delete(long personaFisicaId)
        {
            SegnalazioneManager sManager = new SegnalazioneManager(_connection);

            int conta;
            try
            {
                _transactionBegin();

                sManager._transactionBegin(ref _transaction);
                //Cancello tutte le segnalazioni e pratiche associate alla persona
                string strSQL = "SELECT ID FROM SEGNALAZIONE where CONTATTOID = " + personaFisicaId + " or CEDENTE_ID = " + personaFisicaId;
                System.Data.DataTable  dt = _fillDataTable(strSQL);
                foreach (System.Data.DataRow row in dt.Rows)
                {
                    sManager.delete(long.Parse(row["ID"].ToString()));
                }

                _strSQL = "DELETE FROM  INDIRIZZO WHERE CEDENTEID = " + personaFisicaId;
                _executeNoQuery(_strSQL);

                _strSQL = "DELETE FROM  DOCUMENTO_IDENTITA WHERE CEDENTE_ID = " + personaFisicaId;
                _executeNoQuery(_strSQL);

                _strSQL = "DELETE FROM  IMPIEGO WHERE CONTATTOID = " + personaFisicaId;
                _executeNoQuery(_strSQL);

                _strSQL = "DELETE FROM  RIFERIMENTO WHERE CONTATTOID = " + personaFisicaId;
                conta = _executeNoQuery(_strSQL);

                _strSQL = "DELETE FROM  PERSONA_FISICA WHERE ID = " + personaFisicaId;
                conta = _executeNoQuery(_strSQL);

                _transactionCommit();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception: " + ex.Message);
                _transactionRollback();
                throw ex;
            }
            return (conta == 1);
        }


    }
}
