/****** CREATES ******/

CREATE TABLE [dbo].[ComentarioTarefa](
	[ComentarioTarefaID] [int] IDENTITY(1,1) NOT NULL,
	[Observacao] [text] NOT NULL,
	[TarefaID] [int] NOT NULL,
	[UsuarioID] [int] NOT NULL,
 CONSTRAINT [PK_ComentarioTarefa] PRIMARY KEY CLUSTERED 
(
	[ComentarioTarefaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[Funcao](
	[FuncaoID] [int] IDENTITY(1,1) NOT NULL,
	[Descricao] [varchar](150) NOT NULL,
 CONSTRAINT [PK_Funcao] PRIMARY KEY CLUSTERED 
(
	[FuncaoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[LogTarefa](
	[LogID] [int] IDENTITY(1,1) NOT NULL,
	[Descricao] [varchar](500) NOT NULL,
	[DataAlteracao] [datetime] NOT NULL,
	[UsuarioID] [int] NOT NULL,
	[Chave] [varchar](50) NULL,
 CONSTRAINT [PK_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Prioridade](
	[PrioridadeID] [int] IDENTITY(1,1) NOT NULL,
	[Descricao] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Prioridade] PRIMARY KEY CLUSTERED 
(
	[PrioridadeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Projeto](
	[ProjetoID] [int] IDENTITY(1,1) NOT NULL,
	[Descricao] [varchar](250) NULL,
	[UsuarioID] [int] NULL,
 CONSTRAINT [PK_Projeto] PRIMARY KEY CLUSTERED 
(
	[ProjetoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Status](
	[StatusID] [int] IDENTITY(1,1) NOT NULL,
	[Descricao] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Status] PRIMARY KEY CLUSTERED 
(
	[StatusID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Tarefa](
	[TarefaID] [int] IDENTITY(1,1) NOT NULL,
	[Titulo] [varchar](150) NULL,
	[Descricao] [varchar](250) NULL,
	[DataVencimento] [datetime] NULL,
	[StatusID] [int] NULL,
	[ProjetoID] [int] NULL,
	[PrioridadeID] [int] NULL,
 CONSTRAINT [PK_Tarefa] PRIMARY KEY CLUSTERED 
(
	[TarefaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Usuario](
	[UsuarioID] [int] IDENTITY(1,1) NOT NULL,
	[Nome] [varchar](150) NOT NULL,
	[Login] [varchar](50) NULL,
	[FuncaoID] [int] NULL,
 CONSTRAINT [PK_Usuario] PRIMARY KEY CLUSTERED 
(
	[UsuarioID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** INSERT ******/
INSERT INTO [dbo].[Prioridade] ([Descricao]) VALUES ('Baixa')
GO
INSERT INTO [dbo].[Prioridade] ([Descricao]) VALUES ('Média')
GO
INSERT INTO [dbo].[Prioridade] ([Descricao]) VALUES ('Alta')
GO

INSERT INTO [dbo].[Status] ([Descricao]) VALUES ('Pendente')
GO
INSERT INTO [dbo].[Status] ([Descricao]) VALUES ('Em Andamento')
GO
INSERT INTO [dbo].[Status] ([Descricao]) VALUES ('Concluida')