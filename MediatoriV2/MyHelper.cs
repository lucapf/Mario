using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc.Html;
using System.Web.Mvc;
using System.Text;

using System.Diagnostics;
using System.Linq.Expressions;

public static class MyHelper
{
   public static string ToTitleCase(string value)
    {
        return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value.ToLower());
    }

    public static string getDbEntityValidationException(System.Data.Entity.Validation.DbEntityValidationException ex)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        foreach (var failure in ex.EntityValidationErrors)
        {
            sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
            foreach (var error in failure.ValidationErrors)
            {
                sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                sb.AppendLine();
            }
        }
        Debug.WriteLine("EntityValidationErrors:" + sb.ToString());

        return sb.ToString();
    }

    public static string decodeSiNo(int? value)
    {
        if (value == null || value == -1)
        {
            return "N/A";
        }

        if (value == 0)
        {
            return "No";
        }

        if (value == 1)
        {
            return "Si";
        }
        return "ERRORE: decodeSiNo value = " + value;
    }

    public static string decodeSiNo(bool value)
    {
        if (value)
        {
            return "Si";
        }

        if (value == false)
        {
            return "No";
        }
        return "ERRORE: decodeSiNo value = " + value;
    }

    public static string decodeAttiva(bool value)
    {
        if (value)
        {
            return "Attiva";
        }

        if (value == false)
        {
            return "NON attiva";
        }
        return "ERRORE: decodeSiNo value = " + value;
    }

    public static string decodeAttivo(bool value)
    {
        if (value)
        {
            return "Attivo";
        }

        if (value == false)
        {
            return "NON attivo";
        }
        return "ERRORE: decodeSiNo value = " + value;
    }

    public static HtmlString decodeAttivaImage(string contesto, bool value)
    {

        if (contesto == "/")
        {
            contesto = "";
        }

        string temp;
        if (value)
        {
            temp = "green2_16x16.png";
        }
        else
        {
            temp = "red2_16x16.png";
        }
        return new HtmlString(String.Format("<img src='{1}/Content/Images/Shared/{0}' />", temp, contesto));
    }

    #region "Decode NULL"



    public static HtmlString decodeNull(DateTime? valore)
    {

        if (valore == null || valore == DateTime.MinValue)
        {
            return new HtmlString("N/A");
        }


        return new HtmlString(valore.ToString());
    }

    public static HtmlString decodeNull(bool? valore)
    {

        if (valore == null)
        {
            return new HtmlString("N/A");
        }


        return new HtmlString(valore.ToString());
    }


    public static HtmlString decodeNull(string valore)
    {

        if (String.IsNullOrEmpty(valore))
        {
            return new HtmlString("N/A");
        }


        return new HtmlString(valore.Replace(Environment.NewLine, "<br />"));
    }

    public static HtmlString decodeNull(decimal? valore)
    {

        if (valore == null)
        {
            return new HtmlString("N/A");
        }


        return new HtmlString(valore.Value.ToString());
    }


    public static HtmlString decodeNull(int? valore)
    {

        if (valore == null)
        {
            return new HtmlString("N/A");
        }


        return new HtmlString(valore.Value.ToString());
    }


    public static HtmlString decodeEnum(string value)
    {
        if (String.IsNullOrEmpty(value) || (value == "0") || (value == "Undefined"))
        {
            return new HtmlString("N/A");
        }


        return new HtmlString(value.Replace("_", " "));
    }


    public static HtmlString decodeNull(double? valore)
    {

        if (valore == null)
        {
            return new HtmlString("N/A");
        }


        return new HtmlString(valore.Value.ToString());
    }

    #endregion


    public static string getColorByRating(decimal value)
    {

        if ((double)value <= 3.3)
        {
            return "black";

        }

        if ((double)value <= 6.6)
        {
            return "orange";

        }

        return "red";
    }

    public static HtmlString getImageRating(string contesto, double? value)
    {
        if (value == null || value == -1)
        {
            return new HtmlString("");
        }

        if (contesto == "/")
        {
            contesto = "";
        }


        string temp;
        temp = String.Format("<img style=\"float:right;\" height=\"20px\" src='{1}/Content/Images/pdf/{0}.jpg' />", Math.Round((double)value), contesto);

        return new HtmlString(temp);
    }


    //public static String toTockenizedw(List<String> values)
    //{
    //    if (values == null)
    //    {
    //        return "";
    //    }

    //    String retString = ";";
    //    foreach (String v in values)
    //    {
    //        retString += v + ";";
    //    }
    //    return retString;
    //}



    //public static HtmlString CheckboxListForEnum<T>(this HtmlHelper html, string name, T modelItems) where T : struct
    //{
    //    StringBuilder sb = new StringBuilder();

    //    foreach (T item in Enum.GetValues(typeof(T)).Cast<T>())
    //    {
    //        TagBuilder builder = new TagBuilder("input");
    //        long targetValue = Convert.ToInt64(item);
    //        long flagValue = Convert.ToInt64(modelItems);

    //        if ((targetValue & flagValue) == targetValue)
    //            builder.MergeAttribute("checked", "checked");

    //        builder.MergeAttribute("type", "checkbox");
    //        builder.MergeAttribute("value", item.ToString());
    //        builder.MergeAttribute("name", name);
    //        builder.InnerHtml = item.ToString();

    //        sb.Append(builder.ToString(TagRenderMode.Normal));
    //    }

    //    return new HtmlString(sb.ToString());
    //}



    //public static SelectList ToSelectList(this Enum enumObj)
    //{

    //    foreach (var value in Enum.GetValues(typeof(Enum)))
    //    {
    //        Debug.WriteLine(String.Format("{0}", value));
    //    }



    //    return new SelectList(null, "Id", "Name", enumObj);
    //}





    //public static IEnumerable<SelectListItem > ToSelectList<TEnum>(this TEnum listOfValues,  string currentValue)
    //{

    //    Debug.WriteLine(String.Format("currentValue: {0}", currentValue));

    //    foreach (var value in Enum.GetValues(typeof(TEnum)))
    //    {
    //        Debug.WriteLine(String.Format("{0}", value));
    //    }





    ////    var result =
    ////from e in Enum.GetValues(typeof(TEnum)).Cast<TEnum>()
    ////select new
    ////{
    ////    Id = (int)Enum.Parse(typeof(TEnum), e.ToString()),
    ////    Name = e.ToString()
    ////};


    //    List <SelectListItem  > risultato = new List<SelectListItem> ();

    //    risultato.Add (new SelectListItem (){ Text = "---", Value = "" });

    //  //  risultato.AddRange(from e in Enum.GetValues(typeof(TEnum)).Cast<TEnum>() select new SelectListItem { Value = ((int)Enum.Parse(typeof(TEnum), e.ToString()) ).ToString() , Text = e.ToString() });

    //    bool selected;
    //    foreach (var e in Enum.GetValues(typeof(TEnum)))
    //    {
    //        Debug.WriteLine(String.Format("{0}", e));
    //        selected = false;
    //        if (e.ToString().Equals(currentValue))
    //        {
    //            selected = true;
    //        }


    //        risultato.Add(new SelectListItem() { Value = ((int)Enum.Parse(typeof(TEnum), e.ToString())).ToString(), Text = e.ToString(), Selected = selected });

    //    }

    //  //  SelectListItem e = new SelectListItem ();
    //    //e.Selected = 

    //    //if (String.IsNullOrEmpty(currentValue))
    //    //{
    //    //    return new SelectList(risultato, "Value", "Text", null );
    //    //}

    //   // return new SelectList(risultato, "Value", "Text", ((int) Enum.Parse( typeof(TEnum),  currentValue.ToString())).ToString() );
    //    //return new SelectList(risultato, "Value", "Text", );
    //    //return new SelectList(risultato, "Value", "Text", "2");
    //   // return new SelectList(risultato, "Value", "Text");
    //    return risultato;
    //}




    #region "Combo Box"

    public static HtmlString getComboEnum<TEnum>(TEnum allValues, string selectedValue, string nome, bool isRequired)
    {

        //Debug.WriteLine(String.Format("selectedValue: {0}", selectedValue));

        //foreach (var value in Enum.GetValues(typeof(TEnum)))
        //{
        //    Debug.WriteLine(String.Format("{0}", value));
        //}


        string temp;

        string required = "";

        if (isRequired)
        {
            required = "required=\"required\"";
        }

        //data-inline=\"true\" 

        temp = String.Format("<select id=\"{0}\" name=\"{1}\"   data-mini=\"true\" {2}  >", nome.Replace(".", "_"), nome, required);

        temp += "<option value=\"\">---</option>";

        foreach (var e in Enum.GetValues(typeof(TEnum)))
        {

            if (e.ToString().Equals("Undefined"))
            {
                continue;
            }

            if (selectedValue.Equals(e.ToString()))
            {
                temp += String.Format("<option value=\"{0}\"  selected=\"selected\"  >{1}</option>", ((int)Enum.Parse(typeof(TEnum), e.ToString())).ToString(), e.ToString().Replace("_", " "));
            }
            else
            {
                temp += String.Format("<option value=\"{0}\">{1}</option>", ((int)Enum.Parse(typeof(TEnum), e.ToString())).ToString(), e.ToString().Replace("_", " "));
            }
        }


        temp += "</select>";

        return new HtmlString(temp);
    }
        



    #endregion



    public static void printRequest(System.Web.HttpRequest request)
    {
        foreach (string k in request.Form.AllKeys)
        {
            Debug.WriteLine(String.Format("Key: {0} \t Value: {1}", k, request[k]));
        }
    }


    public static void printRequest(System.Web.HttpRequestBase request)
    {

        foreach (string k in request.Form.AllKeys)
        {
            Debug.WriteLine(String.Format("Key: {0} \t Value: {1}", k, request[k]));
        }

    }
}
