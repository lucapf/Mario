using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using mediatori.Models;
using mediatori.Models.Anagrafiche;
using System.Linq;
using System.Web.Mvc;
using System.Web;
using mediatori.Controllers.Business.Anagrafiche;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace mediatori.UnitTest
{
    [TestClass]
    public class PopolamentoDropDownListTest
    {
        public static String URLTEST = "http://TestDatabaseConnection.prova.it";
        [TestMethod]
        public void PopolaComboComuni()
        {
            MainDbContext db = new MainDbContext(URLTEST);
            Provincia roma = (from p in db.Province where p.sigla=="RM" select p).First();
            List<Comune> comuni = (from com in db.Comuni where com.codiceProvincia == roma.id select com).ToList();
            string combo = new PopolaDropDownListAnagrafiche().popolaDropDownListComuni(roma.denominazione,db);
            Assert.IsTrue(combo.Contains(comuni.First().denominazione),"la combo comuni non è stata valorizzata correttamente");
        }

        [TestMethod]
        public void PopolaComboProvince()
        {
            MainDbContext db = new MainDbContext(URLTEST);
            string combo = new PopolaDropDownListAnagrafiche().popolaDropDownListProvince(db);
            Assert.IsTrue(combo.Contains("Roma"), "combo non valorizzata correttamente");
            Assert.IsTrue(combo.Contains("Rieti"), "combo non valorizzata correttamente");
        }
        [TestMethod]
        public void PopolaComboComuniJSON()
        {
            MainDbContext db = new MainDbContext(URLTEST);
            String idElemento = Guid.NewGuid().ToString();
             Provincia roma = (from p in db.Province where p.sigla=="RM" select p).First();
            List<Comune> comuni = (from com in db.Comuni where com.codiceProvincia == roma.id select com).ToList();
            String jsonStringCombo = new PopolaDropDownListAnagrafiche().popolaDropDownListComuniJSON(idElemento, roma.denominazione, db);
            JObject jsonObject = (JObject)JsonConvert.DeserializeObject(jsonStringCombo);
          //  Assert.AreEqual(jsonObject.GetValue("idElemento"), idElemento, "elemento non serializzato correttamente");
           // JToken t = jsonObject.GetValue("html");
            //Assert.IsTrue(t.ToString().Contains(comuni.First().denominazione), "la combo comuni non è stata valorizzata correttamente");
        }
    }
}
