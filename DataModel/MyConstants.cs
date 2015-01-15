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



    public const string ROW_STYLE = "ui-body-a";
    public const string ALTERNATING_ROW_STYLE = "ui-body-d";

    public enum MySessionData
    {
        ProdottiSimulazioneFinanziaria
    }

    public class Profilo
    {
        public const string AMMINISTRATORE = "AMMINISTRATORE";
        public const string DIPENDENTE = "DIPENDENTE";
        public const string COLLABORATORE = "COLLABORATORE";
    }

    public const bool CHECK_ASSEGANAZIONI_ENABLED = true;

    public const int STATO_INIZIALE_SEGNALAZIONE = 1;
    public static System.Globalization.CultureInfo CultureInfoEN = new System.Globalization.CultureInfo("en-GB");

    public const string DATE_FORMAT = "{0:dd/MM/yyyy}";

}
