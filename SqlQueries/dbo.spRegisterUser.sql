CREATE OR ALTER PROCEDURE [dbo].[spRegisterUser]
	@email varchar(255),
	@password varchar(255),
	@firstName varchar(20),
	@lastName varchar(20)
AS
	INSERT INTO dbo.Users( email, password, firstname, lastname, verified, joindate, token)
	VALUES (@email, HASHBYTES('SHA2_256', @password), @firstName, @lastName, 0, GETDATE(), '0')
RETURN 0