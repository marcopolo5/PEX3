USE [GeoChat_DB]
GO
/****** Object:  StoredProcedure [dbo].[spCheckPassword]    Script Date: 8/25/2021 7:52:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[spCheckPassword]
	@email varchar(255),
	@password varchar(255)
AS
DECLARE
	@currentPassword varchar(255),
	@valid int

	SELECT @currentPassword = password 
		FROM dbo.Users 
		WHERE email = @email

	
	IF(@currentPassword = HASHBYTES('SHA2_256', @password))
	BEGIN
		SET @valid = 1
	END
	ELSE
	BEGIN
		IF(@currentPassword is NULL)
		BEGIN
			SET @valid = 2
		END
	ELSE
	BEGIN	
		SET @valid = 0
	END
	END

	SELECT @valid AS 'Valid'