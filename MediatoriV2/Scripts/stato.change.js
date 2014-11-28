function valorizzaStatiDisponibili(){
    if (!statoFocusOn){
        return;
    }
    var codiceStato= $("#codiceStatoCorrente").val();
    $.getJSON( contextPath +"Stato/targets",{codiceStato:codiceStato,entita:"SEGNALAZIONE"}, 

function (jsonStati){
              
    var items=[];
    items.push("<option/>");
    $.each(jsonStati, function(i,stato){
        items.push("<option value='"+stato.id+"'>"+
              stato.descrizione+"</option>");
    });
                    
                    
    $("#gestioneStato").html("");
    $("<input/>",{id:"btnAnnullaGestioneStato",type:"button", value:"Annulla",onclick:"h2stato()"}).appendTo("#gestioneStato");
    $("<input/>",{id:"btnSalvaGestioneStato",type:"button", value:"Salva",onclick:"save()"}).appendTo("#gestioneStato");

    $("<select/>",{id:"selectGestioneStato",html: items.join("")}).appendTo("#gestioneStato");
               

    $("<br/>").appendTo("#gestioneStato");
    $("#btnSalvaGestioneStato").button();
    $("#btnAnnullaGestioneStato").button();

    $("#selectGestioneStato").selectmenu().addClass( "selectModificaStato" );
})
}; 
function save(){
    var statoSelezionato = $("#selectGestioneStato").val();
    if (statoSelezionato==""){
        displayError("selezionare lo stato di destinazione");
        return ;
    }
    $.post(contextPath +"Stato/update", {codiceStato: statoSelezionato, codiceEntita:@Model.id , entita:"SEGNALAZIONE"})
.done(function(jsonResponse){h2stato(jsonResponse);displayMessage("aggiornamento avvenuto con successo")});
}
function h2stato(descrizioneStato){
        
    var descrizione="stato " + "@descrizioneStato";

    if (descrizioneStato!=null){
        descrizione="stato " + descrizioneStato;
    }

    $("#gestioneStato").html("");

    $("<h2/>",{
        style:"text-align:right;display:block;width:100%",
        id:"labelStato",
        html:descrizione
    }).appendTo("#gestioneStato");

    $("#labelStato").mouseenter(function(){
        statoFocusOn=true;
        setTimeout(function(){valorizzaStatiDisponibili()},1000)});
    $("#labelStato").mouseleave(statoFocusOn=false);  
    statoFocusOn=false;
}