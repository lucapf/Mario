USE Mediatori;


INSERT INTO PROFILO ( profilo_id, nome, date_added) VALUES ( 'ADMIN', 'Amministratre', GetDate());
GO
INSERT INTO PROFILO ( profilo_id, nome, date_added) VALUES ( 'BACKOFFICE', 'Back Office', GetDate());
GO
INSERT INTO PROFILO ( profilo_id, nome, date_added) VALUES ( 'FRONTOFFICE', 'Front Office', GetDate());
GO
INSERT INTO PROFILO ( profilo_id, nome, date_added) VALUES ( 'COLLABORATORE', 'Collaboratore', GetDate());
GO


-- admin = 21232F297A57A5A743894A0E4A801FC3
INSERT INTO UTENTE ( my_login, my_password, is_enabled) VALUES('admin' , '21232F297A57A5A743894A0E4A801FC3', 1);
INSERT INTO UtenteProfilo ( user_id, profilo_id,  date_added) VALUES(1 , 'ADMIN',  GetDate());


INSERT INTO UTENTE ( my_login, my_password, is_enabled) VALUES('operatore' , '21232F297A57A5A743894A0E4A801FC3', 1);
INSERT INTO UtenteProfilo ( user_id, profilo_id,  date_added) VALUES(2 , 'BACKOFFICE',  GetDate());