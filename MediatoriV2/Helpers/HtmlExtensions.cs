using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using mediatori.Models;
using mediatori.Models.Anagrafiche;
using System.Web.Mvc;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.IO;

namespace System.Web.Mvc.Html
{
    public static class CheckBoxListExtension
    {
        public static MvcHtmlString CheckBoxList(this HtmlHelper htmlHelper, List<SelectListItem> listOfValues, String id, bool listInTable = false)
        {
            StringBuilder sb = new StringBuilder();

            if (listOfValues != null)
            {
                foreach (SelectListItem i in listOfValues)
                {

                    String label = HttpUtility.HtmlEncode(i.Text);
                    TagBuilder tb = new TagBuilder("input");
                    tb.Attributes.Add("type", "checkbox");
                    tb.Attributes.Add("value", i.Value);
                    tb.Attributes.Add("name", id);
                    if (i.Selected) tb.Attributes.Add("checked", "true");
                    String checkbox = tb.ToString(TagRenderMode.SelfClosing);
                    if (listInTable)
                    {
                        sb.AppendLine("<tr><td>" + label + "</td><td>" + checkbox + "<td></tr>");
                    }
                    else
                    {
                        sb.AppendLine(label + checkbox);
                    }
                    //sb.AppendFormat("{0}{1}", checkbox  , label );

                }
            }

            return MvcHtmlString.Create(sb.ToString());
        }

        public static MvcHtmlString RadioButtonList<TModel, TProperty>
                (this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> ex, String name, IEnumerable<SelectListItem> listOfValues)
        {
            StringBuilder sb = new StringBuilder();
            var metaData = ModelMetadata.FromLambdaExpression(ex, htmlHelper.ViewData);
            if (listOfValues != null)
            {

                foreach (var item in listOfValues)
                {

                    String label = HttpUtility.HtmlEncode(item.Text);
                    IDictionary<String, Object> valori = new Dictionary<string, object>();
                    valori.Add("class", "CustomCheckBox");
                    item.Selected = ((metaData.Model == null ? String.Empty : metaData.Model as string) == item.Value);
                    String checkbox = htmlHelper.RadioButton(name, item.Value, item.Selected, valori).ToHtmlString();
                    sb.AppendLine(label + checkbox);
                }
            }

            return MvcHtmlString.Create(sb.ToString());
        }
        public static MvcHtmlString indirizzoReadOnlyString(this HtmlHelper htmlHelper, Indirizzo indirizzo)
        {
            TagBuilder tag = new TagBuilder("div");
            tag.Attributes.Add("id", "ADDR_" + indirizzo.id.ToString());
            tag.InnerHtml = indirizzo.toponimo + " "
                            + indirizzo.recapito + " "
                            + indirizzo.numeroCivico + " "
                            + indirizzo.interno + " "
                            + indirizzo.cap + " "
                            + indirizzo.provincia;
            return MvcHtmlString.Create(tag.ToString(TagRenderMode.SelfClosing));
        }

        public static MvcHtmlString valuta(this HtmlHelper htmlHelper, Type tipo, String name, float importo)
        {
            StringBuilder sb = new StringBuilder();
            DisplayAttribute da = (DisplayAttribute)tipo.GetProperty(name)
                                            .GetCustomAttributes(typeof(DisplayAttribute), true)
                                            .SingleOrDefault();
            RangeAttribute range = (RangeAttribute)tipo.GetProperty(name)
                                            .GetCustomAttributes(typeof(RangeAttribute), true)
                                            .SingleOrDefault();
            RequiredAttribute ra = (RequiredAttribute)tipo.GetProperty(name).
                                    GetCustomAttributes(typeof(RequiredAttribute), true).SingleOrDefault();
            String dataValidationMessage = String.Empty;
            TagBuilder inputTag = new TagBuilder("input");
            inputTag.Attributes.Add("name", name);
            inputTag.Attributes.Add("id", name);
            if (importo > 0)
            {
                inputTag.Attributes.Add("value", importo.ToString());
            }
            inputTag.Attributes.Add("data-val-number", "Il campo " + da.Name + "deve essere un numero.");
            // String DataValidationRegex = " ";
            if (ra != null)
            {
                inputTag.Attributes.Add("data-val", "true");
                if (ra.ErrorMessage != null)
                {
                    inputTag.Attributes.Add("data-val-required", ra.ErrorMessage);
                }
                else
                {
                    inputTag.Attributes.Add("data-val-required", " Il campo " + da.Name + " è obbligatorio.");
                }

            }
            if (range != null)
            {
                inputTag.Attributes.Add("data-val-range", "Il campo " + da.Name + " deve essere compreso tra " + range.Minimum + " e " + range.Maximum + ".");
                inputTag.Attributes.Add("data-val-range-max", range.Maximum.ToString());
                inputTag.Attributes.Add("dava-val-range-min", range.Minimum.ToString());
            }
            sb.AppendLine("<script> $(function() {  " +
                            " $( \"#" + name + "\" ).spinner({ min: 0,max: 25000,step: 100,start: 1000,culture:\"de-DE\",numberFormat: \"C\"});" +
                            " $( \"#" + name + "\" ).spinner( \"option\", \"culture\", \"de-DE\" );});</script>");
            sb.AppendLine(inputTag.ToString(TagRenderMode.SelfClosing));
            return MvcHtmlString.Create(sb.ToString());
        }
    }

    public class CustomDateTimeModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var displayFormat = bindingContext.ModelMetadata.DisplayFormatString;
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (!string.IsNullOrEmpty(displayFormat) && value != null)
            {
                DateTime date;
                displayFormat = displayFormat.Replace("{0:", string.Empty).Replace("}", string.Empty);
                // use the format specified in the DisplayFormat attribute to parse the date
                if (DateTime.TryParseExact(value.AttemptedValue, displayFormat, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out date))
                {
                    return date;
                }
                else
                {
                    bindingContext.ModelState.AddModelError(
                        bindingContext.ModelName,
                        string.Format("{0} is an invalid date format", value.AttemptedValue)
                    );
                }
            }

            return base.BindModel(controllerContext, bindingContext);
        }
    }

}
//oggetto che contiene i riferimenti per costruire, in base al modello il codice per la gestione
namespace mediatori.helper
{
    public class FireAntDetailEventDetection
    {
        String idShortDetail = String.Empty;
        String idFullDetail = String.Empty;
        String idBtnShowDetail = String.Empty;
        String idBtnHideDetail = String.Empty;
        String idBtnModifica = String.Empty;
        String idBtnAnnulla = String.Empty;
        String idBtnSalva = String.Empty;
        String idDivMessage = String.Empty;
        String id = "0";

        public FireAntDetailEventDetection(String prefisso, int id) :
            this(prefisso, id + "")
        {


        }
        public FireAntDetailEventDetection(String prefisso, string id)
        {
            idShortDetail = prefisso + "Short" + id;
            idFullDetail = prefisso + "Full" + id;
            idBtnShowDetail = prefisso + "BtnShowDetail" + id;
            idBtnHideDetail = prefisso + "BtnHideDetail" + id;
            idBtnModifica = prefisso + "btnEdit" + id;
            idBtnAnnulla = prefisso + "btnAnnulla" + id;
            idBtnSalva = prefisso + "btnSalva" + id;
            idDivMessage = prefisso + "divMesg" + id;
            this.id = id;
        }
        public String buildMsgDiv()
        {
            TagBuilder tb = new TagBuilder("div");
            tb.Attributes.Add("id", idDivMessage.ToString());
            tb.Attributes.Add("class", "boxMessages");


            return tb.ToString(TagRenderMode.StartTag) + tb.ToString(TagRenderMode.EndTag);
        }
        public String buildShortDetailDiv(String innerHtml = "")
        {
            TagBuilder tb = new TagBuilder("div");
            tb.Attributes.Add("id", idShortDetail.ToString());
            tb.Attributes.Add("class", "boxShortDetail");
            tb.Attributes.Add("title", "fai click per visualizzare il dettaglio");

            return tb.ToString(TagRenderMode.StartTag) + innerHtml + tb.ToString(TagRenderMode.EndTag);
        }
        public String buildFullDetailDiv(String innerHtml)
        {
            TagBuilder tb = new TagBuilder("div");
            // id="@idBtnHideDetail" class="boxFullDetailLink" title="fai click per chiudere il dettaglio"
            tb.Attributes.Add("id", idBtnHideDetail);
            tb.Attributes.Add("class", "boxFullDetailLink");
            tb.Attributes.Add("title", "fai click per chiudere il dettaglio");
            return tb.ToString(TagRenderMode.StartTag) + innerHtml + tb.ToString(TagRenderMode.EndTag);
        }
        public String getScriptHandler()
        {
            String args = String.Format("\"{0}\", \"{1}\", \"{2}\", \"{3}\", \"{4}\",\"{5}\"",
                idFullDetail, idShortDetail, idBtnModifica, idBtnAnnulla, idBtnSalva, idBtnHideDetail);
            return "<script> $(function () { " +
         "$(\"#" + idFullDetail + "\").hide(); " +
         "$(\"#" + idShortDetail + "\").click(function () { FireAntDetailEventDetection_MostraDettaglio(" + args + "); }); " +
         "$(\"#" + idBtnHideDetail + "\").click(function () { FireAntDetailEventDetection_NascondiDettaglio(" + args + "); });" +
         "}); </script>";
        }
        public String getIdFullDetail()
        {
            return idFullDetail.ToString();
        }
        public String getIdCmdHideShortDetail()
        {
            return idBtnHideDetail.ToString();
        }
        public String getIdCmdEdit()
        {
            return idBtnModifica.ToString();
        }

        public String getHeader(String htmlShortDispaly)
        {
            return getScriptHandler() + buildShortDetailDiv(htmlShortDispaly) + buildMsgDiv();
        }

    }
    public class FireAntEditHelper
    {
        private String codiceDiv = String.Empty;
        private String codiceMessage = String.Empty;
        private String buttonModifica = String.Empty;
        private String buttonAnnullaModifica = String.Empty;
        private String buttonSalvaModifica = String.Empty;
        private String buttonNascondiDettaglio = String.Empty;
        private String formDati = String.Empty;
        private String modelId = String.Empty;
        private String url = String.Empty;
        private String codiceDivFullDetail = String.Empty;
        private String codiceDivShortDetail = String.Empty;
        private FireAntEditHelper() { }
        public FireAntEditHelper(String prefisso, String url, String modelId)
        {
            this.codiceDiv = String.Format("{0}div{1}", prefisso, modelId);
            this.codiceDivFullDetail = String.Format("{0}Full{1}", prefisso, modelId);
            this.codiceDivShortDetail = String.Format("{0}Short{1}", prefisso, modelId);
            this.codiceMessage = String.Format("{0}divMesg{1}", prefisso, modelId);
            this.buttonModifica = String.Format("{0}btnEdit{1}", prefisso, modelId);
            this.buttonAnnullaModifica = String.Format("{0}btnAnnulla{1}", prefisso, modelId);
            this.buttonSalvaModifica = String.Format("{0}btnSalva{1}", prefisso, modelId);
            this.buttonNascondiDettaglio = String.Format("{0}BtnHideDetail{1}", prefisso, modelId);
            this.formDati = String.Format("{0}Form{1}", prefisso, modelId);
            this.modelId = modelId;
            this.url = url;
        }
        public String getIdDiv() { return codiceDiv; }
        public String getIdDivMessage() { return codiceMessage; }
        public String getIdForm() { return formDati; }
        public String refreshValidation() { return String.Format("<script>refreshValidation('{0}')</script>", getIdForm()); }
       

        //public String getButtonModifica(String htmlAttributes = "", String javascriptCustomFunction = ""){
        //    return getButtonModifica(htmlAttributes, javascriptCustomFunction);
        //}

        public String getButtonModifica(String htmlAttributes = "", String javascriptCustomFunction = "")
        {
            htmlAttributes += "class='h3Link' title='Modifica'";
            return buildBottoneGenerico(buttonModifica, "[ M ]", "FireAntEditHelper_ModificaDati", htmlAttributes, javascriptCustomFunction);
        }

        public String getButtonModificaV2(String htmlAttributes = "", String javascriptCustomFunction = "")
        {
            htmlAttributes += "class='h3Link ui-btn-right' title='Modifica'";
            return buildBottoneGenerico(buttonModifica, "[ M ]", "FireAntEditHelper_ModificaDati", htmlAttributes, javascriptCustomFunction);
        }
       
        public String getButtonAnnullaModifica(String value = "[ A ] ", String htmlAttributes = "", String javascriptCustomFunction = "")
        {
            htmlAttributes += "class='h3Link'  title='Annulla' style='visibility:hidden;'";
            return buildBottoneGenerico(buttonAnnullaModifica, value, "FireAntEditHelper_AnnullaModificaDati", htmlAttributes, javascriptCustomFunction);
        }

        public String getButtonCollapse(String value = "[ C ] ", String htmlAttributes = "", String javascriptCustomFunction = "")
        {
            htmlAttributes += "class='buttonCollapse' title='forma breve'";
            return buildBottoneGenerico(buttonNascondiDettaglio, value, "FireAntDetailEventDetection_NascondiDettaglio", htmlAttributes, javascriptCustomFunction);
        }

        public String getButtonSalvaModifica(String value = "[ S ]", String htmlAttributes = "")
        {
            htmlAttributes += "class='buttonSalvaModifica' title='salva modifiche'";
            return buildBottoneGenerico(buttonSalvaModifica, value, "FireAntEditHelper_SubmitInnerForm", htmlAttributes, String.Empty);

        }

        public String getCrudButtons()
        {
            return getButtonModifica() + getButtonSalvaModifica()
                   + getButtonAnnullaModifica() + getButtonAnnullaInserimento()
                   + getButtonCollapse() + new TagBuilder("br").ToString(TagRenderMode.SelfClosing);
        }

        public String getSimpleCrudButtons()
        {
            return getButtonModifica() + getButtonSalvaModifica()
                + getButtonAnnullaModifica() + "<script>$(\"#" + buttonModifica + "\").show();</script>";

        }
        public String getStartForm(String urlAction)
        {
            TagBuilder tb = new TagBuilder("form");
            tb.Attributes.Add("id", getIdForm());
            tb.Attributes.Add("method", "post");
            tb.Attributes.Add("action", urlAction);
            return tb.ToString(TagRenderMode.StartTag);

        }
        public String getEndForm()
        {
            return new TagBuilder("form").ToString(TagRenderMode.EndTag);
        }

        public String getButtonAnnullaInserimento(String value = "Annulla", String htmlAttributes = "")
        {
            TagBuilder submitForm = new TagBuilder("input");
            //Rutigliano
            submitForm.Attributes.Add("data-role", "none");
            submitForm.Attributes.Add("type", "button");
            submitForm.Attributes.Add("value", value);
            submitForm.Attributes.Add("style", "display:none");
            submitForm.Attributes.Add("id", buttonAnnullaModifica);
            //(idButtonModifica, idButtonAnnullaSalvataggio, idButtonSalva
            string scriptAppender = "<script>$(\"#" + buttonAnnullaModifica + "\").click(function(){ " +
                " FireAntEditHelper_AnnullaInserimento(\"" + buttonModifica + "\",\"" + buttonAnnullaModifica
                                                           + "\",\"" + buttonSalvaModifica + "\",\"" + buttonNascondiDettaglio + "\",\"" + getIdDiv() + "\")});</script>";
            return submitForm.ToString(TagRenderMode.SelfClosing) + scriptAppender;

        }

     

        private String buildBottoneGenerico(String idBottone, String value, String onclickFunction, String htmlAttributes, String javaScriptCustomFunction)
        {

            String onclickArgs = String.Format("\"{0}\", \"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\",\"{8}\",\"{9}\",\"{10}\",\"{11}\"",
                         modelId, url, codiceMessage, codiceDiv, codiceDivFullDetail, codiceDivShortDetail, buttonModifica,
                          buttonAnnullaModifica, buttonSalvaModifica, buttonNascondiDettaglio, formDati, javaScriptCustomFunction);

            //Dopo il clic del bottone viene richiamata la javaScriptCustomFunction


            string onclickEvent = "<script type=\"text/javascript\">   $(document).on(\"pageinit\", function () { ";

            onclickEvent += " $(\"#" + idBottone + "\").click(function(){ " + onclickFunction + "( " + onclickArgs + " )});";
            
            onclickEvent += "});";

            onclickEvent += "</script>";

          //  String htmlTag = String.Format("<div id='{0}' {1}>{3}</div>{2}", idBottone, htmlAttributes, onclickEvent, value);
            String htmlTag = String.Format("<strong id='{0}' {1}>{3}</strong> {2}", idBottone, htmlAttributes, onclickEvent, value);
            return htmlTag;
        }


    }
    public class FireAntHtmlHelper
    {
        public static HtmlTableCell buildCell(String value)
        {
            HtmlTableCell tableCell = new HtmlTableCell();
            tableCell.InnerText = value;
            return tableCell;
        }
        public static HtmlTableCell buildTH(String value)
        {
            HtmlTableCell tableCell = new HtmlTableCell("th");
            tableCell.InnerText = value;
            return tableCell;

        }
        public static HtmlTableCell buildCell(TagBuilder innerTag)
        {
            HtmlTableCell tc = new HtmlTableCell();
            tc.InnerHtml = innerTag.ToString();
            return tc;

        }
        internal static string renderControl(Control control)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            HtmlTextWriter writer = new HtmlTextWriter(sw);
            control.RenderControl(writer);
            return sb.ToString();
        }
    }

}