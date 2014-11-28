using mediatori.Models.etc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mediatori.Models.Anagrafiche
{

    //public class GruppoLavorazioneView
    //{
    //    public GruppoLavorazione gl { get; set; }

    //    public GruppoLavorazioneView(List<string> users)
    //    {
    //        popolaView(users);
    //    }
    //    public GruppoLavorazioneView() { }
    //    public void popolaView(List<string> listAllUsers)
    //    {
    //        List<String> listUtenti = new List<String>();
    //        if (gl == null) gl = new GruppoLavorazione();
    //        if (gl.utenti != null) listUtenti = gl.utenti.Split(';').ToList<String>();


    //        liUtenti = new List<SelectListItem>();
    //        foreach (String utente in listAllUsers)
    //        {
    //            liUtenti.Add(new SelectListItem { Text = utente, Value = utente, Selected = listUtenti.Contains(utente) });
    //        }
    //    }

    //    public List<SelectListItem> liUtenti { get; set; }
    //}
    //public static class GruppoLavorazioneUtils
    //{
    //    public static List<GruppoLavorazioneView> toView(List<GruppoLavorazione> lstGruppoLavorazione, List<String> allUsers)
    //    {
    //        List<GruppoLavorazioneView> lsGlw = new List<GruppoLavorazioneView>();
    //        foreach (GruppoLavorazione gl in lstGruppoLavorazione)
    //        {
    //            GruppoLavorazioneView glw = toView(allUsers, gl);
    //            lsGlw.Add(glw);
    //        }
    //        return lsGlw;
    //    }

    //    public static GruppoLavorazioneView toView(List<String> allUsers, GruppoLavorazione gl)
    //    {
    //        GruppoLavorazioneView glw = new GruppoLavorazioneView();
    //        glw.gl = gl;
    //        glw.popolaView(allUsers);
    //        return glw;
    //    }

       
    


    //public class DecodeStatoCivile
    //{
    //    public static String decode(EnumStatoCivile estatoCivile)
    //    {
    //        return decode(estatoCivile, EnumSesso.MASCHIO);
    //    }
    //    public static String[] listValues()
    //    {
    //        return listValues(EnumSesso.MASCHIO);
    //    }
    //    public static String[] listValues(EnumSesso eSesso)
    //    {
    //        return new String[]{
    //            decode(EnumStatoCivile.CELIBE,eSesso),
    //            decode(EnumStatoCivile.DIVORZIATO,eSesso),
    //            decode(EnumStatoCivile.SPOSATO,eSesso),
    //            decode(EnumStatoCivile.VEDOVO,eSesso)
    //        };
    //    }

    //    public static List<System.Web.Mvc.SelectListItem> getSelectListValues()
    //    {
    //        List<System.Web.Mvc.SelectListItem> items = new List<System.Web.Mvc.SelectListItem>();
    //        items.Add(new System.Web.Mvc.SelectListItem { Text = decode(EnumStatoCivile.CELIBE), Value = "CELIBE" });
    //        items.Add(new System.Web.Mvc.SelectListItem { Text = decode(EnumStatoCivile.DIVORZIATO), Value = "DIVORZIATO" });
    //        items.Add(new System.Web.Mvc.SelectListItem { Text = decode(EnumStatoCivile.SPOSATO), Value = "SPOSATO" });
    //        items.Add(new System.Web.Mvc.SelectListItem { Text = decode(EnumStatoCivile.VEDOVO), Value = "VEDOVO" });

    //        return items;

    //    }


    //    public static String decode(EnumStatoCivile estatoCivile, EnumSesso eSesso)
    //    {
    //        String retValue = "";
    //        switch (estatoCivile)
    //        {
    //            case EnumStatoCivile.CELIBE:
    //                if (EnumSesso.MASCHIO.Equals(eSesso))
    //                {
    //                    retValue = "Celibe";
    //                }
    //                else
    //                {
    //                    retValue = "Nubile";
    //                }
    //                break;
    //            case EnumStatoCivile.SPOSATO:
    //                if (EnumSesso.MASCHIO.Equals(eSesso))
    //                {
    //                    retValue = "Sposato";
    //                }
    //                else
    //                {
    //                    retValue = "Sposata";
    //                }
    //                break;
    //            case EnumStatoCivile.DIVORZIATO:
    //                if (EnumSesso.MASCHIO.Equals(eSesso))
    //                {
    //                    retValue = "Divorziato";
    //                }
    //                else
    //                {
    //                    retValue = "Divorziata";
    //                }
    //                break;
    //            case EnumStatoCivile.VEDOVO:
    //                if (EnumSesso.MASCHIO.Equals(eSesso))
    //                {
    //                    retValue = "Vedovo";
    //                }
    //                else
    //                {
    //                    retValue = "Vedova";
    //                }
    //                break;
    //            default:
    //                break;
    //        }
    //        return retValue;
    //    }
    //}



    //public class StatoView
    //{


    //    public StatoView(MainDbContext db)
    //    {
    //        popolaListeStato(new Stato(), db);
    //    }

    //    public StatoView(Stato stato1, MainDbContext db)
    //    {
    //        popolaListeStato(stato1, db);
    //    }


    //    public void popolaListeStato(Stato stato, MainDbContext db)
    //    {
    //        this.stato = stato;
    //        lstStatoBase = new SelectList(from EnumStatoBase e in EnumStatoBase.GetValues(typeof(EnumStatoBase))
    //                                      select new { Id = e, Name = e.ToString() }, "Id", "Name", stato.statoBase);
    //        lstEntitaAssociata = new SelectList(from EnumEntitaAssociataStato e in
    //                                                EnumEntitaAssociataStato.GetValues(typeof(EnumEntitaAssociataStato))
    //                                            select new { Id = e, Name = e.ToString() }, "Id", "Name", stato.entitaAssociata);
    //        if (stato.gruppo == null)
    //        {
    //            stato.gruppo = new MyUsers.Models.MyGroup();
    //        }

    //        //lstgruppoLavorazione = new SelectList(from GruppoLavorazione gl in db.GruppiLavorazione                                             select new { Id = gl.id, Name = gl.nome },                                                 "Id", "Name", stato.gruppoLavorazione.id);
    //        //    List<String> users = db.Database.SqlQuery<String>("select UserName from dbo.UserProfile").ToList();
    //        //    gruppoLavorazioneView = GruppoLavorazioneUtils.toView(users, stato.gruppoLavorazione);

    //    }
    //    public Stato stato { get; set; }
    //    public SelectList lstStatoBase { get; set; }
    //    public SelectList lstEntitaAssociata { get; set; }
    //    public SelectList lstgruppoLavorazione { get; set; }
    //}

    public class DecodeSesso
    {
        public static List<SelectListItem> getSelectListItems()
        {
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            selectListItems.Add(new SelectListItem { Text = EnumSesso.MASCHIO.ToString() });
            selectListItems.Add(new SelectListItem { Text = EnumSesso.FEMMINA.ToString(), Value = EnumSesso.FEMMINA.ToString() });
            return selectListItems;

        }
    }



    public class DecodeStatoCivile
    {
        public static String decode(EnumStatoCivile estatoCivile)
        {
            return DecodeStatoCivile.decode(estatoCivile, EnumSesso.MASCHIO);
        }
        public static String[] listValues()
        {
            return DecodeStatoCivile.listValues(EnumSesso.MASCHIO);
        }
        public static String[] listValues(EnumSesso eSesso)
        {
            return new String[]{
                DecodeStatoCivile.decode(EnumStatoCivile.CELIBE,eSesso),
                decode(EnumStatoCivile.DIVORZIATO,eSesso),
                decode(EnumStatoCivile.SPOSATO,eSesso),
                decode(EnumStatoCivile.VEDOVO,eSesso)
            };
        }

        public static List< System.Web.Mvc.SelectListItem>  getSelectListValues()
        {
            List<System.Web.Mvc.SelectListItem> items = new List<System.Web.Mvc.SelectListItem>();
            items.Add(new System.Web.Mvc.SelectListItem { Text = decode(EnumStatoCivile.CELIBE), Value = "CELIBE" });
            items.Add(new System.Web.Mvc.SelectListItem { Text = decode(EnumStatoCivile.DIVORZIATO), Value = "DIVORZIATO" });
            items.Add(new System.Web.Mvc.SelectListItem { Text = decode(EnumStatoCivile.SPOSATO), Value = "SPOSATO" });
            items.Add(new System.Web.Mvc.SelectListItem { Text = decode(EnumStatoCivile.VEDOVO), Value = "VEDOVO" });

            return items;

        }


        public static String decode(EnumStatoCivile estatoCivile, EnumSesso eSesso)
        {
            String retValue = "";
            switch (estatoCivile)
            {
                case EnumStatoCivile.CELIBE:
                    if (EnumSesso.MASCHIO.Equals(eSesso))
                    {
                        retValue = "Celibe";
                    }
                    else
                    {
                        retValue = "Nubile";
                    }
                    break;
                case EnumStatoCivile.SPOSATO:
                    if (EnumSesso.MASCHIO.Equals(eSesso))
                    {
                        retValue = "Sposato";
                    }
                    else
                    {
                        retValue = "Sposata";
                    }
                    break;
                case EnumStatoCivile.DIVORZIATO:
                    if (EnumSesso.MASCHIO.Equals(eSesso))
                    {
                        retValue = "Divorziato";
                    }
                    else
                    {
                        retValue = "Divorziata";
                    }
                    break;
                case EnumStatoCivile.VEDOVO:
                    if (EnumSesso.MASCHIO.Equals(eSesso))
                    {
                        retValue = "Vedovo";
                    }
                    else
                    {
                        retValue = "Vedova";
                    }
                    break;
                default:
                    break;
            }
            return retValue;
        }
    }

}