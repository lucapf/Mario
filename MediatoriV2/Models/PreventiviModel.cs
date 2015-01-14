using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mediatori.Models
{
    public class PreventiviModel
    {
        public int? segnalazioneId { get; set; }
        public List<PreventivoSmall> preventiviSmall { get; set; }
        public List<Preventivo> preventivi { get; set; }
        public PreventivoSmall nuovoPreventivoSmall { get; set; }

        public bool simulazioneEnabled { get; set; }


        public int? praticaId { get; set; }
        public Preventivo preventivoConfermato { get; set; }
    }
}