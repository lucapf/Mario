var searchDone = false;

function contattoDetail(idContatto) {
    if (isNaN(idContatto) || idContatto == 0) {
        alert("codice contatto non valido");
        return;
    }
    window.location.href = contextPath + "/GestioneSegnalazioni/Create?codiceContatto=" + idContatto;
}

function ricercaContatto() {
    var clNome = $("#segnalazione_contatto_nome").val();
    var clCognome = $("#segnalazione_contatto_cognome").val();
    var cf = $("#segnalazione_contatto_codiceFiscale").val();

    if (!searchDone && clNome != "" && clCognome != "") {
        $.ajax({
            url: contextPath + "/Contatto/findContattoByNomeCognome",
            data: { nome: clNome, cognome: clCognome },
            cache: false,
            error: function (html) {
                $("#grigliaContattiDuplicati").html("");
                alert("errore ricerca di possibili contatti duplicati \n" + html)
            },
            success: function (html) {
                mostraDuplicati(html);
            }
        });
    }

    if (!searchDone && cf != "") {
        $.ajax({
            url: contextPath + "/Contatto/findContattoByCodiceFiscale",
            data: { codiceFiscale: cf },
            cache: false,
            error: function (html) {
                $("#grigliaContattiDuplicati").html("");
                alert("errore ricerca di possibili contatti duplicati \n" + html);

            },
            success: function (html) {
                mostraDuplicati(html);
            }
        });

    }

}
function mostraDuplicati(htmlString) {
    $("#grigliaContattiDuplicati").html(htmlString);
    $("#grigliaContattiDuplicati").show("slide", { direction: "right" }, 500);
    searchDone = true;
}