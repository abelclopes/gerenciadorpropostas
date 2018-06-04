-- Drop table

-- DROP TABLE gerenciadorPropostas.dbo.Categorias

CREATE TABLE gerenciadorPropostas.dbo.Categorias (
	Id uniqueidentifier NOT NULL,
	DataAtualizacao datetime2,
	DataCriacao datetime2 NOT NULL,
	Descricao nvarchar(1073741823),
	Excluido bit NOT NULL,
	Nome nvarchar(1073741823),
	CONSTRAINT PK_Categorias PRIMARY KEY (Id)
) go;

-- Drop table

-- DROP TABLE gerenciadorPropostas.dbo.Fornecedores

CREATE TABLE gerenciadorPropostas.dbo.Fornecedores (
	Id uniqueidentifier NOT NULL,
	CnpjCpf nvarchar(450),
	DataAtualizacao datetime2,
	DataCriacao datetime2 NOT NULL,
	Email nvarchar(1073741823),
	Excluido bit NOT NULL,
	Nome nvarchar(1073741823),
	Telefone nvarchar(1073741823),
	CONSTRAINT PK_Fornecedores PRIMARY KEY (Id)
) go
CREATE UNIQUE INDEX IX_Fornecedores_CnpjCpf ON gerenciadorPropostas.dbo.Fornecedores (CnpjCpf) go;

-- Drop table

-- DROP TABLE gerenciadorPropostas.dbo.Permissoes

CREATE TABLE gerenciadorPropostas.dbo.Permissoes (
	Id uniqueidentifier NOT NULL,
	DataAtualizacao datetime2,
	DataCriacao datetime2 NOT NULL,
	Excluido bit NOT NULL,
	Nivel int NOT NULL,
	Nome nvarchar(1073741823),
	CONSTRAINT PK_Permissoes PRIMARY KEY (Id)
) go;

-- Drop table

-- DROP TABLE gerenciadorPropostas.dbo.PropostaAnexos

CREATE TABLE gerenciadorPropostas.dbo.PropostaAnexos (
	Id uniqueidentifier NOT NULL,
	ContentType nvarchar(1073741823),
	DataAtualizacao datetime2,
	DataCriacao datetime2 NOT NULL,
	Excluido bit NOT NULL,
	FileContent varbinary,
	Nome nvarchar(1073741823),
	PropostaId uniqueidentifier,
	CONSTRAINT PK_PropostaAnexos PRIMARY KEY (Id),
	CONSTRAINT FK_PropostaAnexos_Propostas_PropostaId FOREIGN KEY (PropostaId) REFERENCES gerenciadorPropostas.dbo.Propostas(Id) ON DELETE RESTRICT ON UPDATE RESTRICT
) go
CREATE INDEX IX_PropostaAnexos_PropostaId ON gerenciadorPropostas.dbo.PropostaAnexos (PropostaId) go;

-- Drop table

-- DROP TABLE gerenciadorPropostas.dbo.Propostas

CREATE TABLE gerenciadorPropostas.dbo.Propostas (
	Id uniqueidentifier NOT NULL,
	CategoriaId uniqueidentifier NOT NULL,
	DataAtualizacao datetime2,
	DataCriacao datetime2 NOT NULL,
	Descricao nvarchar(500),
	Excluido bit NOT NULL,
	FornecedorId uniqueidentifier NOT NULL,
	NomeProposta nvarchar(1073741823),
	Status int NOT NULL,
	Valor nvarchar(1073741823),
	CONSTRAINT PK_Propostas PRIMARY KEY (Id),
	CONSTRAINT FK_Propostas_Categorias_CategoriaId FOREIGN KEY (CategoriaId) REFERENCES gerenciadorPropostas.dbo.Categorias(Id) ON DELETE CASCADE ON UPDATE RESTRICT,
	CONSTRAINT FK_Propostas_Fornecedores_FornecedorId FOREIGN KEY (FornecedorId) REFERENCES gerenciadorPropostas.dbo.Fornecedores(Id) ON DELETE CASCADE ON UPDATE RESTRICT
) go
CREATE INDEX IX_Propostas_CategoriaId ON gerenciadorPropostas.dbo.Propostas (CategoriaId) go
CREATE INDEX IX_Propostas_FornecedorId ON gerenciadorPropostas.dbo.Propostas (FornecedorId) go;

-- Drop table

-- DROP TABLE gerenciadorPropostas.dbo.PropostasHistoricos

CREATE TABLE gerenciadorPropostas.dbo.PropostasHistoricos (
	Id uniqueidentifier NOT NULL,
	DataAtualizacao datetime2,
	DataCriacao datetime2 NOT NULL,
	Excluido bit NOT NULL,
	PropostaId uniqueidentifier NOT NULL,
	PropostaStatus int NOT NULL,
	UsuarioId uniqueidentifier NOT NULL,
	CONSTRAINT PK_PropostasHistoricos PRIMARY KEY (Id),
	CONSTRAINT FK_PropostasHistoricos_Propostas_PropostaId FOREIGN KEY (PropostaId) REFERENCES gerenciadorPropostas.dbo.Propostas(Id) ON DELETE CASCADE ON UPDATE RESTRICT
) go
CREATE INDEX IX_PropostasHistoricos_PropostaId ON gerenciadorPropostas.dbo.PropostasHistoricos (PropostaId) go
CREATE INDEX IX_PropostasHistoricos_UsuarioId ON gerenciadorPropostas.dbo.PropostasHistoricos (UsuarioId) go;

-- Drop table

-- DROP TABLE gerenciadorPropostas.dbo.UsuarioPermissoes

CREATE TABLE gerenciadorPropostas.dbo.UsuarioPermissoes (
	UsuarioId uniqueidentifier NOT NULL,
	PermissaoId uniqueidentifier NOT NULL,
	CONSTRAINT PK_UsuarioPermissoes PRIMARY KEY (UsuarioId),
	CONSTRAINT FK_UsuarioPermissoes_Permissoes FOREIGN KEY (PermissaoId) REFERENCES gerenciadorPropostas.dbo.Permissoes(Id) ON DELETE RESTRICT ON UPDATE RESTRICT,
	CONSTRAINT FK_UsuarioPermissoes_Usuarios FOREIGN KEY (UsuarioId) REFERENCES gerenciadorPropostas.dbo.Usuarios(Id) ON DELETE RESTRICT ON UPDATE RESTRICT
) go
CREATE INDEX IX_UsuarioPermissoes_PermissaoId ON gerenciadorPropostas.dbo.UsuarioPermissoes (PermissaoId) go;

-- Drop table

-- DROP TABLE gerenciadorPropostas.dbo.Usuarios

CREATE TABLE gerenciadorPropostas.dbo.Usuarios (
	Id uniqueidentifier NOT NULL,
	Cpf nvarchar(18) NOT NULL,
	DataAtualizacao datetime2,
	DataCriacao datetime2 NOT NULL,
	DataNacimento datetime2 NOT NULL,
	Email nvarchar(1073741823) NOT NULL,
	Excluido bit NOT NULL,
	Nome nvarchar(1073741823) NOT NULL,
	Senha nvarchar(1073741823) NOT NULL,
	CONSTRAINT PK_Usuarios PRIMARY KEY (Id)
) go;
