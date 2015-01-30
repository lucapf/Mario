using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mediatori.Models.Segnalazione
{
    public class SegnalazioneCreateModel
    {
        public Anagrafiche.Segnalazione segnalazione { get; set; }

        public Models.Anagrafiche.Contatto contatto { get; set; }
        public Models.Anagrafiche.Impiego impiego { get; set; }
        public Models.Anagrafiche.Riferimento riferimento { get; set; }
        public Models.Nota nota { get; set; }

        //In caso di un contatto 


        public SegnalazioneCreateModel()
        {
            segnalazione = new Anagrafiche.Segnalazione();
            contatto = new Anagrafiche.Contatto();

            impiego = new Anagrafiche.Impiego();
            riferimento = new Anagrafiche.Riferimento();
            nota = new Nota();
        }
    }
}