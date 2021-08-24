alter table Profiles add reputation int not null default (0)

create table Strikes
(
id int primary key identity(1,1),
userid int not null foreign key references Users(id),
firststrike bit not null default(0),
secondstrike bit not null default(0),
thirdstrike bit not null default(0),
unbandate date default(null)
);

create table UserReports
(
id int primary key identity(1,1),
userid int foreign key references Users(id),
reporteduserid int foreign key references Users(id),
);

insert into Strikes(userid)
select Users.id from Users