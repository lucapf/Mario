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

    #region "___ JQUERY MOBILE __"

    private static string DATA_MINI = "true";
    private static bool UNOBTRUSIVE_VALIDATION = true;

    //public static HtmlString MyInputType<TProperty>(System.Linq.Expressions.Expression<Func<TProperty>> property)
    //{
    //    return MyInputType<TProperty>(property, String.Empty);
    //}

    //public static HtmlString MyInputType<TProperty>(System.Linq.Expressions.Expression<Func<TProperty>> property, string prefix)
    //{
    //    //http://blogs.msdn.com/b/csharpfaq/archive/2010/03/11/how-can-i-get-objects-and-property-values-from-expression-trees.aspx

    //    //http://thatextramile.be/blog/2011/01/prefixing-input-elements-of-partial-views-with-asp-net-mvc/

    //    var lambda = (LambdaExpression)property;

    //    MemberExpression memberExpression;
    //    if (lambda.Body is UnaryExpression)
    //    {
    //        var unaryExpression = (UnaryExpression)lambda.Body;
    //        memberExpression = (MemberExpression)unaryExpression.Operand;
    //    }
    //    else
    //    {
    //        memberExpression = (MemberExpression)lambda.Body;
    //    }


    //    //*** FULL NAME ***//
    //    string name = "";
    //    name = memberExpression.Member.Name;
    //    MemberExpression tempEx = memberExpression;
    //    while (tempEx.Expression.NodeType == ExpressionType.MemberAccess)
    //    {
    //        var propInfo = tempEx.Expression.GetType().GetProperty("Member");

    //        var propValue = propInfo.GetValue(tempEx.Expression, null) as System.Reflection.PropertyInfo;
    //        if (propValue != null)
    //        {
    //            name = propValue.Name + "." + name;
    //        }
    //        else
    //        {
    //            name = ((MemberExpression)tempEx.Expression).Member.Name + "." + name;
    //        }

    //        tempEx = tempEx.Expression as MemberExpression;
    //    }

    //    //        name = tempEx.Type.Name + "." + name;


    //    name = name.Replace("Model.", "");


    //    if (!String.IsNullOrEmpty(prefix))
    //    {

    //        name = prefix + name;
    //    }


    //    //*** VALUE ***//
    //    TProperty value = property.Compile()();
    //    Debug.WriteLine("Name: {0} - Value: {1} - Type: {2}", name, value, memberExpression.Type.FullName);


    //    //*** Attributes ***//
    //    var requiredAttribute = memberExpression.Member.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.RequiredAttribute), false);

    //    bool isRequired = false;
    //    if (requiredAttribute != null && requiredAttribute.Length != 0)
    //    {
    //        isRequired = true;
    //    }

    //    object[] display = memberExpression.Member.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.DisplayAttribute), false);
    //    string displayName = "";
    //    if (display != null && display.Length != 0)
    //    {
    //        displayName = (display[0] as System.ComponentModel.DataAnnotations.DisplayAttribute).Name;
    //        Debug.WriteLine("Display Attribute Value: " + displayName);


    //        if (isRequired)
    //        {
    //            displayName = displayName + " *";
    //        }
    //    }


    //    //*** DATA TYPE**//
    //    string dataType;
    //    dataType = memberExpression.Type.FullName;

    //    object[] dataTypeAttr = memberExpression.Member.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.DataTypeAttribute), false);
    //    if (dataTypeAttr != null && dataTypeAttr.Length != 0)
    //    {
    //        dataType = (dataTypeAttr[0] as System.ComponentModel.DataAnnotations.DataTypeAttribute).DataType.ToString();
    //        Debug.WriteLine("Data Type Attribute Value: " + dataType);
    //    }


    //    if (dataType == "System.String")
    //    {
    //        if (value == null)
    //        {
    //            return MyInputType(name, displayName, "", isRequired);
    //        }
    //        return MyInputType(name, displayName, value.ToString(), isRequired);
    //    }

    //    if (dataType.StartsWith("System.Nullable`1[[System.Decimal,"))
    //    {
    //        if (value == null)
    //        {
    //            return MyInputType(name, displayName, (decimal?)null, isRequired);
    //        }
    //        return MyInputType(name, displayName, decimal.Parse(value.ToString()), isRequired);
    //    }

    //    if (dataType == "Date" || dataType.StartsWith("System.Nullable`1[[System.DateTime,"))
    //    {
    //        if (value == null)
    //        {
    //            return MyInputType(name, displayName, (DateTime?)null, isRequired);
    //        }
    //        return MyInputType(name, displayName, DateTime.Parse(value.ToString()), isRequired);
    //    }



    //    if (dataType.StartsWith("System.Nullable`1[[System.Int32,"))
    //    {
    //        if (value == null)
    //        {
    //            return MyInputType(name, displayName, (int?)null, isRequired);
    //        }
    //        return MyInputType(name, displayName, int.Parse(value.ToString()), isRequired);
    //    }



    //    return new HtmlString(String.Format("<h1>Tipo non riconosciuto: {0} <h1>", dataType));
    //}

    public static HtmlString MyInputType(string name, string displayName, DateTime? value, bool isRequired)
    {
        string id = name.Replace(".", "_");

        StringBuilder unobtrusiveValidation = new StringBuilder();

        StringBuilder temp = new StringBuilder();
        if (!String.IsNullOrEmpty(displayName))
        {
            temp.Append(String.Format("<label for=\"{0}\">{1}</label>", id, displayName));
            temp.Append(Environment.NewLine);
        }

        string tempValue;
        if (value == null || value == DateTime.MinValue)
        {
            tempValue = "";
        }
        else
        {
            tempValue = value.Value.ToShortDateString();
        }


        temp.Append("<input type=\"date\" ");
        if (isRequired)
        {
            m_isRequired(temp, unobtrusiveValidation, name, displayName);
        }
        temp.Append(String.Format("data-role=\"date\" id=\"{0}\" name=\"{1}\" value=\"{2}\" data-mini=\"{3}\" />", id, name, tempValue, DATA_MINI));
        temp.Append(Environment.NewLine);
        temp.Append(unobtrusiveValidation);
        return new HtmlString(temp.ToString());
    }

    public static HtmlString MyInputType(string name, string displayName, decimal? value, bool isRequired)
    {
        string id = name.Replace(".", "_");

        StringBuilder unobtrusiveValidation = new StringBuilder();

        StringBuilder temp = new StringBuilder();
        if (!String.IsNullOrEmpty(displayName))
        {
            temp.Append(String.Format("<label for=\"{0}\">{1}</label>", id, displayName));
            temp.Append(Environment.NewLine);
        }
        else
        {
            displayName = name;
        }

        string tempValue;
        if (value == null)
        {
            tempValue = "";
        }
        else
        {
            tempValue = String.Format(MyConstants.CultureInfoEN, "{0:N2}", value).Replace(",", "");
        }


        temp.Append("<input type=\"number\" ");
        if (isRequired)
        {
            m_isRequired(temp, unobtrusiveValidation, name, displayName);
        }
        temp.Append(String.Format("step=\"0.1\" min=\"0\" id=\"{0}\" name=\"{1}\" value=\"{2}\" data-mini=\"{3}\" />", id, name, tempValue, DATA_MINI));
        temp.Append(Environment.NewLine);
        temp.Append(unobtrusiveValidation);
        return new HtmlString(temp.ToString());
    }

    public static HtmlString MyInputType(string name, string displayName, float? value, bool isRequired)
    {
        string id = name.Replace(".", "_");

        StringBuilder unobtrusiveValidation = new StringBuilder();

        StringBuilder temp = new StringBuilder();
        if (!String.IsNullOrEmpty(displayName))
        {
            temp.Append(String.Format("<label for=\"{0}\">{1}</label>", id, displayName));
            temp.Append(Environment.NewLine);
        }


        string tempValue;
        if (value == null)
        {
            tempValue = "";
        }
        else
        {
            tempValue = String.Format("{0:N2}", value);
        }

        temp.Append("<input type=\"number\" ");
        if (isRequired)
        {
            m_isRequired(temp, unobtrusiveValidation, name, displayName);
        }
        temp.Append(String.Format("step=\"0.1\" min=\"0\" id=\"{0}\" name=\"{1}\" value=\"{2}\" data-mini=\"{3}\" />", id, name, tempValue, DATA_MINI));
        temp.Append(Environment.NewLine);
        temp.Append(unobtrusiveValidation);

        return new HtmlString(temp.ToString());
    }

    public static HtmlString MyInputType(string name, string displayName, int? value, bool isRequired)
    {
        string id = name.Replace(".", "_");

        StringBuilder unobtrusiveValidation = new StringBuilder();


        StringBuilder temp = new StringBuilder();
        if (!String.IsNullOrEmpty(displayName))
        {
            temp.Append(String.Format("<label for=\"{0}\">{1}</label>", id, displayName));
            temp.Append(Environment.NewLine);
        }

        string tempValue;
        if (value == null)
        {
            tempValue = "";
        }
        else
        {
            tempValue = String.Format("{0:N0}", value);
        }


        temp.Append("<input type=\"number\" ");
        if (isRequired)
        {
            m_isRequired(temp, unobtrusiveValidation, name, displayName);
        }
        temp.Append(String.Format("step=\"1\" min=\"0\" id=\"{0}\" name=\"{1}\" value=\"{2}\" data-mini=\"{3}\" />", id, name, tempValue, DATA_MINI));
        temp.Append(Environment.NewLine);
        temp.Append(unobtrusiveValidation);

        return new HtmlString(temp.ToString());
    }

    public static HtmlString MyInputType(string name, string displayName, string value, bool isRequired)
    {
        string id = name.Replace(".", "_");

        StringBuilder unobtrusiveValidation = new StringBuilder();

        StringBuilder temp = new StringBuilder();
        if (!String.IsNullOrEmpty(displayName))
        {
            temp.Append(String.Format("<label for=\"{0}\">{1}</label>", id, displayName));
            temp.Append(Environment.NewLine);
        }
        else
        {
            displayName = name;
        }

        temp.Append("<input type=\"text\" ");
        if (isRequired)
        {
            m_isRequired(temp, unobtrusiveValidation, name, displayName);
        }
        temp.Append(String.Format(" id=\"{0}\" name=\"{1}\" value=\"{2}\" data-inline=\"true\" data-mini=\"{3}\" />", id, name, value, DATA_MINI));
        temp.Append(Environment.NewLine);
        temp.Append(unobtrusiveValidation);

        return new HtmlString(temp.ToString());
    }


    private static void m_isRequired(StringBuilder input, StringBuilder validation, string name, string displayName)
    {
        if (UNOBTRUSIVE_VALIDATION)
        {
            input.Append(String.Format("data-val=\"true\" data-val-required=\"Il campo {0} è obbligatorio.\" ", displayName.Replace("*", "")));
            validation.Append(String.Format("<br /><span class=\"field-validation-valid\"  data-valmsg-for=\"{0}\" data-valmsg-replace=\"true\"></span>", name));
            validation.Append(Environment.NewLine);
        }
        else
        {
            input.Append(" required ");
        }

    }

    public static HtmlString MyCheckBox(string name, bool isChecked, string label)
    {
        string id = name.Replace(".", "_");

        string temp;
        string strIsChecked = "";

        if (isChecked)
        {
            strIsChecked = String.Format("checked = \"checked\"");
        }

        temp = String.Format("<input type=\"checkbox\" id=\"{0}\" name=\"{1}\"  data-mini=\"true\" {2} />", id, name, strIsChecked);

        if (!String.IsNullOrEmpty(label))
        {
            temp += Environment.NewLine + String.Format(" <label for=\"{0}\">{1}</label>", name, label);
        }


        return new HtmlString(temp);
    }








    public static HtmlString MyDisplay<TProperty>(System.Linq.Expressions.Expression<Func<TProperty>> property)
    {
        //http://blogs.msdn.com/b/csharpfaq/archive/2010/03/11/how-can-i-get-objects-and-property-values-from-expression-trees.aspx

        //http://thatextramile.be/blog/2011/01/prefixing-input-elements-of-partial-views-with-asp-net-mvc/

        var lambda = (LambdaExpression)property;

        MemberExpression memberExpression;
        if (lambda.Body is UnaryExpression)
        {
            var unaryExpression = (UnaryExpression)lambda.Body;
            memberExpression = (MemberExpression)unaryExpression.Operand;
        }
        else
        {
            memberExpression = (MemberExpression)lambda.Body;
        }


        ////*** FULL NAME ***//
        string name = "";
        name = memberExpression.Member.Name;
        //MemberExpression tempEx = memberExpression;
        //while (tempEx.Expression.NodeType == ExpressionType.MemberAccess)
        //{
        //    var propInfo = tempEx.Expression.GetType().GetProperty("Member");

        //    var propValue = propInfo.GetValue(tempEx.Expression, null) as System.Reflection.PropertyInfo;
        //    name = propValue.Name + "." + name;

        //    tempEx = tempEx.Expression as MemberExpression;
        //}
        //name = name.Replace("Model.", "");


        //*** VALUE ***//
        TProperty value = property.Compile()();
        Debug.WriteLine("Name: {0} - Value: {1} - Type: {2}", name, value, memberExpression.Type.FullName);


        //*** Attributes ***//
        //var isRequired = memberExpression.Member.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.RequiredAttribute), false);

        object[] display = memberExpression.Member.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.DisplayAttribute), false);

        string displayName = "";
        displayName = name;

        if (display != null && display.Length != 0)
        {
            displayName = (display[0] as System.ComponentModel.DataAnnotations.DisplayAttribute).Name;
            Debug.WriteLine("Display Attribute Value: " + displayName);

            //if (isRequired != null)
            //{
            //    displayName = displayName + " *";
            //}
        }


        //*** DATA TYPE**//
        string dataType;
        dataType = memberExpression.Type.FullName;

        object[] dataTypeAttr = memberExpression.Member.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.DataTypeAttribute), false);
        if (dataTypeAttr != null && dataTypeAttr.Length != 0)
        {
            dataType = (dataTypeAttr[0] as System.ComponentModel.DataAnnotations.DataTypeAttribute).DataType.ToString();
            Debug.WriteLine("Data Type Attribute Value: " + dataType);
        }


        if (dataType == "System.String")
        {
            if (value == null)
            {
                return MyDisplay(displayName, "");
            }
            return MyDisplay(displayName, value.ToString());
        }

        if (dataType.StartsWith("System.Nullable`1[[System.Decimal,"))
        {
            if (value == null)
            {
                return MyDisplay(displayName, "");
            }
            return MyDisplay(displayName, String.Format("{0:N2}", decimal.Parse(value.ToString())));
        }

        if (dataType == "Date" || dataType.StartsWith("System.Nullable`1[[System.DateTime,"))
        {
            if (value == null)
            {
                return MyDisplay(displayName, "");
            }
            return MyDisplay(displayName, String.Format("{0}", DateTime.Parse(value.ToString()).ToShortDateString()));
        }



        if (dataType.StartsWith("System.Nullable`1[[System.Int32,"))
        {
            if (value == null)
            {
                return MyDisplay(displayName, "");
            }
            return MyDisplay(displayName, String.Format("{0:N0}", int.Parse(value.ToString())));
        }



        return new HtmlString(String.Format("<h1>Tipo non riconosciuto: {0} <h1>", dataType));
    }



    public static HtmlString MyDisplay(string label, string value)
    {
        StringBuilder temp = new StringBuilder();

        temp.Append(String.Format("<label for=\"{0}\">{1}</label>", "", label));
        temp.Append(Environment.NewLine);

        temp.Append(String.Format("<div>{0}</div>", "", value));

        return new HtmlString(temp.ToString());
    }



    #endregion


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
