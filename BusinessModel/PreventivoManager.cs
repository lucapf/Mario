using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel
{
    public class PreventivoManager : MyManagerCSharp.ManagerDB
    {
        public PreventivoManager(string connectionName)
            : base(connectionName)
        {

        }

        public PreventivoManager(System.Data.Common.DbConnection connection)
            : base(connection)
        {

        }



        public List<mediatori.Models.Preventivo> getPreventiviBySegnalazione(int segnalazioneId)
        {
            _strSQL = "select * from preventivo where segnalazioneId = " + segnalazioneId;

            _dt = _fillDataTable(_strSQL);
            List<mediatori.Models.Preventivo> risultato = new List<mediatori.Models.Preventivo>();

            mediatori.Models.Preventivo preventivo;

            foreach (System.Data.DataRow row in _dt.Rows)
            {
                preventivo = _getPreventivo(row);

                risultato.Add(preventivo);
            }

            return risultato;
        }


        private mediatori.Models.Preventivo _getPreventivo(System.Data.DataRow row)
        {
            mediatori.Models.Preventivo preventivo = new mediatori.Models.Preventivo();

            if (!String.IsNullOrEmpty(row["assicurazioneImpiegoId"].ToString()))
            {
                preventivo.assicurazioneImpiegoId = int.Parse(row["assicurazioneImpiegoId"].ToString());
            }

            if (!String.IsNullOrEmpty(row["assicurazioneVitaId"].ToString()))
            {
                preventivo.assicurazioneVitaId = int.Parse(row["assicurazioneVitaId"].ToString());
            }
            preventivo.bySimulatore = bool.Parse(row["bySimulatore"].ToString());
            if (!(row["dataConferma"] is DBNull))
            {
                preventivo.dataConferma = DateTime.Parse(row["dataConferma"].ToString());
            }

            if (!(row["dataDecorrenza"] is DBNull))
            {
                preventivo.dataDecorrenza = DateTime.Parse(row["dataDecorrenza"].ToString());
            }
            preventivo.dataInserimento = DateTime.Parse(row["dataInserimento"].ToString());
            preventivo.durata = int.Parse(row["durata"].ToString());
            if (!String.IsNullOrEmpty(row["finanziariaId"].ToString()))
            {
                preventivo.finanziariaId = int.Parse(row["finanziariaId"].ToString());
            }
            preventivo.id = int.Parse(row["id"].ToString());
            preventivo.importoCoperturaImpiego = decimal.Parse(row["importoCoperturaImpiego"].ToString());
            preventivo.importoCoperturaVita = decimal.Parse(row["importoCoperturaVita"].ToString());
            if (!(row["importoImpegniDaEstinguere"] is DBNull))
            {
                preventivo.importoImpegniDaEstinguere = decimal.Parse(row["importoImpegniDaEstinguere"].ToString());
            }

            if (!(row["importoInteressi"] is DBNull))
            {
                preventivo.importoInteressi = decimal.Parse(row["importoInteressi"].ToString());
            }
            preventivo.importoProvvigioni = decimal.Parse(row["importoProvvigioni"].ToString());
            preventivo.importoRata = decimal.Parse(row["importoRata"].ToString());
            preventivo.montante = decimal.Parse(row["montante"].ToString());
            preventivo.nettoCliente = decimal.Parse(row["nettoCliente"].ToString());
            preventivo.nomeProdotto = row["nomeProdotto"].ToString();
            if (!(row["oneriFiscali"] is DBNull))
            {
                preventivo.oneriFiscali = decimal.Parse(row["oneriFiscali"].ToString());
            }
            preventivo.progressivo = int.Parse(row["progressivo"].ToString());
            preventivo.segnalazioneId = int.Parse(row["segnalazioneId"].ToString());
            if (!(row["speseAttivazione"] is DBNull))
            {
                preventivo.speseAttivazione = decimal.Parse(row["speseAttivazione"].ToString());
            }
            if (!(row["speseIncasso"] is DBNull))
            {
                preventivo.speseIncasso = decimal.Parse(row["speseIncasso"].ToString());
            }
            preventivo.tabellaFinanziaria = row["tabellaFinanziaria"].ToString();
            preventivo.taeg = decimal.Parse(row["taeg"].ToString());
            preventivo.teg = decimal.Parse(row["teg"].ToString());
            preventivo.tan = decimal.Parse(row["tan"].ToString());


            return preventivo;
        }
    }
}
