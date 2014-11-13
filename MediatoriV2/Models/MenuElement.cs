using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mediatori.Models
{
    public class MenuElement
    {
        public string display { get; set; }
        public string url { get; set; }
        public int livello { get; set; }
        public String role { get; set; }
        public int ordinamento { get; set; }
    }
}