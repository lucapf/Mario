using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mediatori.Models
{
    public class CoordinateErogazioneModel
    {
        public List<Anagrafiche.CoordinateErogazione> coordinateErogazione { get; set; }

        public int? cedenteId { get; set; }
    }
}