using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mediatori.Models.Test
{
    public class SimulazioneModel
    {
        public string url { get; set; }
        public string login { get; set; }
        public string password { get; set; }

        public List<MyManagerCSharp.Models.MyItem> agenzie { get; set; }
    }
}