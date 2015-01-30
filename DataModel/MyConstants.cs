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



    public const string TABLE_ROW_STYLE = "ui-body-a";
    public const string TABLE_ALTERNATING_ROW_STYLE = "ui-body-d";
    public const string TABLE_STYLE = "MyTable ui-responsive";
    public const string TABLE_HEADER_STYLE = "ui-bar-b";
    public const string TABLE_FOOTER_STYLE = "ui-bar-b";


    public const string FIXED_FOOTER_THEME = "b";

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

    public const string STATO_INIZIALE_PRATICA = "Pratica caricata";
    public const string STATO_INIZIALE_SEGNALAZIONE = "Segnalazione caricata";

    public static System.Globalization.CultureInfo CultureInfoEN = new System.Globalization.CultureInfo("en-GB");

    public const string DATE_FORMAT = "{0:dd/MM/yyyy}";

}
