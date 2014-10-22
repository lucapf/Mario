USE [Mediatori]
GO

INSERT INTO [dbo].[preventivo]
           ([progressivo]
           ,[nomeProdotto]
           ,[importoRata]
           ,[durata]
           ,[tabellaFinanziaria]
           ,[importoCoperturaVita]
           ,[importoCoperturaImpego]
           ,[dataInserimento]
           ,[dataDecorrenza]
           ,[importoProvvigioni]
           ,[montante]
           ,[importoInteressi]
           ,[speseAttivazione]
           ,[speseIncasso]
           ,[oneriFiscali]
           ,[importoImpegniDaEstinguere]
           ,[nettoCliente]
           ,[tan]
           ,[taeg]
           ,[teg]
           ,[dataConferma]
           ,[Segnalazione_id])
     VALUES
           (
		   18
           ,'nomeProdotto'
           ,2
           ,2
           ,'tabellaFinanziaria'
           ,18.2
           ,18.2
           ,GETDATE()
           ,GETDATE()
           ,3
           ,3
           ,3
           ,18.2
           ,18.2
           ,18.2
           ,18.2
           ,18.2
           ,4
           ,4
           ,4
           ,GETDATE()
           ,3)
GO


