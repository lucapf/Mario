using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mediatori.Models.Configurazione
{
    public class ConfigurazioneModel
    {
        public string nomeEntita { get; set; }

        public List<EntitaModel> listaEntita { get; set; }


        public ConfigurazioneModel(string nome)
        {
            nomeEntita = nome;
        }
    }
}