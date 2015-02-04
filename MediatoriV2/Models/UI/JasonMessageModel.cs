using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mediatori.Models
{
    public class JsonMessageModel
    {
        public enum Esito
        {
            Failed = 0,
            Succes = 1
        }

        public string referenceId;
        public string messaggio;
        public Esito esito;

    }

}