var guidUltimoElementoInserito = "";

/*
per gli elementi collegati a liste tramite il parametro BeginFormCollection recupera il guid associato agli elementi 
delle form.
*/
function getGUID(htmlString) {
    var elementoBase = $('<p />').html(htmlString)
    guidUltimoElementoInserito = elementoBase.find("input[type='hidden']").val()
    return guidUltimoElementoInserito;
}

function getUrlParameter(sParam) {
    var sPageURL = window.location.search.substring(1);
    var sURLVariables = sPageURL.split('&');
    for (var i = 0; i < sURLVariables.length; i++) {
        var sParameterName = sURLVariables[i].split('=');
        if (sParameterName[0] == sParam) {
            return sParameterName[1];
        }
    }
}

/*
Effettua il caricamento della combo province, aggancia all'elemento il popolamento automatico della Combo comuni e aggiorna 
i parametri di validazione.
idElementoProvincia : codice Elemento provincia
idElementoComune    : codice Elemento comune
*/
function popolaComboProvince(idElementoProvincia, idElementoComune) {

    //alert("popolaComboProvince");
    $.ajax({
        url: contextPath + "/Configurazioni/popolaDropDownlistProvince",
        error: function () { alert("popolamento lista Province fallito"); },
        success: function (html) {

            $('#' + idElementoProvincia).html("<option name=''></option>" + html);

            //creo l'handler per il comune
            $('#' + idElementoProvincia).change(function () {
                popolaComboComuni(idElementoProvincia, idElementoComune);
            })
            refreshValidation("createForm");

        }
    })
}


/*
   Popola i valori della combo comune in funzione della provincia selezionata. 
   Effettua l'aggiornamento dei dati di validazione
*/
function popolaComboComuni(idElementoProvincia, idElementoComune) {
    //alert("popolaComboComuni");
    //+ '__comune_denominazione'
    var provinciaSelezionata = $('#' + idElementoProvincia).val();

    //alert("provinciaSelezionata: " + provinciaSelezionata);
    if (provinciaSelezionata == "") return true;


    $.getJSON(contextPath + "/Configurazioni/popolaDropDownlistComuni",
                  { comboComunElementId: idElementoComune, denominazioneProvincia: provinciaSelezionata },
                  function (data) {
                      // alert("popolaComboComuni: " + data.html);
                      $("#" + data.idElemento).empty();
                      $("#" + data.idElemento).append($('<option/>', { value: "", text: "---" }));
                      $("#" + data.idElemento).append(data.html);
                      $("#" + data.idElemento).val(provinciaSelezionata).change();
                      refreshValidation("createForm");
                  });
};


/*
 permette di agganciare gli eventi di validazione ad una specifica form.
 idForm : IdElemento Form.
*/
function refreshValidation(idForm) {
    $("#" + idForm).removeData("validator");
    $("#" + idForm).removeData("unobtrusiveValidation");
    $.validator.unobtrusive.parse("#" + idForm);
}
/*
permette di visualizzare il dettaglio
*/
function FireAntDetailEventDetection_MostraDettaglio(idFullDetail, idShortDetail, idBtnModifica, idBtnAnnulla, idBtnSalva, idBtnHideDetail) {
    if (idFullDetail != null) {
        $("#" + idFullDetail).show("slide", { direction: "left" }, 500);

    }
    if (idShortDetail != null) {
        $("#" + idShortDetail).hide("slide", { direction: "right" }, 500);
    }
    if (idBtnModifica != null) {
        $("#" + idBtnModifica).show();
    }
    if (idBtnHideDetail != null) {
        $("#" + idBtnHideDetail).show();
    }

    if (idBtnSalva != null) {
        $("#" + idBtnSalva).hide();
    }
    if (idBtnAnnulla != null) {
        $("#" + idBtnAnnulla).hide();
    }
}
function FireAntDetailEventDetection_NascondiDettaglio(idFullDetail, idShortDetail, idBtnModifica, idBtnAnnulla, idBtnSalva, idBtnHideDetail) {

    if (idFullDetail != null) {
        $("#" + idFullDetail).hide('slide', { direction: 'right' }, 500);
    }
    if (idShortDetail != null) {
        $("#" + idShortDetail).show("slide", { direction: "left" }, 500);
    }
    if (idBtnModifica != null) {
        $("#" + idBtnModifica).hide();
    }
    if (idBtnSalva != null) {
        $("#" + idBtnSalva).hide();
    }
    if (idBtnAnnulla != null) {
        $("#" + idBtnAnnulla).hide();
    }
    if (idBtnHideDetail != null) {
        $("#" + idBtnHideDetail).hide();
    }

}
/*
"0", "/DocumentoIdentita/DocumentoIdentitaPartialById", "DIdivMesg0", "DIdiv0", "DIbtnEdit0", "DIbtnAnnulla0", "DIbtnSalva0", "DIForm0", ""

*/

function FireAntEditHelper_ModificaDati(idEntita, urlPercorso, idDivMessage, idDivElement, idDivFullDetail, idDivShortDetail, idBtnEdit, idBtnAnnulla, idBtnSalva, idBtnHideDetail, idFormModifica, styleCustomAttributes) {
    alert("FireAntEditHelper_ModificaDati: " + urlPercorso);
    $("#" + idDivMessage).html("caricamento in corso");
    $.ajax({
        url: urlPercorso,
        data: { id: idEntita, tipoAzione: "MODIFICA" },
        cache: false,
        error: function (html) {
            $("#" + idDivMessage).html("");
            alert("errore nella gestione della richiesta\n" + html)
        },
        success: function (html) {
            $("#" + idDivMessage).html("");

            $("#" + idDivFullDetail).html(html);
            //Rutigliano 24/11/2014 jquery mobile
            $("#" + idDivFullDetail).trigger('create');

            refreshValidation(idFormModifica);

            $("#" + idBtnEdit).hide();
            $("#" + idBtnAnnulla).show();
            $("#" + idBtnSalva).show();
            $("#" + idBtnHideDetail).hide();
        }
    });
}
/*
   "1005", "/DocumentoIdentita/DocumentoIdentitaPartialById","DIdivMesg1005","DIFull1005",
   "DIbtnEdit1005","DIbtnAnnulla1005","DIbtnSalva1005","DIForm1005","" 
*/
function FireAntEditHelper_AnnullaModificaDati(idEntita, urlPercorso, idDivMessage, idDivElement, idDivFullDetail, idDivShortDetail, idBtnEdit, idBtnAnnulla, idBtnSalva, idBtnHideDetail, idFormModifica, styleCustomAttributes) {
    $("#" + idDivMessage).html("caricamento in corso");

    $.ajax({
        url: urlPercorso,
        data: { id: idEntita, tipoAzione: "VISUALIZZAZIONE" },
        cache: false,
        error: function (html) {
            $("#" + idDivMessage).html("");
            alert("errore nella gestione della richiesta\n" + html)
        },
        success: function (html) {
            $("#" + idDivMessage).html("");

            $("#" + idDivFullDetail).html(html);
            refreshValidation(idFormModifica)
            $("#" + idBtnEdit).hide();
            $("#" + idBtnAnnulla).hide();
            $("#" + idBtnSalva).hide();
            $("#" + idBtnHideDetail).show();


        }
    });
}
function FireAntEditHelper_AnnullaInserimento(idBtnEdit, idBtnAnnulla, idBtnSalva, idBtnHideDetail, idDivElement) {
    $("#" + idBtnEdit).show();
    $("#" + idDivElement).html("");
    $("#" + idBtnAnnulla).hide();
    $("#" + idBtnSalva).hide();
    $("#" + idBtnHideDetail).hide();
}


/*
"DIForm1005", "DIdiv1005", "DIbtnEdit1005", "DIbtnAnnulla1005", "DIbtnSalva1005"
*/
function FireAntEditHelper_SubmitInnerForm(modelId, url, codiceMessage, idDiv, idDivElement, idDivShort, idBtnEdit, idBtnAnnulla, idBtnSalva, idBtnHideDetail, idForm) {
    if ($("#" + idForm).valid()) {
        $("#" + idForm).submit();
    }


};

