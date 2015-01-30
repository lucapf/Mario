using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.Anagrafiche.Amministrazione
{
    public class AmministrazioneManager : MyManagerCSharp.ManagerDB
    {
        public AmministrazioneManager(string connectionName)
            : base(connectionName)
        {

        }

        public AmministrazioneManager(System.Data.Common.DbConnection connection)
            : base(connection)
        {

        }

        public void getList(SearchAmministrazione model)
        {
            List<mediatori.Models.Anagrafiche.Amministrazione> risultato;
            risultato = new List<mediatori.Models.Anagrafiche.Amministrazione>();

            _strSQL = " FROM amministrazione as t1 ";
            _strSQL += " join soggetto_giuridico as t2 on t1.soggettoGiuridicoId = t2.id ";
            _strSQL += " join tipo_natura_giuridica as t3 on t1.tipoNaturaGiuridica_id = t3.id";

            System.Data.Common.DbCommand command;
            command = _connection.CreateCommand();

            string strWHERE = "";

            if (!String.IsNullOrEmpty(model.filtroRagioneSociale))
            {
                strWHERE += " AND UPPER(t2.ragioneSociale) like  @NOME";
                _addParameter(command, "@NOME", "%" + model.filtroRagioneSociale.ToUpper().Trim() + "%");
            }

            if (!String.IsNullOrEmpty(model.filtroPartitaIva))
            {
                strWHERE += " AND UPPER(t1.partitaIva) like  @PIVA";
                _addParameter(command, "@PIVA", "%" + model.filtroPartitaIva.ToUpper().Trim() + "%");
            }


            if (!String.IsNullOrEmpty(strWHERE))
            {
                _strSQL += " WHERE (1=1) " + strWHERE;
            }


            string temp;
            //paginazione
            if (model.PageSize > 0 && (_connection is System.Data.SqlClient.SqlConnection))
            {
                temp = "SELECT COUNT(*) " + _strSQL;
                command.CommandText = temp;

                model.TotalRows = int.Parse(_executeScalar(command));
            }


            temp = "SELECT t1.id, t1.partitaIva,  t1.isEnabled, t2.ragioneSociale, t3.descrizione as 'NaturaGiuridica' " + _strSQL;

            if (String.IsNullOrEmpty(model.Sort))
            {
                temp += " ORDER BY t2.ragioneSociale ";
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
                    risultato.Add(getAmministrazione(row));
                }
            }
            else
            {
                foreach (DataRow row in _dt.Rows)
                {
                    risultato.Add(getAmministrazione(row));
                }
            }

            model.Amministrazioni = risultato;

        }


        private mediatori.Models.Anagrafiche.Amministrazione getAmministrazione(DataRow row)
        {
            mediatori.Models.Anagrafiche.Amministrazione amministrazione;
            amministrazione = new mediatori.Models.Anagrafiche.Amministrazione();

            amministrazione.id = int.Parse(row["id"].ToString());
            amministrazione.partitaIva = row["partitaIva"].ToString();
            amministrazione.soggettoGiuridico = new mediatori.Models.Anagrafiche.SoggettoGiuridico();
            amministrazione.soggettoGiuridico.ragioneSociale = row["ragioneSociale"].ToString();

            amministrazione.tipoNaturaGiuridica = new mediatori.Models.Anagrafiche.TipoNaturaGiuridica();
            amministrazione.tipoNaturaGiuridica.descrizione = row["NaturaGiuridica"].ToString();

            amministrazione.isEnabled = bool.Parse(row["isEnabled"].ToString());
            return amministrazione;
        }

    }
}
