CREATE   PROCEDURE [dbo].[spViewUser]
	@id int,
	@token varchar(255)
AS
BEGIN
	SELECT Users.*, Profiles.*, Settings.*
	FROM Users JOIN Profiles ON Users.id=Profiles.userid JOIN Settings ON Users.id=Settings.userid 
	WHERE Users.id=@id AND Users.token=@token
END