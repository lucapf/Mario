using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel
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

            _strSQL = "update Segnalazione set stato_id=" + codiceStato;
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
    }
}
