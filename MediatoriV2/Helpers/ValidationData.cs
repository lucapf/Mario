using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace mediatori.Helpers
{
    public sealed  class ValidationData : ValidationAttribute
    {
        
        public override bool IsValid(object value)
        {
            var required = new RequiredAttribute();
            if ( value.ToString().Equals("01/01/0001 00:00:00"))
            {
                value = null;
            }
            return required.IsValid(value);
        }
    }
}