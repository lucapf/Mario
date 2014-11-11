using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Diagnostics;

public static class MyExtensions
{
    private static string DATA_MINI = "true";
    

    public static MvcHtmlString MyTextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
    {
        string elementName = ExpressionHelper.GetExpressionText(expression);

        MvcHtmlString myTextBox;


        var lambda = (LambdaExpression)expression;

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


        //*** DATA TYPE**//
        string dataType;
        dataType = memberExpression.Type.FullName;

        string temp;

        Debug.WriteLine(dataType);

        if (dataType.StartsWith("System.Nullable`1[[System.Decimal,"))
        {
            myTextBox = htmlHelper.TextBoxFor(expression, "{0:0.00}", new { type = "number", step = "0.1", min = "0", data_mini = DATA_MINI });
            temp = myTextBox.ToHtmlString().Replace(",",".");
            myTextBox = new MvcHtmlString(temp);
        }
        else if (dataType.StartsWith("System.Nullable`1[[System.Int32,"))
        {
            myTextBox = htmlHelper.TextBoxFor(expression, "{0:0}", new { type = "number", step = "1", min = "0", data_mini = DATA_MINI });
            temp = myTextBox.ToHtmlString().Replace(",", ".");
            myTextBox = new MvcHtmlString(temp);
        }
        else if (dataType.StartsWith("System.Nullable`1[[System.DateTime,"))
        {
            myTextBox = htmlHelper.TextBoxFor(expression, "{0:d}", new { type = "date", data_role = "date", data_mini = DATA_MINI });
        }
        else if (dataType == "System.String")
        {
            myTextBox = htmlHelper.TextBoxFor(expression, new { data_mini = DATA_MINI });
        }
        else
        {
            myTextBox = new MvcHtmlString(String.Format("<h1>Tipo non riconosciuto: {0} <h1>", dataType));
        }

        return myTextBox;
    }



    public static MvcHtmlString MyInputType<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
    {
        MvcHtmlString label = htmlHelper.LabelFor(expression);
        MvcHtmlString textBox = htmlHelper.MyTextBoxFor(expression);
        MvcHtmlString validation = htmlHelper.ValidationMessageFor(expression);


        var lambda = (LambdaExpression)expression;

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
        //*** Attributes ***//
        var requiredAttribute = memberExpression.Member.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.RequiredAttribute), false);

        bool isRequired = false;
        if (requiredAttribute != null && requiredAttribute.Length != 0)
        {
            isRequired = true;

            System.Text.RegularExpressions.Regex pattern = new System.Text.RegularExpressions.Regex("<label\\b[^>]*>(.*?)</label>");

            Debug.WriteLine(pattern.Match(label.ToHtmlString()));
            System.Text.RegularExpressions.Match m = pattern.Match(label.ToHtmlString());
            if (m.Success)
            {
                label = new MvcHtmlString(label.ToHtmlString().Replace(m.Groups[1].Value, m.Groups[1].Value + " *"));
            }
        }





        return new MvcHtmlString(label.ToHtmlString() + textBox.ToHtmlString() + validation.ToHtmlString());
    }





    public static int WordCount(this String str)
    {
        return str.Split(new char[] { ' ', '.', '?' }, StringSplitOptions.RemoveEmptyEntries).Length;
    }




    public static HtmlString Input(this object instance)
    {
        string temp;
        temp = instance.GetType().Name;


        Type t = instance.GetType();
        PropertyInfo[] properties = t.GetProperties();
        foreach (PropertyInfo property in properties)
        {
            Console.WriteLine(property.ToString());
        }


        return new HtmlString(temp);
    }


}

