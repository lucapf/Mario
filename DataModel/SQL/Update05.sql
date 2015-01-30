--INSERT INTO ISTITUTO (nome, dataInserimento, applicativo, url) VALUES ( 'Techub', GetDate(), 'CreditoLab','http://creditolab-atlantide-collaudo.techub.lan:8080/PccWS/PccImpl')

--INSERT INTO ISTITUTO (nome, dataInserimento, applicativo, url) VALUES ( 'Techub', GetDate(), 'CreditoLab','https://creditolab-atlantide-collaudo.techub.it:20443/PccWS/PccImpl')

GO
ALTER TABLE [UTENTE] ADD istituto_id  int NOT NULL DEFAULT (1)
GO
ALTER TABLE [UTENTE]  WITH CHECK ADD  CONSTRAINT [UtenteFKIstituto] FOREIGN KEY(istituto_id) REFERENCES [istituto] ([id])
GO
ALTER TABLE [UTENTE] CHECK CONSTRAINT [UtenteFKIstituto]
GO
-- rimuovo il defautl a 1
ALTER TABLE [UTENTE]  ALTER COLUMN istituto_id  int NOT NULL 


CREATE TABLE [dbo].[UtenteCredenzialiIstituto](
	[user_id]		int NOT NULL,
	[istituto_id]	int NOT NULL,
	[login]			nvarchar(255) NOT NULL,
	[password]		nvarchar(255) NOT NULL,
	[date_added]	datetime NOT NULL,
 CONSTRAINT [UtenteCredenzialiIstitutoPK] PRIMARY KEY CLUSTERED 
(
	[user_id] ASC,
	[istituto_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ) 
GO

ALTER TABLE [UtenteCredenzialiIstituto] WITH CHECK ADD  CONSTRAINT [CredenzialiIstitutoFKUtente] FOREIGN KEY([user_id]) REFERENCES [dbo].UTENTE ([user_id])
GO

ALTER TABLE [UtenteCredenzialiIstituto] CHECK CONSTRAINT [CredenzialiIstitutoFKUtente]
GO

ALTER TABLE [UtenteCredenzialiIstituto]  WITH CHECK ADD  CONSTRAINT [CredenzialiIstitutoFKIstituto] FOREIGN KEY([istituto_id]) REFERENCES [dbo].[istituto] (id)
GO

ALTER TABLE [UtenteCredenzialiIstituto] CHECK CONSTRAINT [CredenzialiIstitutoFKIstituto]
GO

ALTER TABLE [UtenteCredenzialiIstituto] ADD  DEFAULT (getdate()) FOR [date_added]
GO



INSERT INTO STATO (DESCRIZIONE, STATOBASE, ENTITAASSOCIATA) VALUES ( 'Pratica caricata', 1, 2);