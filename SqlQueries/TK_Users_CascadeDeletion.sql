create or alter trigger TK_Users_CascadeDeletion
	on Users
	instead of delete
as
begin
	set nocount on;
	delete from Blocked_Users where (userid in (select id from deleted)
		or blockeduserid in (select id from deleted))
	delete from Friends where (userid in (select id from deleted)
		or friendid in (select id from deleted))
	delete from Friend_Requests where (senderid in (select id from deleted)
		or receiverid in (select id from deleted))
	delete from Group_Members where userid in (select id from deleted)
	delete from Messages where senderid in (select id from deleted)
	delete from Profiles where userid in (select id from deleted)
	delete from Settings where userid in (select id from deleted)
	delete from Strikes where userid in (select id from deleted)
	delete from UserReports where (userid in (select id from deleted)
		or reporteduserid in (select id from deleted))
	delete from Users where id in (select id from deleted)
end