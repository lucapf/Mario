using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mediatori.Models
{
    public class PopupConfirmDeleteModel
    {
        public string actionName { get; set; }
        public string controllerName { get; set; }

        public List<HiddenFiled> hiddenFields { get; set; }

        public PopupConfirmDeleteModel()
        {
            hiddenFields = new List<HiddenFiled>();
        }
    }


    public class HiddenFiled
    {
        public string id { get; set; }
        public string name { get; set; }
        public string value { get; set; }

        public override string ToString()
        {
            string temp;
            temp = String.Format ("<input type=\"hidden\" id=\"{0}\" name=\"{1}\" value=\"{2}\" />", id, name,value);
            return temp;
        }
    }
}