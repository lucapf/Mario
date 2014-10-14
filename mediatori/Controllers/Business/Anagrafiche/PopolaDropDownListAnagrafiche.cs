using mediatori.Models;
using mediatori.Models.Anagrafiche;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace mediatori.Controllers.Business.Anagrafiche
{
    public class JsonHTMLRetValue
    {
        public String idElemento { get; set; }
        public String html { get; set; }
    }
    public class PopolaDropDownListAnagrafiche
    {
       
        public String popolaDropDownListComuniJSON(String idElement,String  denominazioneProvincia, MainDbContext db)
        {
            JsonHTMLRetValue jsrv = new JsonHTMLRetValue { idElemento = idElement,
                        html = MvcHtmlString.Create(popolaDropDownListComuni(denominazioneProvincia, db)).ToString() };
            return JsonConvert.SerializeObject(jsrv);
            
            
             
        }
        public String popolaDropDownListComuni(String denominazioneProvincia, MainDbContext db)
        {
            StringBuilder response = new StringBuilder() ;
           List<Comune> listaComuni = (from com in db.Comuni 
                                       where com.provincia.denominazione==denominazioneProvincia select com).ToList();
            foreach (Comune c in listaComuni )
            {
                TagBuilder tb = new TagBuilder("option");
                tb.Attributes.Add("name", c.id.ToString());
                tb.SetInnerText(c.denominazione);
                response.AppendLine(tb.ToString(TagRenderMode.Normal));

            }
            return new MvcHtmlString(response.ToString()).ToHtmlString();
        }
        public String popolaDropDownListProvince(MainDbContext  db){
            StringBuilder response = new StringBuilder();
            foreach (Provincia p in (from prov in db.Province select prov).ToList())
            {
                TagBuilder tb = new TagBuilder("option");
                tb.Attributes.Add("name", p.sigla);
                tb.SetInnerText(p.denominazione);
                response.AppendLine(tb.ToString(TagRenderMode.Normal));
            }
            return new MvcHtmlString(response.ToString()).ToHtmlString();
        }

    }
}