select * from segnalazione where id =1008


update segnalazione set cedenteId = contattoId  where id =1008

update segnalazione set Discriminator = 'Segnalazione', preventivoConfermatoId = null  where id =1008


--select * from persona_fisica where id = 1007

--update persona_fisica set tipoPersonaFisica = 'Contatto'  where id = 1007

select * from preventivo where id = 1009

update preventivo set dataConferma = null  where id = 1009