CREATE OR ALTER PROCEDURE [dbo].[spViewUser]
	@id int
AS
BEGIN
	SELECT Users.*, Profiles.*, Settings.*
	FROM Users JOIN Profiles ON Users.id=Profiles.userid JOIN Settings ON Users.id=Settings.userid 
	WHERE Users.id=@id
END