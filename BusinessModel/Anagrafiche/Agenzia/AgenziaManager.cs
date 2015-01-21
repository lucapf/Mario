using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.Anagrafiche.Agenzia
{
    public class AgenziaManager : MyManagerCSharp.ManagerDB
    {
        public AgenziaManager(string connectionName)
            : base(connectionName)
        {

        }

        public AgenziaManager(System.Data.Common.DbConnection connection)
            : base(connection)
        {

        }

        public void getList(SearchAgenzia model)
        {
            List<mediatori.Models.Anagrafiche.Agenzia> risultato;
            risultato = new List<mediatori.Models.Anagrafiche.Agenzia>();

            _strSQL = " FROM AGENZIA as t1 ";
            _strSQL += " join soggetto_giuridico as t2 on t1.soggettoGiuridico_id = t2.id ";
            _strSQL += " join tipo_natura_giuridica as t3 on t1.tipoNaturaGiuridica_id = t3.id";

            System.Data.Common.DbCommand command;
            command = _connection.CreateCommand();

            //if (model...filter != null && !String.IsNullOrEmpty(model.filter.tipo))
            //{
            //    _strSQL += " WHERE tipo_id = '" + model.filter.tipo + "'";
            //}

            // _strSQL += " order by nome";


            string temp;
            //paginazione
            if (model.PageSize > 0 && (_connection is System.Data.SqlClient.SqlConnection))
            {
                temp = "SELECT COUNT(*) " + _strSQL;
                command.CommandText = temp;

                model.TotalRows = int.Parse(_executeScalar(command));
            }


            temp = "SELECT t1.id, t1.isEnabled, t1.partitaIva, t2.ragioneSociale, t3.descrizione as 'NaturaGiuridica' " + _strSQL;

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
                    risultato.Add(getAgenzia(row));
                }
            }
            else
            {
                foreach (DataRow row in _dt.Rows)
                {
                    risultato.Add(getAgenzia(row));
                }
            }

            model.Agenzie = risultato;

        }


        private mediatori.Models.Anagrafiche.Agenzia getAgenzia(DataRow row)
        {
            mediatori.Models.Anagrafiche.Agenzia agenzia;
            agenzia = new mediatori.Models.Anagrafiche.Agenzia();

            agenzia.id = int.Parse(row["id"].ToString());
            agenzia.partitaIva = row["partitaIva"].ToString();
            agenzia.soggettoGiuridico = new mediatori.Models.Anagrafiche.SoggettoGiuridico();
            agenzia.soggettoGiuridico.ragioneSociale = row["ragioneSociale"].ToString();

            agenzia.tipoNaturaGiuridica = new mediatori.Models.Anagrafiche.TipoNaturaGiuridica();
            agenzia.tipoNaturaGiuridica.descrizione = row["NaturaGiuridica"].ToString();

            agenzia.isEnabled = bool.Parse(row["isEnabled"].ToString());

            return agenzia;
        }

    }
}
