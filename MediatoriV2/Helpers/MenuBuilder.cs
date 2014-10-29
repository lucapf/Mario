using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mediatori.Helpers
{

    public enum EMenuSection
    {
        ANAGRAFICA, SICUREZZA, CONFIGURAZIONI

    }
    public class MenuElement
    {
        public string display { get; set; }
        public string url { get; set; }
        public int livello { get; set; }
        public String role { get; set; }
        public int ordinamento { get; set; }
    }
    public static class MenuBuilder
    {
        private static Dictionary<EMenuSection, List<MenuElement>> configMenu { get; set; }
        private static void initMenu()
        {
            configMenu = new Dictionary<EMenuSection, List<MenuElement>>();
            configMenu.Add(EMenuSection.ANAGRAFICA,
                new List<MenuElement>(){
                    new MenuElement(){display="Segnalazioni", ordinamento=1,livello=1,url="GestioneSegnalazioni"},
                    new MenuElement(){display="Nuova Segn.", ordinamento=2,livello=2,url="GestioneSegnalazioni/Create"},
                    new MenuElement(){display="Cedenti", ordinamento=3,livello=1,url="Cedente"},
                    new MenuElement(){display="Registrazione Cedente", ordinamento=4,livello=2,url="Cedente/Create"},
                    new MenuElement(){display="Amministrazione", ordinamento=5,livello=1,url="Amministrazione"},
                    new MenuElement(){display="Nuova Amministratore", ordinamento=6,livello=2,url="Amministrazione/Create"},
                    new MenuElement(){display="Societa'", ordinamento=7,livello=1,url="SoggettoGiuridico"},
                    new MenuElement(){display="Societa Nuova", ordinamento=8,livello=1,url="SoggettoGiuridico/Create"},
                    new MenuElement(){display="Agenzia", ordinamento=9,livello=1,url="Agenzia"},
                    
                });
            configMenu.Add(EMenuSection.SICUREZZA,
               new List<MenuElement>(){
                    new MenuElement(){display="Nuovo utente", ordinamento=1,livello=1, role="Amministratore",url="Account/Register"},
                    new MenuElement(){display="Utenti", ordinamento=1,livello=1,role="Amministratore",url="Account/ListUsers"},
                    new MenuElement(){display="Modifica Passowrd", ordinamento=2,livello=1,role="Amministratore",url="Account/Manage"},
                    new MenuElement(){display="Logout", ordinamento=4,livello=3,url="Account/LogOff"}
                });
            configMenu.Add(EMenuSection.CONFIGURAZIONI,
              new List<MenuElement>(){
                    new MenuElement(){display="Province", ordinamento=1,livello=1,role="Amministratore",url="Configurazioni/Province"},
                    new MenuElement(){display="Toponimi", ordinamento=1,livello=1,role="Amministratore",url="Configurazioni/Toponimi"},
                    new MenuElement(){display="Fonti Pubblicitarie", ordinamento=1,livello=1,role="Amministratore",url="Configurazioni/fontePubblicitaria"},
                    new MenuElement(){display="Tipo Prestito", ordinamento=1,livello=1,role="Amministratore",url="Configurazioni/tipologiaPrestito"}, 
                    new MenuElement(){display="Tipo Azienda", ordinamento=1,livello=1,role="Amministratore",url="Configurazioni/tipologiaAzienda"},
                    new MenuElement(){display="Tipo Indirizzo", ordinamento=1,livello=1,role="Amministratore",url="Configurazioni/tipologiaIndirizzo"},
                    new MenuElement(){display="Tipo Impiego", ordinamento=1,livello=1,role="Amministratore",url="Configurazioni/tipoContrattoImpiego"},
                    new MenuElement(){display="Tipo Ente Rilascio", ordinamento=1,livello=1,role="Amministratore",url="Configurazioni/tipoEnteRilascio"}, 
                    new MenuElement(){display="Tipo Documento Identita", ordinamento=1,livello=1,role="Amministratore",url="Configurazioni/tipoDocumentoIdentita"},
                    new MenuElement(){display="Gruppi di lavorazione", ordinamento=1,livello=1,role="Amministratore",url="Configurazioni/tipoCampagnaPubblicitaria"},
                    new MenuElement(){display="Tipo Contatto", ordinamento=1,livello=1,role="Amministratore",url="Configurazioni/tipoContatto"},
                    new MenuElement(){display="Canale Acquisizione", ordinamento=1,livello=1,role="Amministratore",url="Configurazioni/tipoCanaleAcquisizione"},
                    new MenuElement(){display="Tipo Luogo Ritrovo", ordinamento=1,livello=1,role="Amministratore",url="Configurazioni/tipoLuogoRitrovo"},
                    new MenuElement(){display="Tipo Riferimento", ordinamento=1,livello=1,role="Amministratore",url="Configurazioni/tipoRiferimento"},
                    new MenuElement(){display="Tipo Prodotto", ordinamento=1,livello=1,role="Amministratore",url="Configurazioni/tipoProdotto"},
                    new MenuElement(){display="Tipo Categoria", ordinamento=1,livello=1,role="Amministratore",url="Configurazioni/tipoCategoriaAmministratore"},
                    new MenuElement(){display="Tipo Agenzia", ordinamento=1,livello=1,role="Amministratore",url="Configurazioni/tipoAgenzia"},
                    new MenuElement(){display="Tipo Erogazione", ordinamento=1,livello=1,role="Amministratore",url="Configurazioni/tipoErogazione"},
                    new MenuElement(){display="Tipo Assumibilia", ordinamento=1,livello=1,role="Amministratore",url="Configurazioni/tipoAssumibilitaAmministratore"},
                    new MenuElement(){display="Stato", ordinamento=1,livello=1,role="Amministratore",url="Configurazioni/stato"},
                    new MenuElement(){display="Nuova Rete ", ordinamento=1,livello=1,role="Amministratore",url="Rete/Create"}
                });
        }
        public static List<MenuElement> getMenuData(EMenuSection sezione, String[] ruoli)
        {
            if (configMenu == null) initMenu();
            List<MenuElement> listAElementiMenu = configMenu[sezione];

            if (ruoli == null)
            {
                return listAElementiMenu.OrderBy(m => m.display).OrderBy(m => m.ordinamento).ToList();
            }
            List<MenuElement> listaFiltrata = listAElementiMenu.Where(m => m.role == null || ruoli.Contains(m.role)).OrderBy(m => m.display).OrderBy(m => m.ordinamento).ToList();
            return listaFiltrata;

        }
    }
}