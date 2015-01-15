USE SistemaDeCompras;

--Retorna o ID do usuario que possui aquele token
--Retorna -2000 caso não haja usuarios com este ID
CREATE PROCEDURE usp_checarTokenUsuario
	@token varchar(50) output
AS
BEGIN
	DECLARE @idUsuario varchar(50)
	DECLARE @tokenExistente varchar(50)
	SET @tokenExistente = (SELECT COUNT(*) FROM tb_Usuario WHERE token = @token);
	IF(@tokenExistente  > 0) BEGIN
		SET @idUsuario = (SELECT id_usuario FROM tb_Usuario WHERE token = @token);
	END
	ELSE BEGIN
		SET @idUsuario = -2000
	END
	RETURN @idUsuario
END



--Retorna o ID do usuario criado
--Caso não cadastre o usuario, retornará -2000
CREATE PROCEDURE usp_cadastrarUsuario
	@nomeUsuario varchar(50) output,
	@email varchar(50) output,
	@senha varchar(50) output,
	@token varchar(50) output
AS
BEGIN
	DECLARE @testarEmail int
	SET @testarEmail = (SELECT COUNT(*) FROM tb_usuario WHERE email = @email);

	IF(@testarEmail = 1) BEGIN --email ja cadastrado
		SET @testarEmail = -2000
	END
	ELSE BEGIN --email nao cadastrado
		UPDATE tb_Usuario SET token = null WHERE token = @token;
		INSERT INTO tb_Usuario VALUES (@nomeUsuario,@email,@senha,@token);
		SET @testarEmail = (SELECT IDENT_CURRENT('tb_Usuario'));
	END
	RETURN @testarEmail
END



--Verificação com o token e email para pular tela login
CREATE PROCEDURE usp_verificarLogin
	@email varchar(50) output,
	@token varchar(50) output
AS
BEGIN
	DECLARE @usuarioLogado int
	SET @usuarioLogado = (SELECT COUNT(*) FROM tb_Usuario WHERE email = @email AND token = @token);
	IF(@usuarioLogado = 1) BEGIN --usuario esta logado
		RETURN 1
	END
	RETURN -2000
END


--Atualizar cadastro de usuario
CREATE PROCEDURE usp_atualizarSenhaUsuario
	@email varchar(50) output,
	@senha varchar(50) output,
	@novaSenha varchar(50) output

AS
BEGIN
	DECLARE @usuarioExistente int
	SET @usuarioExistente = (SELECT COUNT(*) FROM tb_Usuario WHERE email = @email AND senha = @senha);
	IF(@usuarioExistente = 1) BEGIN --usuario existente
		UPDATE tb_Usuario SET senha = @novaSenha WHERE email = @email;
		RETURN 1
	END
	RETURN -2000
END



--Logout
CREATE PROCEDURE usp_logout
	@email varchar(50) output
AS
BEGIN
	DECLARE @usuarioExistente int
	SET @usuarioExistente = (SELECT COUNT(*) FROM tb_Usuario WHERE email = @email);
	IF(@usuarioExistente = 1) BEGIN --usuario existente
		UPDATE tb_Usuario SET token = NULL WHERE email = @email;
		RETURN 1
	END
	RETURN -2000
END




--ALTERAR PRA FUNÇÃO!!!
--DEVERÁ RETORNAR OS DADOS DO USUARIO
CREATE PROCEDURE usp_fazerLogin
	@email varchar(50) output,
	@senha varchar(50) output,
	@senhaRecuperada varchar(50) output,
	@token varchar(50) output
AS
BEGIN
	DECLARE @testarLogin int
	SET @testarLogin = (SELECT COUNT(*) FROM tb_Usuario WHERE email = @email AND (senha = @senha OR 
	(SELECT SUBSTRING(senha,1,6) FROM tb_Usuario WHERE email = @email) = @senhaRecuperada));

	IF(@testarLogin = 1) BEGIN --usuario esta cadastrado
		UPDATE tb_Usuario SET token = null WHERE token = @token;
		UPDATE tb_Usuario SET token = @token WHERE email = @email AND (senha = @senha OR 
		(SELECT SUBSTRING(senha,1,6) FROM tb_Usuario WHERE email = @email) = @senhaRecuperada);
		SET @testarLogin = (SELECT IDENT_CURRENT('tb_Usuario'));
	END
	ELSE BEGIN -- usuario nao cadastrado
		SET @testarLogin = -2000
	END
	RETURN @testarLogin
END