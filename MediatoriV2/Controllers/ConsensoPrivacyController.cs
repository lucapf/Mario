using System;
using System.Collections.Generic;
using System.Linq;
using mediatori.Models.Anagrafiche;
using System.Web;
using System.Web.Mvc;
using mediatori.Models;

namespace mediatori.Controllers
{
    public class ConsensoPrivacyController : MyBaseController
    {

        [ChildActionOnly]
        public ActionResult Details(int segnalazioneId)
        {            
            List<ConsensoPrivacy> consensoPrivacy = new List<ConsensoPrivacy>();
            consensoPrivacy = db.ConsensoPrivacy.Include("tipoConsensoPrivacy").Where(c => c.segnalazioneId == segnalazioneId).ToList();

            ConsensoPrivacyModel consensoPrivacyModel = new ConsensoPrivacyModel();
            consensoPrivacyModel.consensiPrivacy = consensoPrivacy;
            return View("_ConsensiPrivacy", consensoPrivacyModel);
        }

       
    }
}