USE [GeoChat_DB]
GO
/****** Object:  StoredProcedure [dbo].[spUpdatePassword]    Script Date: 8/25/2021 7:53:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE OR ALTER   PROCEDURE [dbo].[spUpdatePassword]
	@userId int,
	@newPassword varchar(255)
AS
	UPDATE dbo.Users SET password=HASHBYTES('SHA2_256', @newPassword) WHERE id=@userId
RETURN 0