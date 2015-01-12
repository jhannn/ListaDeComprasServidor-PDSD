CREATE DATABASE SistemaDeCompras;
USE SistemaDeCompras;

CREATE TABLE tb_Estabelecimento
(
	id_estabelecimento INT PRIMARY KEY IDENTITY(1,1),
	nome VARCHAR(50) NOT NULL,
	bairro VARCHAR(100) NOT NULL,
	cidade VARCHAR(100) NOT NULL,
	numero INT NOT NULL
);

CREATE TABLE tb_Usuario
(
	id_usuario INT PRIMARY KEY IDENTITY(1,1),
	nome VARCHAR(50) NOT NULL,
	email VARCHAR(50) NOT NULL,
	senha VARCHAR(100) NOT NULL,
	token VARCHAR(50) NULL
);

CREATE TABLE tb_Produto
(
	id_produto INT PRIMARY KEY IDENTITY(1,1),
	nome VARCHAR(50) NOT NULL, 
	codigoDeBarras VARCHAR(50),
	tipoCodigoDeBarras VARCHAR(50)
);

CREATE TABLE tb_Item
(
	id_item INT PRIMARY KEY IDENTITY(1,1),
	preco FLOAT NOT NULL,
	qualificacao INT NOT NULL,
	compraMaisRecente DATE NOT NULL,
	id_estabelecimento INT FOREIGN KEY REFERENCES tb_Estabelecimento(id_estabelecimento) NOT NULL,
	id_produto INT FOREIGN KEY REFERENCES tb_Produto(id_produto) NOT NULL
);

CREATE TABLE tb_ListaDeItens
(
	id_listaDeItens INT PRIMARY KEY IDENTITY(1,1),
	dataDeCompras DATE NOT NULL,
	id_usuario INT FOREIGN KEY REFERENCES tb_Usuario(id_usuario) NOT NULL
);

CREATE TABLE tb_ListaDeProdutos
(
	id_listaDeProdutos INT PRIMARY KEY IDENTITY(1,1),
	nome VARCHAR(50),
	id_usuario INT FOREIGN KEY REFERENCES tb_Usuario(id_usuario) NOT NULL
);

CREATE TABLE tb_ItemDaLista
(
	id_item INT FOREIGN KEY REFERENCES tb_Item(id_item) NOT NULL,
	id_listaI INT FOREIGN KEY REFERENCES tb_ListaDeItens(id_listaDeItens) NOT NULL,
	quantidade INT NOT NULL
);

CREATE TABLE tb_ProdutoDaLista
(
	id_produto INT FOREIGN KEY REFERENCES tb_Produto(id_produto) NOT NULL,
	id_listaP INT FOREIGN KEY REFERENCES tb_ListaDeProdutos(id_listadeProdutos) NOT NULL,
	quantidade INT NOT NULL
);

CREATE TABLE tb_ProdutosInvalidos
(
	id_produto INT PRIMARY KEY IDENTITY(1,1),
	nome VARCHAR(50) NOT NULL, 
	codigoDeBarras VARCHAR(50),
	tipoCodigoDeBarras VARCHAR(50)
);



/*----------------- Stored Procedures -----------------*/



--Retorna o ID do usuario que possui aquele token
--Retorna -1 caso não haja usuarios com este ID
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
		SET @idUsuario = -1
	END
	RETURN @idUsuario
END



--Retorna o ID da lista criada
--Caso não crie a lista, retornará -1
CREATE PROCEDURE usp_criarListaDeCompras
	@nomeLista varchar(50) output,
	@idUsuario varchar(50) output
AS
BEGIN
	DECLARE @retorno int

	DECLARE @quantidadeListas int
	SET @quantidadeListas = (SELECT COUNT(*) FROM tb_ListaDeProdutos WHERE id_usuario = @idUsuario AND nome LIKE @nomeLista + '%');
	IF(@quantidadeListas >= 1)BEGIN
		SET @nomeLista += '-' + CAST((@quantidadeListas + 1) AS varchar(5))
	END
	INSERT INTO tb_ListaDeProdutos VALUES (@nomeLista, @idUsuario);
	SET @retorno = (SELECT IDENT_CURRENT('tb_ListaDeProdutos')); --Lista Criada
	--tem que retornar o ID da lista
	RETURN @retorno
END



--Excluir Lista de Compras
CREATE PROCEDURE usp_excluirListaDeCompras
	@idLista varchar(50) output,
	@idUsuario varchar(50) output
AS
BEGIN
	DECLARE @retorno int
	DECLARE @listaExistente int
	SET @listaExistente =  (SELECT COUNT(*)
							FROM tb_ListaDeProdutos AS LC
							WHERE  LC.id_usuario = @idUsuario
							AND LC.id_listaDeProdutos = @idLista
							);
	IF (@listaExistente > 0) BEGIN
		DELETE FROM tb_ProdutoDaLista WHERE id_listaP = @idLista;
		DELETE FROM tb_ListaDeProdutos WHERE id_listaDeProdutos = @idLista;
		SET @retorno = 1
	END
	ELSE BEGIN
		SET @retorno = -1
	END
	RETURN @retorno
END

----------------------Editar nome da Lista de compras------------------
CREATE PROCEDURE usp_editarNomeListaDeCompras
	@idLista varchar(50) output,
	@nome varchar(50) output,
	@idUsuario varchar(50) output
AS
BEGIN
	DECLARE @retorno int
	DECLARE @listaExistente int
	SET @listaExistente =  (SELECT COUNT(*)
							FROM tb_ListaDeProdutos AS LC
							WHERE LC.id_usuario = @idUsuario
							AND LC.id_listaDeProdutos = @idLista
							);
	IF (@listaExistente > 0) BEGIN
		UPDATE tb_ListaDeProdutos SET nome = @nome WHERE id_listaDeProdutos = @idLista;
		SET @retorno = @idLista
	END
	ELSE BEGIN
		SET @retorno = -1
	END
	RETURN @retorno
END

------------------------------------------------------------------------------------------------
CREATE PROCEDURE usp_criarProduto
	@idLista int output,
	@nomeProduto varchar(50) output,
	@codigoDeBarras varchar(50) output,
	@tipoCodigo varchar(50) output,
	@quantidade int output
AS
BEGIN
	IF((SELECT COUNT(*) FROM tb_ListaDeProdutos WHERE id_listaDeProdutos = @idLista) != 1) BEGIN
		RETURN -1 --Lista não existe
	END
	DECLARE @idProduto int
	IF (@codigoDeBarras IS NULL ) BEGIN
		--Produto sem código de barras
		IF((SELECT COUNT(*) FROM tb_Produto WHERE nome = @nomeProduto) < 1) BEGIN
			--Produto não existe
			INSERT INTO tb_Produto VALUES (@nomeProduto, NULL, NULL);
			SET @idProduto = (SELECT IDENT_CURRENT('tb_Produto'));
		END
		ELSE BEGIN
			--Produto existe
			SET @idProduto = (SELECT id_produto FROM tb_Produto WHERE nome = @nomeProduto);
		END
	END
	ELSE BEGIN --Produto com código de barras
		DECLARE @nomeProdutoConf VARCHAR (50)
		DECLARE @codigoProdutoConf VARCHAR(50)
		DECLARE @tipoProdutoConf VARCHAR(50)
		IF((SELECT COUNT(*) FROM tb_Produto WHERE codigoDeBarras = @codigoDeBarras AND nome != @nomeProduto) >= 1) BEGIN
			--Coloca os registros com codigos duplicados na tabela de produtos invalidos
				INSERT INTO tb_ProdutosInvalidos VALUES (@nomeProduto, @codigoDeBarras, @tipoCodigo);
				SET @nomeProdutoConf = (SELECT nome FROM tb_Produto WHERE codigoDeBarras = @codigoDeBarras);
				SET @codigoProdutoConf = (SELECT codigoDeBarras FROM tb_Produto WHERE codigoDeBarras = @codigoDeBarras);
				SET @tipoProdutoConf = (SELECT tipoCodigoDeBarras FROM tb_Produto WHERE codigoDeBarras = @codigoDeBarras);
				INSERT INTO tb_ProdutosInvalidos VALUES (@nomeProdutoConf, @codigoProdutoConf, @tipoProdutoConf);
			/*----- enviar mensagem de erro pro ADM -----*/
			SET @codigoDeBarras = ''
			SET @tipoCodigo = ''
			IF((SELECT COUNT(*) FROM tb_Produto WHERE nome = @nomeProduto) = 1) BEGIN
				--Novo produto ja existe
				SET @idProduto = (SELECT id_produto FROM tb_Produto WHERE nome = @nomeProduto);
			END
			ELSE BEGIN
				--Novo produto nao existe
				INSERT INTO tb_Produto VALUES (@nomeProduto, @codigoDeBarras, @tipoCodigo);
				SET @idProduto = (SELECT IDENT_CURRENT('tb_Produto'));
			END
		END
		ELSE BEGIN
			IF((SELECT COUNT(*) FROM tb_Produto WHERE nome = @nomeProduto) = 1) BEGIN
				--Produto ja existe
				IF ((SELECT COUNT(*) FROM tb_Produto WHERE codigoDeBarras != @codigoDeBarras AND nome = @nomeProduto) >= 1) BEGIN
					--Coloca os registros com codigos duplicados na tabela de produtos invalidos
					INSERT INTO tb_ProdutosInvalidos VALUES (@nomeProduto, @codigoDeBarras, @tipoCodigo);
					SET @nomeProdutoConf = (SELECT nome FROM tb_Produto WHERE nome = @nomeProduto);
					SET @codigoProdutoConf = (SELECT codigoDeBarras FROM tb_Produto WHERE nome = @nomeProduto);
					SET @tipoProdutoConf = (SELECT tipoCodigoDeBarras FROM tb_Produto WHERE nome = @nomeProduto);
					INSERT INTO tb_ProdutosInvalidos VALUES (@nomeProdutoConf, @codigoProdutoConf, @tipoProdutoConf);
					/*----- enviar mensagem de erro pro ADM -----*/
				END
				ELSE BEGIN
					UPDATE tb_Produto SET codigoDeBarras = @codigoDeBarras , tipoCodigoDeBarras = @tipoCodigo WHERE nome = @nomeProduto;
				END
				SET @idProduto = (SELECT id_produto FROM tb_Produto WHERE nome = @nomeProduto);
			END
			ELSE BEGIN
				--Produto nao existe
				INSERT INTO tb_Produto VALUES (@nomeProduto, @codigoDeBarras, @tipoCodigo);
				SET @idProduto = (SELECT IDENT_CURRENT('tb_Produto'));
			END
		END
	END
	EXEC usp_criarProdutoDaLista @idLista, @idProduto, @quantidade
	RETURN 1
END



CREATE PROCEDURE usp_criarProdutoDaLista
	@idLista int output,
	@idProduto int output,
	@quantidade int output
AS
BEGIN
	IF ((SELECT COUNT(*) FROM tb_ProdutoDaLista WHERE id_produt = @idProduto AND id_listaP = @idLista) != 1) BEGIN
		INSERT INTO tb_ProdutoDaLista VALUES (@idProduto, @idLista, @quantidade);
	END
	ELSE BEGIN
		UPDATE tb_produtoDaLista SET quantidade = @quantidade WHERE id_produt = @idProduto AND id_listaP = @idLista
	END
END



CREATE PROCEDURE usp_removerProdutoDaLista
	@idLista int output,
	@idProduto int output
AS
BEGIN
	DELETE FROM tb_ProdutosDaLista WHERE id_listaP = @idLista AND is_produto = @idProduto
END



--Retorna o ID do usuario criado
--Caso não cadastre o usuario, retornará -1
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
		SET @testarEmail = (SELECT IDENT_CURRENT('tb_Usuario'));
	END
	ELSE BEGIN --email nao cadastrado
		UPDATE tb_Usuario SET token = null WHERE token = @token;
		INSERT INTO tb_Usuario VALUES (@nomeUsuario,@email,@senha,@token);
		SET @testarEmail = -1
	END
	RETURN @testarEmail
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
		SET @testarLogin = -1
	END
	RETURN @testarLogin
END


--Verificação com o token e email para ppular tela login
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
	RETURN -1
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
	RETURN -1
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
	RETURN -1
END



USE SistemaDeCompras
SELECT * FROM tb_ListaDeProdutos;
SELECT * FROM tb_Usuario;
UPDATE tb_Usuario SET token = NULL WHERE token = '1245723423322';
DELETE FROM tb_ProdutoDaLista;
DELETE FROM tb_Produto;
DELETE FROM tb_ProdutosInvalidos;
SELECT * FROM tb_ListaDeProdutos
EXEC usp_editarNomeListaDeCompras 1, 'Sou eu', 1, '124576453875'
SELECT * FROM tb_Produto
SELECT * FROM tb_ProdutosInvalidos
EXEC usp_criarProduto 1, 'Biscoito maria', '1001', '01', 1
EXEC usp_criarProduto 1, 'Biscoito maria', NULL, '01', 1
EXEC usp_criarProduto 1, 'Todao', NULL, NULL, 1
EXEC usp_criarProduto 1, 'Todao', '0101', '01', 1
EXEC usp_criarProduto 1, 'Todao', '0011', '01', 1
EXEC usp_criarProduto 1, 'Feijao', '1001', '01', 1
EXEC usp_criarProduto 1, 'Biscoito maria', '0101', '01', 1
SELECT p.nome, p.id_produto, pl.quantidade FROM tb_Produto AS p INNER JOIN tb_ProdutoDaLista AS pl ON pl.id_listaP = 1 AND pl.id_produt = p.id_produto

/*
@idLista int output,
@nomeProduto varchar(50) output,
@codigoDeBarras varchar(50) output,
@tipoCodigo varchar(50) output,
@quantidade int output
*/







