


create table Tramites (
	id int identity(1,1) primary key,
	Nombre nvarchar(100) not null,
	Descripcion nvarchar(max),
	Precio decimal(10,2) not null,
	Fecha datetime default getdate()
	);