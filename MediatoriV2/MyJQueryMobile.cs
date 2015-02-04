using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;



public class MyJQueryMobile
{
    public enum IconType
    {
        undefined,
        edit,
        delete
    }




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

    #region "___ MyInputType ___"


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


        temp.Append("<input type=\"text\" ");
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
        temp.Append(String.Format(" id=\"{0}\" name=\"{1}\" value=\"{2}\" data-clear-btn=\"true\" data-inline=\"true\" data-mini=\"{3}\" />", id, name, value, DATA_MINI));
        temp.Append(Environment.NewLine);
        temp.Append(unobtrusiveValidation);

        return new HtmlString(temp.ToString());
    }

    #endregion

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

        temp = String.Format("<input type=\"checkbox\" id=\"{0}\" name=\"{1}\" value=\"true\"  data-mini=\"true\" {2} />", id, name, strIsChecked);

        if (!String.IsNullOrEmpty(label))
        {
            temp += Environment.NewLine + String.Format(" <label for=\"{0}\">{1}</label>", name, label);
        }


        return new HtmlString(temp);
    }


    public static HtmlString MyAnchorPopup(string url, string label, IconType ico)
    {
        if (!url.StartsWith("#"))
        {
            throw new ApplicationException("La url deve iniziare con un #");
        }
        string css = "";

        if (String.IsNullOrEmpty(label))
        {
            label = "No text";
            css = " ui-btn-icon-notext";
        }
        else
        {
            css = " ui-btn-icon-left";
        }


        if (ico != IconType.undefined)
        {
            css += " ui-icon-" + ico.ToString();
        }

        if (DATA_MINI == "true")
        {
            css += " ui-mini";
        }

        string temp;
        temp = String.Format("<a href=\"{0}\" data-rel=\"popup\" data-position-to=\"window\"  class=\"ui-btn ui-btn-inline ui-shadow ui-corner-all {1}\" >{2}</a> ", url.Trim(), css.Trim(), label.Trim());


        return new HtmlString(temp);

    }

    public static HtmlString MyAnchor(string url, string label, IconType ico)
    {
        string temp;
        string css = "";

        if (String.IsNullOrEmpty(label))
        {
            label = "No text";
            css = " ui-btn-icon-notext";
        }
        else
        {
            css = " ui-btn-icon-left";
        }

        if (String.IsNullOrEmpty(url))
        {
            url = "#";
        }
       

        if (ico != IconType.undefined)
        {
            css += " ui-icon-" + ico.ToString();
        }

        if (DATA_MINI == "true")
        {
            css += " ui-mini";
        }

        temp = String.Format("<a href=\"{0}\"  class=\"ui-btn ui-btn-inline ui-corner-all ui-shadow {1}\" >{2}</a> ", url.Trim(), css.Trim(), label.Trim());


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

        temp.Append("<div class=\"display-control\">");

        temp.Append(String.Format("<label for=\"{0}\">{1}</label>", "", label));
        temp.Append(Environment.NewLine);

        temp.Append(String.Format("<div>{1}</div>", "", value));

        temp.Append("</div>");

        return new HtmlString(temp.ToString());
    }






}
