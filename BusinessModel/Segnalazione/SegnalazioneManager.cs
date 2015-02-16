using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.Segnalazione
{
    public class SegnalazioneManager : MyManagerCSharp.ManagerDB
    {
        public SegnalazioneManager(string connectionName)
            : base(connectionName)
        {

        }

        public SegnalazioneManager(System.Data.Common.DbConnection connection)
            : base(connection)
        {

        }

        public bool updateStato(int codiceStato, int codiceEntita, DateTime? dataPromemoria)
        {
            System.Data.Common.DbCommand command;
            command = _connection.CreateCommand();

            _strSQL = "update Segnalazione set statoId=" + codiceStato;
            if (dataPromemoria != null)
            {
                _strSQL += " , dataPromemoria = @DATAPROMEMORIA ";
                _addParameter(command, "@DATAPROMEMORIA", dataPromemoria);
            }

            _strSQL += " WHERE id=" + codiceEntita;

            command.CommandText = _strSQL;
            _executeNoQuery(command);

            return true;
        }

        public int deleteAllSegnalazioni()
        {
            _strSQL = "SELECT ID FROM SEGNALAZIONE";

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

        public bool delete(long segnalazioneId)
        {
            int conta;
            bool externalTransaction = true;
            try
            {
                if (_transaction == null)
                {
                    externalTransaction = false;
                    _transactionBegin();
                }

                _strSQL = "DELETE FROM  NOTA WHERE segnalazione_id = " + segnalazioneId;
                _executeNoQuery(_strSQL);

                _strSQL = "DELETE FROM  PREVENTIVO WHERE segnalazioneId = " + segnalazioneId;
                _executeNoQuery(_strSQL);

                _strSQL = "DELETE FROM  ASSEGNAZIONE WHERE segnalazioneId = " + segnalazioneId;
                _executeNoQuery(_strSQL);

                _strSQL = "DELETE FROM  DOCUMENTO WHERE segnalazioneId = " + segnalazioneId;
                _executeNoQuery(_strSQL);

                _strSQL = "DELETE FROM  SEGNALAZIONE WHERE Id = " + segnalazioneId;
                conta = _executeNoQuery(_strSQL);

                if (externalTransaction == false)
                {
                    _transactionCommit();
                }
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
