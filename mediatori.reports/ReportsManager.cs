using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mediatori.Reports
{
    public class ReportsManager : MyManagerCSharp.RGraph.RGraphManager
    {
        public ReportsManager(string connectionName)
            : base(connectionName)
        {

        }


        public ReportsManager(System.Data.Common.DbConnection connection)
            : base(connection)
        {

        }


        public void getTest(MyManagerCSharp.RGraph.Models.RGraphModel report)
        {

            //MyManagerCSharp.RGraph.Models.RGraphModel report = new MyManagerCSharp.RGraph.Models.RGraphModel(); 

            _strSQL = "select top 10  login_success as valore , my_login as label " +
                 " from UTENTE " +
                 " order by login_success desc";

            report.Data = _fillDataTable(_strSQL);
            report.Label = LabelType.Label;
            report.Tipo = ReportType.HBar;
            report.Titolo = "Test";



            //ShowLabels = true;


            getChart(report, false, false);
            Debug.WriteLine("html" + report.Html);
            //  return report;
        }


        public void getSegnalazioniPratiche(MyManagerCSharp.RGraph.Models.RGraphModel report, MyManagerCSharp.ManagerDB.Days days, mediatori.Models.etc.EnumEntitaAssociataStato entita)
        {
            _strSQL = "select " +
                 getGroupByByDate("dataInserimento", days) + " as label " +
                ",COUNT(*)  as Totale " +
                ",sum(case when t2.statoBase =  1 then 1 else 0 end) as 'Attivo'" +
                ",sum(case when t2.statoBase =  2 then 1 else 0 end) as 'Annullato'" +
                ",sum(case when t2.statoBase =  3 then 1 else 0 end) as 'Respinto'" +
                ",sum(case when t2.statoBase =  4 then 1 else 0 end) as 'Chiuso'" +
                " from segnalazione as t1 " +
                " join stato as t2 on t1.statoId = t2.id ";

            if (entita == mediatori.Models.etc.EnumEntitaAssociataStato.PRATICA)
            {
                _strSQL += " where Discriminator = 'Pratica' ";
            }
            else
            {
                _strSQL += " where Discriminator = 'Segnalazione' ";
            }
                           

            _strSQL += getWhereConditionByDate("dataInserimento", days);

            _strSQL += " group by t2.statoBase, " + getGroupByByDate("dataInserimento", days);
            _strSQL += " order by " + getGroupByByDate("dataInserimento", days);



            System.Data.DataTable dt = _fillDataTable(_strSQL);


            MyManagerCSharp.Models.MySeries serieAttivo = new MyManagerCSharp.Models.MySeries(mediatori.Models.EnumStatoBase.ATTIVO.ToString());
            MyManagerCSharp.Models.MySeries serieAnnullato = new MyManagerCSharp.Models.MySeries(mediatori.Models.EnumStatoBase.ANNULLATO.ToString());
            MyManagerCSharp.Models.MySeries serieRespinto = new MyManagerCSharp.Models.MySeries(mediatori.Models.EnumStatoBase.RESPINTO.ToString());
            MyManagerCSharp.Models.MySeries serieChiuso = new MyManagerCSharp.Models.MySeries(mediatori.Models.EnumStatoBase.CHIUSO.ToString());


            string label;
            foreach (System.Data.DataRow row in dt.Rows)
            {
                Debug.WriteLine(String.Format("Label: {0}", row["label"]));

                //   temp = String.Format("{0}", System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(row["nome"].ToString()));

                label = row["label"].ToString();

                serieAttivo.Values.Add(new MyManagerCSharp.Models.MyItemV2(label, int.Parse(row["Attivo"].ToString())));
                serieAnnullato.Values.Add(new MyManagerCSharp.Models.MyItemV2(label, int.Parse(row["Annullato"].ToString())));
                serieRespinto.Values.Add(new MyManagerCSharp.Models.MyItemV2(label, int.Parse(row["Respinto"].ToString())));
                serieChiuso.Values.Add(new MyManagerCSharp.Models.MyItemV2(label, int.Parse(row["Chiuso"].ToString())));
            }

            report.Series.Add(serieAttivo);
            report.Series.Add(serieAnnullato);
            report.Series.Add(serieRespinto);
            report.Series.Add(serieChiuso);


            report.Label = LabelType.LabelAndValore;
            report.Tipo = ReportType.HBar;
            report.Titolo = "";
            report.Colors = PaletteType.Palette01;


            report.ShowLegend = true;

            report.Settings.Add(String.Format("{0}.Set('grouping', 'stacked');", report.Id));
            report.Settings.Add(String.Format("{0}.Set('gutter.left', 70);", report.Id));
            report.Settings.Add(String.Format("{0}.Set('background.grid.dashed', true);", report.Id));

            report.Settings.Add(String.Format("{0}.Set('ylabels', false);", report.Id));
            report.Settings.Add(String.Format("{0}.Set('noaxes', true);", report.Id));

            getChartV2(report);
            Debug.WriteLine("html" + report.Html);
            //  return report;
        }

    }
}
