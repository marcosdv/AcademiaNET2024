CREATE TABLE [TbCategoria](
	[CatId] [int] IDENTITY(1,1) NOT NULL,
	[CatNome] [varchar](50) NOT NULL,
 CONSTRAINT [PK_TbCategoria] PRIMARY KEY CLUSTERED 
(
	[CatId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [TbNoticia](
	[NotId] [int] IDENTITY(1,1) NOT NULL,
	[NotTitulo] [varchar](200) NOT NULL,
	[NotTexto] [text] NOT NULL,
	[NotData] [datetime] NOT NULL,
	[CatId] [int] NOT NULL,
 CONSTRAINT [PK_TbNoticia] PRIMARY KEY CLUSTERED 
(
	[NotId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [TbNoticia]  WITH CHECK ADD  CONSTRAINT [FK_TbNoticia_TbCategoria] FOREIGN KEY([CatId])
REFERENCES [TbCategoria] ([CatId])
GO

ALTER TABLE [TbNoticia] CHECK CONSTRAINT [FK_TbNoticia_TbCategoria]
GO


CREATE VIEW [VwNoticia]
AS
SELECT        TbNoticia.NotId, TbNoticia.NotTitulo, TbNoticia.NotTexto, TbNoticia.NotData, TbNoticia.CatId, TbCategoria.CatNome
FROM            TbNoticia INNER JOIN
                         TbCategoria ON TbNoticia.CatId = TbCategoria.CatId
GO


CREATE TABLE [TbLogCategoria](
	[LogId] [int] IDENTITY(1,1) NOT NULL,
	[LogNomeOriginal] [varchar](50) NOT NULL,
	[LogNomeNovo] [varchar](50) NOT NULL,
	[CatId] [int] NOT NULL,
 CONSTRAINT [PK_TbLogCategoria] PRIMARY KEY CLUSTERED 
(
	[LogId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO



CREATE PROCEDURE ProcBuscarNoticias
	@NomeCategoria varchar(50)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT N.*, C.CatNome FROM TbNoticia N
	INNER JOIN TbCategoria C ON C.CatId = N.CatId
	WHERE UPPER(C.CatNome) LIKE UPPER('%' + @NomeCategoria + '%');

	RETURN;
END
GO