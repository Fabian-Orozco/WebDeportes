/*
	Tabla que almacena información sobre deportes
	Creada por Fabián Orozco - B95690
	Utiliza procedimientos almacenados para las acciones CRUD, manejados por transacciones.
	2do Examen - UCR - CI-0126 - II Ciclo 2022.
*/
use B95690

-- Creación de tabla
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Deporte](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](75) NOT NULL,
	[PaisOrigen] [varchar](56) NOT NULL,
	[CaracteristicaPrincipal] [varchar](max) NOT NULL,
	[JugadoresPorEquipo] [int] NOT NULL,
	[EsOlimpico] [bit] NOT NULL,
 CONSTRAINT [PK_Deporte] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Deporte] ADD  CONSTRAINT [DF_Deporte_PaisOrigen]  DEFAULT ('-') FOR [PaisOrigen]
GO

ALTER TABLE [dbo].[Deporte] ADD  CONSTRAINT [DF_Deporte_CaracteristicaPrincipal]  DEFAULT ('-') FOR [CaracteristicaPrincipal]
GO

ALTER TABLE [dbo].[Deporte] ADD  CONSTRAINT [DF_Deporte_JugadoresPorEquipo]  DEFAULT ((0)) FOR [JugadoresPorEquipo]
GO

ALTER TABLE [dbo].[Deporte] ADD  CONSTRAINT [DF_Deporte_EsOlimpico]  DEFAULT ((0)) FOR [EsOlimpico]
GO

-- Procedures
-- READ
go
create or alter procedure ObtenerDeportes as begin
    set transaction isolation level read committed
		begin try
			begin transaction ObtenerDeportes
				select ID, Nombre, PaisOrigen, CaracteristicaPrincipal, JugadoresPorEquipo, EsOlimpico
				from dbo.Deporte
				order by Nombre asc
			commit transaction ObtenerDeportes
		end try
	begin catch 
		rollback transaction ObtenerDeportes
	end catch
end

-- CREATE
go
create or alter procedure AgregarDeporte (
	@Nombre varchar(75), 
	@PaisOrigen varchar(50), 
	@CaracteristicaPrincipal varchar(max), 
	@JugadoresPorEquipo int, 
	@EsOlimpico bit
)
as begin
    set transaction isolation level read uncommitted
		begin try
			begin transaction AgregarDeporte
				insert into dbo.Deporte (Nombre, PaisOrigen, CaracteristicaPrincipal, JugadoresPorEquipo, EsOlimpico) 
				values (@Nombre, @PaisOrigen, @CaracteristicaPrincipal, @JugadoresPorEquipo, @EsOlimpico)
			commit transaction AgregarDeporte
		end try
	begin catch 
		rollback transaction AgregarDeporte
	end catch
end

-- DELETE
go
create or alter procedure EliminarDeporte (
	@ID bigint
)
as begin
    set transaction isolation level read committed
		begin try
			begin transaction EliminarDeporte
				delete from dbo.Deporte 
				where ID = @ID
			commit transaction EliminarDeporte
		end try
	begin catch 
		rollback transaction EliminarDeporte
	end catch
end

-- UPDATE
go
create or alter procedure ActualizarDeporte (
	@ID bigint, 
	@Nombre varchar(75), 
	@PaisOrigen varchar(50), 
	@CaracteristicaPrincipal varchar(max), 
	@JugadoresPorEquipo int, 
	@EsOlimpico bit
)
as begin
    set transaction isolation level read committed
		begin try
			begin transaction ActualizarDeporte
				update dbo.Deporte
				set Nombre = @Nombre, PaisOrigen = @PaisOrigen, CaracteristicaPrincipal = @CaracteristicaPrincipal, 
				JugadoresPorEquipo = @JugadoresPorEquipo, EsOlimpico = @EsOlimpico
				where ID = @ID
			commit transaction ActualizarDeporte
		end try
	begin catch 
		rollback transaction AgregarDeporte
	end catch
end