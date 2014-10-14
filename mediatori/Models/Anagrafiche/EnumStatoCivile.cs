using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages.Html;

namespace mediatori.Models.Anagrafiche
{
    public enum EnumStatoCivile
    {
        CELIBE, SPOSATO, DIVORZIATO, VEDOVO

    }
   
   
    public class DecodeStatoCivile
    {
        public static String decode(EnumStatoCivile estatoCivile)
        {
            return decode(estatoCivile, EnumSesso.MASCHIO);
        }
        public static String[] listValues()
        {
            return listValues(EnumSesso.MASCHIO);
        }
        public static String[] listValues(EnumSesso eSesso)
        {
            return new String[]{
                decode(EnumStatoCivile.CELIBE,eSesso),
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