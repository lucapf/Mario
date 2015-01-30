using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
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

        public int deleteAllAgenzie()
        {
            _strSQL = "SELECT ID FROM AGENZIA";

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

        public bool delete(long agenziaId)
        {

            int conta;
            try
            {
                _transactionBegin();

                _strSQL = "DELETE FROM  AGENZIA WHERE ID = " + agenziaId;

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


        public List<mediatori.Models.Anagrafiche.SoggettoGiuridico> getSoggettoGiuridicoByRagioneSociale(string sagioneSociale)
        {
            SoggettoGiuridico.SoggettoGiuridicoManager sManager = new SoggettoGiuridico.SoggettoGiuridicoManager(_connection);

            return sManager.getSoggettoGiuridico(sagioneSociale, "", mediatori.Models.EnumEntitaRiferimento.AGENZIA.ToString());

        }

        public List<mediatori.Models.Anagrafiche.SoggettoGiuridico> getSoggettoGiuridicoByCF(string CF)
        {
            SoggettoGiuridico.SoggettoGiuridicoManager sManager = new SoggettoGiuridico.SoggettoGiuridicoManager(_connection);

            return sManager.getSoggettoGiuridico("", CF, mediatori.Models.EnumEntitaRiferimento.AGENZIA.ToString());

        }


        public List<mediatori.Models.Anagrafiche.Agenzia> getAgenziaByPIVA(string partitaIva)
        {
            System.Data.Common.DbCommand command;
            command = _connection.CreateCommand();

            _strSQL = "SELECT * FROM AGENZIA as t1 ";
            
            _strSQL += " WHERE UPPER(t1.partitaIva) =  @PIVA";
            _addParameter(command, "@PIVA", partitaIva.ToUpper().Trim());
            

            command.CommandText = _strSQL;
            _dt = _fillDataTable(command);

            if (_dt.Rows.Count == 0)
            {
                return null;
            }

            List<mediatori.Models.Anagrafiche.Agenzia> risultato = new List<mediatori.Models.Anagrafiche.Agenzia>();

            foreach (System.Data.DataRow row in _dt.Rows)
            {
                risultato.Add(getAgenzia(row));
            }

            return risultato;

        }


    }
}
