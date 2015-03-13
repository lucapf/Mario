using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mediatori.Models
{
    public class CoordinateErogazioniModel
    {
        public List<Anagrafiche.CoordinateErogazione> cordinateErogazioni { get; set; }

        public int? cedenteId { get; set; }
    }
}