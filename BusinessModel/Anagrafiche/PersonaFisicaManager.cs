using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.Anagrafiche
{
    public class PersonaFisicaManager : MyManagerCSharp.ManagerDB
    {
        public PersonaFisicaManager(string connectionName)
            : base(connectionName)
        {

        }

        public PersonaFisicaManager(System.Data.Common.DbConnection connection)
            : base(connection)
        {
        
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
                string strSQL = "SELECT ID FROM SEGNALAZIONE where CONTATTO_ID = " + personaFisicaId + " or CEDENTE_ID = " + personaFisicaId;
                System.Data.DataTable  dt = _fillDataTable(strSQL);
                foreach (System.Data.DataRow row in dt.Rows)
                {
                    sManager.delete(long.Parse(row["ID"].ToString()));
                }


                _strSQL = "DELETE FROM  INDIRIZZO WHERE CEDENTE_ID = " + personaFisicaId;
                _executeNoQuery(_strSQL);

                _strSQL = "DELETE FROM  DOCUMENTO_IDENTITA WHERE CEDENTE_ID = " + personaFisicaId;
                _executeNoQuery(_strSQL);

                _strSQL = "DELETE FROM  IMPIEGO WHERE CONTATTOID = " + personaFisicaId;
                _executeNoQuery(_strSQL);

                _strSQL = "DELETE FROM  RIFERIMENTO WHERE CONTATTO_ID = " + personaFisicaId;
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
