using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public static class MyConstants
{
    public enum Esito
    {
        Ok,
        Errore
    }

    public class Profilo
    {
       public const string  ADMIN = "ADMIN";
        public const string BACKOFFICE = "BACKOFFICE";
        public const string FRONTOFFICE = "FRONTOFFICE";
        public const string COLLABORATORE = "COLLABORATORE";
    }

    public const bool CHECK_ASSEGANAZIONI_ENABLED = false;

    public const int STATO_INIZIALE_SEGNALAZIONE = 1;
    public static  System.Globalization.CultureInfo CultureInfoEN = new System.Globalization.CultureInfo("en-GB");

    public const string DATE_FORMAT = "{0:dd/MM/yyyy}";

}
