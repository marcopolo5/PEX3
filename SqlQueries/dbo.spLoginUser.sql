CREATE OR ALTER PROCEDURE [dbo].[spLoginUser]
	@email varchar(255),
	@password varchar(255)
AS
DECLARE
	@currentPassword varchar(255),
	@token varchar(255)

	SELECT @currentPassword = password 
		FROM dbo.Users 
		WHERE email = @email

	IF(@currentPassword = HASHBYTES('SHA2_256', @password) OR (@currentPassword is NULL))
	BEGIN
		SET @token = CRYPT_GEN_RANDOM(255)
	END
	ELSE
	BEGIN
		SET @token = '0'
	END
	UPDATE dbo.Users
	SET token = @token
	WHERE email = @email AND password = @currentPassword

	SELECT @token AS 'Token'