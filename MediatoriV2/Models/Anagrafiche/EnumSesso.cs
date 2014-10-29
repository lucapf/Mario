using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mediatori.Models.Anagrafiche
{
    public enum EnumSesso
    {
        MASCHIO,FEMMINA
    }
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
}