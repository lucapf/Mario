using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mediatori.Models
{
    public class FlipswitchModel
    {
        private string _name;
        private bool _isChecked;

        public string name
        {
            get { return _name; }
            set { _name = value; }
        }
        public bool isChecked
        {
            get { return _isChecked; }
            set { _isChecked = value; }
        }

        public FlipswitchModel(string name, bool isChecked)
        {
            _name = name;
            _isChecked = isChecked;
        }
    }
}