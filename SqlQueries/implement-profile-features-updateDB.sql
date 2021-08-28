/*
* Sql server query pentru:
* - modificare tip imagine in Profile din `string` in `Image`
* - adaugare coloana noua in Profiles numita `statusmessage` 
* - modificare lungime maxima a coloanei `displayname` (din 20 la 255 de caractere)
*/

ALTER TABLE [Profiles]
ALTER COLUMN [image] Image;

ALTER TABLE [Profiles]
ADD [statusmessage] varchar(1024) NOT NULL DEFAULT 'Hi there';

ALTER TABLE [Profiles]
ALTER COLUMN [displayname] varchar (255) NOT NULL;