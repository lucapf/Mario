USE Mediatori;


INSERT INTO PROFILO ( profilo_id, nome, date_added) VALUES ( 'ADMIN', 'Amministratre', GetDate());
GO
INSERT INTO PROFILO ( profilo_id, nome, date_added) VALUES ( 'BACKOFFICE', 'Back Office', GetDate());
GO
INSERT INTO PROFILO ( profilo_id, nome, date_added) VALUES ( 'FRONTOFFICE', 'Front Office', GetDate());
GO
INSERT INTO PROFILO ( profilo_id, nome, date_added) VALUES ( 'COLLABORATORE', 'Collaboratore', GetDate());
GO
INSERT INTO GRUPPO (  nome, date_added) VALUES ( 'Administrators', GetDate());



-- admin = 21232F297A57A5A743894A0E4A801FC3
INSERT INTO UTENTE ( my_login, my_password, is_enabled) VALUES('admin' , '21232F297A57A5A743894A0E4A801FC3', 1);
INSERT INTO UtenteProfilo ( user_id, profilo_id,  date_added) VALUES(1 , 'ADMIN',  GetDate());
INSERT INTO UtenteGruppo ( user_id, gruppo_id,  date_added) VALUES(1 , 1,  GetDate());

-- password = 5F4DCC3B5AA765D61D8327DEB882CF99
INSERT INTO UTENTE ( my_login, my_password, is_enabled) VALUES('operatore' , '5F4DCC3B5AA765D61D8327DEB882CF99', 1);
INSERT INTO UtenteProfilo ( user_id, profilo_id,  date_added) VALUES(2 , 'BACKOFFICE',  GetDate());

INSERT INTO UTENTE ( my_login, my_password, is_enabled) VALUES('collaboratore' , '5F4DCC3B5AA765D61D8327DEB882CF99', 1);
INSERT INTO UtenteProfilo ( user_id, profilo_id,  date_added) VALUES(3 , 'COLLABORATORE',  GetDate());

INSERT INTO UTENTE ( my_login, my_password, is_enabled) VALUES('frontoffice' , '5F4DCC3B5AA765D61D8327DEB882CF99', 1);
INSERT INTO UtenteProfilo ( user_id, profilo_id,  date_added) VALUES(4 , 'FRONTOFFICE',  GetDate());

INSERT INTO UTENTE ( my_login, my_password, is_enabled) VALUES('amministratore' , '5F4DCC3B5AA765D61D8327DEB882CF99', 1);
INSERT INTO UtenteProfilo ( user_id, profilo_id,  date_added) VALUES(5 , 'ADMIN',  GetDate());



