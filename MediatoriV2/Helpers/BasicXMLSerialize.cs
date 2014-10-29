using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace mediatori.Helpers
{
    public class BasicXMLSerialize
    {
        public static String serialize(Object o, Type typeOfObject)
        {
            String  retValue; 
            var ser = new XmlSerializer(typeOfObject);
             StringWriter writer = new StringWriter();
             ser.Serialize(writer, o);
             retValue = ser.ToString();
             writer.Close();
             return retValue;
        }
    }
}