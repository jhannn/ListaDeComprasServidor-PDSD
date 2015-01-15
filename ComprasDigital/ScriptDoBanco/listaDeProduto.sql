USE SistemaDeCompras;

--Retorna o ID da lista criada
--Caso não crie a lista, retornará -2000
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
		SET @retorno = -2000
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
		SET @retorno = -2000
	END
	RETURN @retorno
END



CREATE PROCEDURE usp_criarProdutoDaLista
	@idLista int output,
	@idProduto int output,
	@quantidade int output
AS
BEGIN
	IF ((SELECT COUNT(*) FROM tb_ProdutoDaLista WHERE id_produto = @idProduto AND id_listaP = @idLista) != 1) BEGIN
		INSERT INTO tb_ProdutoDaLista VALUES (@idProduto, @idLista, @quantidade);
	END
	ELSE BEGIN
		UPDATE tb_produtoDaLista SET quantidade = @quantidade WHERE id_produto = @idProduto AND id_listaP = @idLista
	END
END



CREATE PROCEDURE usp_removerProdutoDaLista
	@idLista int output,
	@idProduto int output
AS
BEGIN
	DELETE FROM tb_ProdutosDaLista WHERE id_listaP = @idLista AND is_produto = @idProduto
END
