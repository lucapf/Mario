using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mediatori.Models
{
    public class MyItem
    {
        public string Value { get; set; }
        public string Text { get; set; }

        public MyItem()
        {
        }

        public MyItem(string value, string text)
        {
            Value = value;
            Text = text;
        }

    }
}