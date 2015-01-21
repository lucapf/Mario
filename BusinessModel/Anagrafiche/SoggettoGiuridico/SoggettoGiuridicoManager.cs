using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mediatori.Models.Anagrafiche;
using System.Data.Entity;

namespace BusinessModel.Anagrafiche.SoggettoGiuridico
{
    public class SoggettoGiuridicoManager : MyManagerCSharp.ManagerDB
    {
        public SoggettoGiuridicoManager(string connectionName)
            : base(connectionName)
        {

        }

        public SoggettoGiuridicoManager(System.Data.Common.DbConnection connection)
            : base(connection)
        {

        }

        public void getList(SearchSoggettoGiuridico model)
        {
            List<mediatori.Models.Anagrafiche.SoggettoGiuridico> risultato;
            risultato = new List<mediatori.Models.Anagrafiche.SoggettoGiuridico>();

            _strSQL = " FROM SOGGETTO_GIURIDICO as t1 ";

            if (model.tipoSoggettoSelezionato == null)
            {
                _strSQL += " WHERE t1.tipoSoggettoGiuridico in " + String.Format(" ('{0}','{1}','{2}') ", mediatori.Models.EnumTipoSoggettoGiuridico.ASSICURAZIONE.ToString(), mediatori.Models.EnumTipoSoggettoGiuridico.FINANZIARIA.ToString(), mediatori.Models.EnumTipoSoggettoGiuridico.GESTORE_FONDO_TFR.ToString());
            }
            else
            {
                _strSQL += " WHERE t1.tipoSoggettoGiuridico = '" + model.tipoSoggettoSelezionato.ToString() + "'";
            }

           

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
                temp += " ORDER BY t1.ragioneSociale ";
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
                    risultato.Add(getSoggettoGiuridico(row));
                }
            }
            else
            {
                foreach (DataRow row in _dt.Rows)
                {
                    risultato.Add(getSoggettoGiuridico(row));
                }
            }

            model.SoggettiGiuridici = risultato;
        }


        private mediatori.Models.Anagrafiche.SoggettoGiuridico getSoggettoGiuridico(DataRow row)
        {
            mediatori.Models.Anagrafiche.SoggettoGiuridico soggettoGiuridico;
            soggettoGiuridico = new mediatori.Models.Anagrafiche.SoggettoGiuridico();

            soggettoGiuridico.id = int.Parse(row["id"].ToString());
            soggettoGiuridico.codiceFiscale = row["codiceFiscale"].ToString();
            soggettoGiuridico.ragioneSociale = row["ragioneSociale"].ToString();
            soggettoGiuridico.tipoSoggettoGiuridico = row["tipoSoggettoGiuridico"].ToString();

            return soggettoGiuridico;
        }




        public static List<mediatori.Models.Anagrafiche.SoggettoGiuridico> getFinanziarie(mediatori.Models.MainDbContext db)
        {
            return _getSoggettoGiuridico(db, mediatori.Models.EnumTipoSoggettoGiuridico.FINANZIARIA);
        }


        public static List<mediatori.Models.Anagrafiche.SoggettoGiuridico> getAssicurazioni(mediatori.Models.MainDbContext db)
        {
            return _getSoggettoGiuridico(db, mediatori.Models.EnumTipoSoggettoGiuridico.ASSICURAZIONE);
        }

        public static List<mediatori.Models.Anagrafiche.SoggettoGiuridico> getGestoriFondoTFR(mediatori.Models.MainDbContext db)
        {
            return _getSoggettoGiuridico(db, mediatori.Models.EnumTipoSoggettoGiuridico.GESTORE_FONDO_TFR);
        }

        private static List<mediatori.Models.Anagrafiche.SoggettoGiuridico> _getSoggettoGiuridico(mediatori.Models.MainDbContext db, mediatori.Models.EnumTipoSoggettoGiuridico tipo)
        {
            IQueryable<mediatori.Models.Anagrafiche.SoggettoGiuridico> listaSoggetti = db.SoggettiGiuridici.Where(s => s.tipoSoggettoGiuridico.Equals(tipo.ToString())).OrderBy(s => s.ragioneSociale);

            //listaSoggetti = listaSoggetti.Where(s => s.tipoSoggettoGiuridico.Equals(tipo));

            //listaSoggetti = listaSoggetti.OrderBy(s => s.ragioneSociale);

            return listaSoggetti.ToList();

        }

    }
}
