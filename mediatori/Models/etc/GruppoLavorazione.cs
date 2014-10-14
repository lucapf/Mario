using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mediatori.Models.etc
{
    [Table("gruppo_lavorazione")]
    public class GruppoLavorazione
    {
        [Key]
        public int id { get; set; }
        [Required]
        public String nome { get; set; }
        //indica la lista degli utenti seperati da ';'
        public String utenti { get; set; }
    }


    public class GruppoLavorazioneView
    {
        public GruppoLavorazione gl { get; set; }

        public GruppoLavorazioneView(List<string> users)
        {
            popolaView(users);
        }
        public GruppoLavorazioneView() { }
        public void popolaView(List<string> listAllUsers)
        {
            List<String> listUtenti = new List<String>();
            if (gl == null) gl = new GruppoLavorazione();
            if (gl.utenti != null) listUtenti = gl.utenti.Split(';').ToList<String>();


            liUtenti = new List<SelectListItem>();
            foreach (String utente in listAllUsers)
            {
                liUtenti.Add(new SelectListItem { Text = utente, Value = utente, Selected = listUtenti.Contains(utente) });
            }
        }

        public List<SelectListItem> liUtenti { get; set; }
    }
    public static class GruppoLavorazioneUtils
    {
        public static List<GruppoLavorazioneView> toView(List<GruppoLavorazione> lstGruppoLavorazione, List<String> allUsers)
        {
            List<GruppoLavorazioneView> lsGlw = new List<GruppoLavorazioneView>();
            foreach (GruppoLavorazione gl in lstGruppoLavorazione)
            {
                GruppoLavorazioneView glw = toView(allUsers, gl);
                lsGlw.Add(glw);
            }
            return lsGlw;
        }

        public static GruppoLavorazioneView toView(List<String> allUsers, GruppoLavorazione gl)
        {
            GruppoLavorazioneView glw = new GruppoLavorazioneView();
            glw.gl = gl;
            glw.popolaView(allUsers);
            return glw;
        }
        public static String toTockenizedView(List<String> listUsers)
        {
            String retString = ";";
            foreach (String user in listUsers)
            {
                retString += user + ";";
            }
            return retString;
        }
    }
}