using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace mediatori.Models
{
    public class CopyObject
    {
        public static Object copy(object target, object source)
        {

            PropertyInfo[] sourceProperties = source.GetType().GetProperties();
            PropertyInfo[] targetProperties = target.GetType().GetProperties();
            foreach (PropertyInfo sourceProperty in sourceProperties)
            {
                PropertyInfo targetProperty = (from targetP in targetProperties
                                               where targetP.Name == sourceProperty.Name
                                               select targetP).First();
                if (sourceProperty != null)
                {
                    targetProperty.SetValue(target, sourceProperty.GetValue(source, null));
                }
            }
            return target;
        }

        public static Object simpleCompy(object target, object source)
        {

            PropertyInfo[] sourceProperties = source.GetType().GetProperties();
            PropertyInfo[] targetProperties = target.GetType().GetProperties();
            foreach (PropertyInfo sourceProperty in sourceProperties)
            {

                PropertyInfo targetProperty = (from targetP in targetProperties
                                               where targetP.Name == sourceProperty.Name
                                               select targetP).First();
                if (sourceProperty != null)
                {
                    targetProperty.SetValue(target, sourceProperty.GetValue(source, null));
                }
            }
            return target;
        }
    }
}