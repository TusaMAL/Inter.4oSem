

USE BDarea52
GO

CREATE TABLE pessoas
(
	id     int			not null primary key identity,    
	nome   varchar(50)  not null,
	email  varchar(60)  not null unique,
	senha  varchar(20)  not null,
	status int			check(status in (1,2,3))
)
GO

CREATE TABLE admins
(
	pessoa_id int		not null primary key references pessoas
)
GO

CREATE TABLE usuarios
(
	pessoa_id int			not null primary key references pessoas,
	nick	  varchar(20)	not null,
	sexo	  varchar(1),
	datanasc  date,
	descricao varchar(500),
	imagem	  varchar(200)
)
GO

CREATE TABLE jogos
(
	id		  int			not null primary key identity,
	nome	  varchar(100)	not null,
	descricao varchar(500),
	imagem	  varchar(200),
	admin_id  int			not null references admins
)
GO

CREATE TABLE grupos
(
	id		  int			not null primary key identity,
	nome	  varchar(30)	not null,
	descricao varchar(500),
	imagem	  varchar(200),
	jogo_id	  int			not null references jogos
)
GO

CREATE TABLE mensagens
(
	id		   int			 not null primary key identity,
	datahora   datetime		not null,
	texto	   varchar(350),
	usuario_id int			 not null references usuarios,
	grupo_id   int			 not null references grupos
)
GO

CREATE TABLE eventos
(
	id		  int			not null primary key identity,
	nome	  varchar(30)	not null,
	data	  date			not null,
	horario   time			not null,
	tipo	  int			check(tipo in (1,2)),
	cep			varchar(9),
	numero		varchar(50),
	logradouro	varchar(50),
	bairro		varchar(50),
	cidade		varchar(50),
	descricao	varchar(500),
	grupo_id  int			not null references grupos
)
GO

CREATE TABLE participantes
(
	grupo_id   int			not null references grupos,
	usuario_id int			not null references usuarios,
	status	   int			check(status in (0,1,2,3)),
	primary key( grupo_id, usuario_id)
)
GO

CREATE TABLE confirmados
(
	grupo_id   int			not null,
	usuario_id int			not null,
	evento_id  int			not null references eventos,
	status	   int			check(status in (0,1)),
	foreign key(grupo_id, usuario_id) references participantes,
	primary key(grupo_id, usuario_id, evento_id)
)
GO

------------------------------------------------------------------------------------------------------
-------------------------------------------PROCEDURES-------------------------------------------------


CREATE PROCEDURE cadAdm			---Procedure de cadastro de administrador
(
	@nome varchar(50), @email varchar(60), @senha varchar(20)
)
AS
BEGIN
	INSERT INTO pessoas	VALUES(@nome, @email, @senha, 2)
	INSERT INTO admins	VALUES(@@IDENTITY)
END
GO


CREATE PROCEDURE cadUser		---Procedure de cadastro de usuario
(
	@nome varchar(50), @email varchar(60), @senha varchar(20),
	@nick varchar(20), @datanasc date
)
AS
BEGIN
	INSERT INTO pessoas	 VALUES(@nome, @email, @senha, 1)
	INSERT INTO usuarios VALUES(@@IDENTITY, @nick, null, @datanasc, null, '/img/userpics/usrdefault.png')
END
GO


CREATE PROCEDURE cadMsg			---Procedure de criação de mensagem
(
	@texto varchar(350), @usuario_id int, @grupo_id int
)
AS
BEGIN
	INSERT INTO mensagens VALUES(GETDATE(), @texto, @usuario_id, @grupo_id) 
END
GO


CREATE PROCEDURE partGrupo		--Procedure usada no método PartGrupo em GrupoModel.cs
(
	@grupo_id int, @usuario_id int
)
AS
BEGIN
	INSERT INTO participantes VALUES(@grupo_id, @usuario_id, 1)
END
GO


CREATE PROCEDURE sairGrupo		--Procedure usada no método SairGrupo em GrupoModel.cs
(
	@grupo_id int, @usuario_id int
)
AS
BEGIN
	SELECT * FROM grupos g WHERE g.id = @grupo_id
	IF EXISTS (SELECT * FROM participantes p WHERE p.usuario_id = @usuario_id)
	UPDATE	participantes
	SET status = 0
	WHERE usuario_id = @usuario_id AND grupo_id = @grupo_id
END
GO


CREATE PROCEDURE voltarGrupo	--Procedure usada no método VoltarGrupo em GrupoModel.cs
(
	@grupo_id int, @usuario_id int
)
AS
BEGIN
	SELECT * FROM grupos g WHERE g.id = @grupo_id
	IF EXISTS (SELECT * FROM participantes p WHERE p.usuario_id = @usuario_id)
	UPDATE	participantes
	SET status = 1
	WHERE usuario_id = @usuario_id AND grupo_id = @grupo_id
END
GO

CREATE PROCEDURE editarGrupo		--Procedure usada no método EditarGrupo em GrupoModel.cs
(
	@grupo_id int, @usuario_id int, @descricao varchar(500)
)
AS
BEGIN
	SELECT * FROM grupos g 
	INNER JOIN participantes p ON p.grupo_id = @grupo_id
	WHERE p.status = 2 AND p.usuario_id = @usuario_id
	UPDATE	grupos
	SET descricao = @descricao
	WHERE id = @grupo_id
END
GO


CREATE PROCEDURE partEvento		-- Procedure utilizada no método PartEvento em EventoModel.cs
(
	@grupo_id int, @usuario_id int, @evento_id int
)
AS
BEGIN
	INSERT INTO confirmados VALUES(@grupo_id, @usuario_id, @evento_id, 1)
END
GO



CREATE PROCEDURE sairEvento		-- Procedure utilizada no método SairEvento em EventoModel.cs
(
	@grupo_id int, @usuario_id int, @evento_id int
)
AS
BEGIN
	SELECT * FROM eventos e WHERE e.id = @evento_id
	IF EXISTS (SELECT * FROM confirmados c WHERE c.grupo_id = @grupo_id AND c.usuario_id = @usuario_id AND c.evento_id = @evento_id)
	UPDATE	confirmados
	SET	 status = 0
	WHERE usuario_id = @usuario_id AND grupo_id = @grupo_id AND evento_id = @evento_id
END
GO


CREATE PROCEDURE partEventoUpdate	-- Procedure utilizada no método PartEventoUpdate em EventoModel.cs
(
	@grupo_id int, @usuario_id int, @evento_id int
)
AS
BEGIN
	SELECT * FROM eventos e WHERE e.id = @evento_id
	IF EXISTS (SELECT * FROM confirmados c WHERE c.grupo_id = @grupo_id AND c.usuario_id = @usuario_id AND c.evento_id = @evento_id)
	UPDATE	confirmados
	SET	 status = 1
	WHERE usuario_id = @usuario_id AND grupo_id = @grupo_id AND evento_id = @evento_id 
END
GO

CREATE PROCEDURE editarEvento		--Procedure utilizada no método EditInfoEvento em EventoModel.cs
(
	@grupo_id int, @id int, @nome varchar(30), @data date, @horario time, @tipo int, @cep varchar(9), @numero varchar(50), 
	@logradouro varchar(50), @bairro varchar(50), @cidade varchar(50),  @descricao varchar(500)
)
AS
BEGIN
	SELECT * FROM eventos e WHERE e.id = @id
	IF EXISTS (SELECT * FROM eventos e WHERE e.id = @id)
	UPDATE	eventos
	SET nome		=		@nome,
		data		=		@data,
		horario		=		@horario,
		tipo		=		@tipo,
		cep			=		@cep,
		numero		=		@numero,
		logradouro	=		@logradouro,
		bairro		=		@bairro,
		cidade		=		@cidade,
		descricao	=		@descricao
	WHERE id = @id AND grupo_id = @grupo_id
END
GO



CREATE PROCEDURE cadJogo			-- Procedure de cadastro de jogo
(
	@nome varchar(100), @descricao varchar(500), @imagem varchar(200), @idAdm int
)
AS
BEGIN
	INSERT INTO jogos VALUES(@nome, @descricao, @imagem, @idAdm)
END
GO

CREATE PROCEDURE editJogo		-- Procedure que está sendo utilizada no metodo EditJogo no JogoModel.cs
(	
	@IdJogo int, @nome varchar(100), @descricao varchar(500)
)
AS
BEGIN
	SELECT * FROM jogos j WHERE j.id = @IdJogo
	IF EXISTS (SELECT * FROM jogos j WHERE j.id = @IdJogo)
	UPDATE	jogos
	SET nome		= @nome,
		descricao	= @descricao
	WHERE id = @IdJogo
END
GO


CREATE PROCEDURE cadGrupo			-- Procedure de cadastro de grupo
(
	@nome varchar(30), @descricao varchar(500), @imagem varchar(200), @jogo int,
	@usuario int
)
AS
BEGIN
	INSERT INTO grupos VALUES(@nome, @descricao, @imagem, @jogo)
	INSERT INTO participantes VALUES(@@IDENTITY, @usuario, 2)
END
GO


CREATE PROCEDURE cadPart			--Procedure de participacao em um grupo
(
	@grupo int, @usuario varchar(500)
)
AS
BEGIN
	INSERT INTO participantes VALUES(@grupo, @usuario, 1)
END
GO

CREATE PROCEDURE editInfo_User		-- Procedure que está sendo utilizada no metodo EditUsuario no UsuarioModel.cs
(	
	@IdUsuario int, @nome varchar(50), @nick varchar(20), @sexo varchar(1), @datanasc date, @descricao varchar(500)
)
AS
BEGIN
	SELECT * FROM usuarios u WHERE u.pessoa_id = @IdUsuario
	IF EXISTS (SELECT * FROM usuarios WHERE pessoa_id = @IdUsuario)
	UPDATE	usuarios 
	SET nick		= @nick,
		sexo		= @sexo,
		datanasc	= @datanasc,
		descricao	= @descricao
	WHERE pessoa_id = @IdUsuario
	IF EXISTS (SELECT * FROM pessoas WHERE id = @IdUsuario)
	UPDATE	pessoas
	SET nome		= @nome
	WHERE id = @IdUsuario  
END
GO

CREATE PROCEDURE editPwd			-- Procedure para mudar a senha, que está no método ChangePwd em UsuarioModel.cs
(
	@IdUsuario int, @newpwd varchar(20)
)
AS
BEGIN
	SELECT * FROM pessoas p WHERE p.id = @IdUsuario
	IF EXISTS (SELECT * FROM pessoas WHERE id = @IdUsuario)
	UPDATE	pessoas 
	SET senha	= @newpwd
	WHERE id	= @IdUsuario
END
GO


CREATE PROCEDURE cadEvento			--Procedure utilizada no método Create em EventoModel.cs
(
	@nome varchar(30), @data date, @horario time, @tipo int, 
	@cep varchar(9), @numero varchar(50), @logradouro varchar(50), 
	@bairro varchar(50), @cidade varchar (50), @descricao varchar(500), @grupo_id int
)
AS
BEGIN
	INSERT INTO eventos VALUES (@nome, @data, @horario, @tipo, @cep, @numero, @logradouro, @bairro, @cidade, @descricao, @grupo_id)
END
GO

------------------------------------------------------------------------------------------------------
---------------------------------------------VIEWS----------------------------------------------------


CREATE VIEW v_Conf_Evento_Grupo		-- View utilizada no método ViewConfUserEvento em EventoModel.cs
AS
SELECT	c.grupo_id		ConfGrupoId,
		c.usuario_id	ConfUserId,
		c.evento_id		ConfEventoId,
		c.status		ConfStatus,
		u.nick			UserNick,
		u.imagem		UserImg

FROM confirmados c
INNER JOIN usuarios u	ON	c.usuario_id	=	u.pessoa_id
INNER JOIN grupos	g	ON	c.grupo_id		=	g.id
INNER JOIN eventos	e	ON	c.evento_id		=	e.id
GO


CREATE VIEW v_Jogos					-- View de exibição dos jogos
AS
    SELECT	j.id		JogoID, 
			j.nome		JogoNome, 
			j.descricao	JogoDesc, 
			j.imagem	JogoImg
    FROM   Jogos j
GO


CREATE VIEW v_Grupo_Msg				-- View Utilizada no Método ReadMensagem, dentro de MensagemModel.cs
AS
SELECT		m.grupo_id,
			m.id				MsgId,
			m.datahora			Datahora,
			m.texto				Texto,
			u.pessoa_id			Idusuario,
			u.nick				Nickusuario,
			u.imagem			Imagemusuario,
			g.id				Idgrupo,
			g.nome				Nomegrupo,
			p.status			PartStatus
	FROM	mensagens m
	INNER JOIN	usuarios		u	ON	u.pessoa_id = m.usuario_id
	INNER JOIN	grupos			g	ON	g.id = m.grupo_id
	INNER JOIN	participantes	p	ON	m.usuario_id = p.usuario_id AND m.grupo_id = p.grupo_id
GO


CREATE VIEW v_User_Info_Edit		-- View Utilizada no Método ReadEditUsuario, dentro de UsuarioModel.cs
AS
	SELECT	p.id				PessoaId,
			p.nome				PessoaNome,
			p.email				PessoaEmail,
			u.pessoa_id			UserId,
			u.nick				Nick,
			u.datanasc			Datanasc,
			u.sexo				Sexo,
			u.imagem			Imagem,
			u.descricao			Descricao
	FROM	usuarios u
	INNER JOIN pessoas p ON u.pessoa_id = p.id
GO


CREATE VIEW v_Grupo_Msg_Part		-- View utilizada no método ReadMensagemIndex dentro de MensagemModel.cs
AS
	SELECT	m.grupo_id			GrupoDaMsg,
			m.datahora			Datahora,
			m.texto				Texto,
			u.pessoa_id			Idusuario,
			u.nick				Nickusuario,
			u.imagem			Imagemusuario,
			g.id				Idgrupo,
			g.nome				Nomegrupo,
			p.grupo_id			PartIdGrupo,
			p.usuario_id		PartIdUser,
			p.status			PartStatus
	FROM		mensagens		m
	INNER JOIN	usuarios		u	ON	u.pessoa_id = m.usuario_id
	INNER JOIN	grupos			g	ON	g.id = m.grupo_id
	INNER JOIN	participantes	p	ON	p.usuario_id = u.pessoa_id OR p.grupo_id = g.id
GO


CREATE VIEW v_User_Info				--View utilizada no método ReadU em UsuarioModel.cs
AS
	SELECT  p.id		ID,
			p.nome		Nome,
			u.pessoa_id PessoaId,
			u.nick		Nick,
			u.sexo		Sexo,
			u.datanasc	Datanasc,
			u.descricao Descricao,
			u.imagem	Imagem
	FROM usuarios u
	INNER JOIN pessoas p ON p.id = u.pessoa_id
	WHERE p.id = u.pessoa_id
GO


CREATE VIEW v_Grupo_Part			--View utilizada no método ReadGrupo em GrupoModel.cs
AS
	SELECT  p.usuario_id	ID,
			p.status		PartStatus,
			g.id			IdGrupo,
			g.nome			Nome,
			g.imagem		Imagem,
			j.nome			NomeJogo

	FROM participantes p
	INNER JOIN grupos	g ON 	p.grupo_id = g.id
	INNER JOIN jogos	j ON	j.id = g.jogo_id
GO


CREATE VIEW v_User_Grupo_Part		-- View utilizada no Método ReadMembrosGrupoTotal e TOP 6 ReadPartGrupo em GrupoModel.cs
AS
	SELECT		p.grupo_id		PartIdGrupo,
				p.usuario_id	PartIdUser,
				p.status		PartStatus,
				u.nick			Nick,
				u.imagem		Imagem,
				ps.nome			Nome

	FROM		usuarios		u
	INNER JOIN	participantes	p	ON	u.pessoa_id		=	p.usuario_id
	INNER JOIN	grupos			g	ON 	p.grupo_id		=	g.id
	INNER JOIN	pessoas			ps	ON	ps.id			=	u.pessoa_id
GO


CREATE VIEW v_Info_Grupo			--View utilizada no método InfoGrupo e BuscarGrupo em Grupomodel.cs
AS
	SELECT		g.id			IdGrupo,
				g.jogo_id		IdJogo,
				g.nome			NomeGrupo,
				g.imagem		ImagemGrupo,
				g.descricao		Descricao,
				j.nome			NomeJogo
	FROM		grupos			g
	INNER JOIN	jogos			j	ON j.id = g.jogo_id	
GO


CREATE VIEW v_Event_Grupo			--View Utilizada no método ViewEventos em EventoModel.cs
AS
	SELECT		g.id			IdGrupo,
				e.grupo_id		EventoIdGrupo,
				e.id			IdEvento,
				e.nome			NomeEvento,
				e.data			DataEvento,
				e.horario		HoraEvento,
				e.tipo			TipoEvento
	FROM		eventos			e, grupos			g
GO


CREATE VIEW v_Event_Index			--View Utilizada no método ReadEvento em EventoModel.cs
AS
	SELECT		g.id			IdGrupo,
				e.grupo_id		EventoIdGrupo,
				e.id			IdEvento,
				e.nome			NomeEvento,
				e.data			DataEvento,
				e.horario		HoraEvento,
				e.tipo			TipoEvento,
				e.descricao		DescricaoEvento,
				e.cep			CepEvento,
				e.numero		NrEvento,
				e.logradouro	LogEvento,
				e.bairro		BairroEvento,
				e.cidade		CidadeEvento
	FROM		eventos			e, grupos			g
GO
