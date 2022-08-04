create table Usuario(
idUser int primary key identity(1,1),
email varchar(100),
pass varchar(100)
)

create proc RegistrarUser(
@email varchar(100),
@pass varchar(100),
@Registrado bit output,
@Mensaje varchar(100) output
)

as 
begin 

if(not exists(select *from Usuario where email=@email))
begin 
insert into Usuario(email,pass) values (@email,@pass)

set @Registrado = 1
set @Mensaje = 'Usuario Registrado'
end 
else 
begin 
set @Registrado=0
set @Mensaje='Correo ya existe'
end
end

create proc ValidarUser(
@email varchar (100),
@pass varchar(100)
)

as 
begin 

if (exists(select * from Usuario where email = @email and @pass = @pass))

select idUser from Usuario where email = @email and pass = @pass
else
select '0'
end


declare @registrado bit, @mensaje varchar(100)

exec RegistrarUser 'daniel@gmail.com','tEGhNFq4oZh9WG9Gwc1Sqzfel5MIb9YaZCoS5z+efF9kpC+Lh1gJ7r3r7cMBYkMA', @registrado output , @mensaje output 

select @registrado
select @mensaje

exec ValidarUser 'daniel@gmail.com','tEGhNFq4oZh9WG9Gwc1Sqzfel5MIb9YaZCoS5z+efF9kpC+Lh1gJ7r3r7cMBYkMA'

select * from Usuario


exec sp_dropserver 'LAPTOP-BQTIKUOI\SQL';
GO
exec sp_addserver 'local',local;
GO

SELECT @@SERVERNAME AS 'Server Name';