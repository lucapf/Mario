using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.SimulazioneFinanziaria
{
    public class SimulazioneModel
    {
        public string versione { get; set; }
        public List<MyManagerCSharp.Models.MyItem> agenzie { get; set; }
        public List<MyManagerCSharp.Models.MyItem> prodotti { get; set; }

        public int? agenziaId { get; set; }
        public string prodottoId { get; set; }
        public DateTime? dataDiNascita { get; set; }
        public string sesso { get; set; }
        public int? numeroRate { get; set; }
        public decimal? importoRata { get; set; }
        public DateTime? dataAssunzione { get; set; }
        //public string nazionalita { get; set; }


        public int? segnalazioneId { get; set; }
        public int indice { get; set; }

        public SimulazioneModel()
        {
            agenzie = new List<MyManagerCSharp.Models.MyItem>();
            prodotti = new List<MyManagerCSharp.Models.MyItem>();
        }
    }
}
