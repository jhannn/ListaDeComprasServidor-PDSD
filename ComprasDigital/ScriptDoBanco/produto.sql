USE SistemaDeCompras;

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
		RETURN -2000 --Lista não existe
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
