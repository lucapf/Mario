using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;



public class MyMessage
{
    public enum MyMessageType {
        Success,
        Failed
    }

    public string testo { get; set; }
    public MyMessageType tipo { get; set; }

    public MyMessage(MyMessageType tipo , string messaggio){
        this.testo = messaggio;
        this.tipo = tipo;
    }

}
