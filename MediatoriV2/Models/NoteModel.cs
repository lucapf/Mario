using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mediatori.Models
{
    public class NoteModel
    {
        public ICollection<Nota> note { get; set; }

        public int? segnalazioneId { get; set; }
        public int? soggettoGiuridicoId { get; set; }
    }
}