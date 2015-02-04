using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mediatori.Models
{
    public class MenuElement
    {
        public string display { get; set; }
        public string action { get; set; }
        public string controller { get; set; }
        public int livello { get; set; }
        public String role { get; set; }
        public int ordinamento { get; set; }
    }
}