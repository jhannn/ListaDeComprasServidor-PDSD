USE SistemaDeCompras;



--Cria um produto e retorna seu ID
CREATE PROCEDURE usp_criarProduto
	@nomeProduto varchar(50) output,
	@codigoDeBarras varchar(50) output,
	@tipoCodigo varchar(50) output,
	@marca varchar(50) output,
	@tipo int output,
	@unidade int output
AS
BEGIN
	DECLARE @idProduto int
	IF (@codigoDeBarras IS NULL ) BEGIN
	--Produto sem código de barras
		IF((SELECT COUNT(*) FROM tb_Produto WHERE nome = @nomeProduto AND marca LIKE '%' + @marca + '%') < 1)) BEGIN
		--Produto não existe
			INSERT INTO tb_Produto VALUES (@nomeProduto, NULL, NULL, @marca, @tipo, @unidade);
			SET @idProduto = (SELECT IDENT_CURRENT('tb_Produto'));
		END
		ELSE IF ((SELECT COUNT(*) FROM tb_Produto WHERE nome = @nomeProduto AND marca LIKE '%' + @marca + '%' AND tipo = @tipo) < 1)) BEGIN
		--Existe, mas com tipo diferente
			INSERT INTO tb_Produto VALUES (@nomeProduto, NULL, NULL, @marca, @tipo, @unidade);
			SET @idProduto = (SELECT IDENT_CURRENT('tb_Produto'));
			EXEC usp_adicionarProdutoInvalido CAST(INT,(SELECT TOP(1) id_produto FROM tb_Produto WHERE nome = @nomeProduto AND marca LIKE '%' + @marca + '%' AND tipo = @tipo)), @idProduto, 'Tipo diferente'
		END
		ELSE IF ((SELECT COUNT(*) FROM tb_Produto WHERE nome = @nomeProduto AND marca LIKE '%' + @marca + '%' AND tipo = @tipo AND unidade = @unidade) < 1)) BEGIN
		--Existe, mas com unidade diferente
			INSERT INTO tb_Produto VALUES (@nomeProduto, NULL, NULL, @marca, @tipo, @unidade);
			SET @idProduto = (SELECT IDENT_CURRENT('tb_Produto'));
			EXEC usp_adicionarProdutoInvalido CAST(INT,(SELECT TOP(1) id_produto FROM tb_Produto WHERE nome = @nomeProduto AND marca LIKE '%' + @marca + '%' AND tipo = @tipo AND unidade = @unidade)), @idProduto, 'Unidade diferente'
		END
		ELSE BEGIN
		--Produto ja existe
			SET @idProduto = (SELECT TOP(1) id_produto FROM tb_Produto WHERE nome = @nomeProduto AND marca LIKE '%' + @marca + '%' AND tipo = @tipo AND unidade = @unidade);
		END
	END
	ELSE BEGIN
	--Produto com código de barras
		IF ((SELECT COUNT(*) FROM tb_Produto WHERE codigoDeBarras = @codigoDeBarras) < 1) BEGIN
		--codigo de barras não existe
			IF((SELECT COUNT(*) FROM tb_Produto WHERE nome = @nomeProduto AND marca LIKE '%' + @marca + '%') < 1)) BEGIN
			--Produto não existe
				INSERT INTO tb_Produto VALUES (@nomeProduto, @codigoDeBarras, @tipoCodigo, @marca, @tipo, @unidade);
				SET @idProduto = (SELECT IDENT_CURRENT('tb_Produto'));
			END
			ELSE IF ((SELECT COUNT(*) FROM tb_Produto WHERE codigoDeBarras IS NOT NULL AND nome = @nomeProduto AND marca LIKE '%' + @marca + '%') < 1) BEGIN
			--existe, mas com codigo de barras diferente
				INSERT INTO tb_Produto VALUES (@nomeProduto, @codigoDeBarras, @tipoCodigo, @marca, @tipo, @unidade);
				SET @idProduto = (SELECT IDENT_CURRENT('tb_Produto'));
				EXEC usp_adicionarProdutoInvalido CAST(INT,(SELECT TOP(1) id_produto FROM tb_Produto WHERE @codigoDeBarras IS NOT NULL AND nome = @nomeProduto AND marca LIKE '%' + @marca + '%')), @idProduto, 'Codigo de barras diferente do existente'
			END
			ELSE IF ((SELECT COUNT(*) FROM tb_Produto WHERE nome = @nomeProduto AND marca LIKE '%' + @marca + '%' AND tipo = @tipo) < 1)) BEGIN
			--Existe, mas com tipo diferente
				INSERT INTO tb_Produto VALUES (@nomeProduto, @codigoDeBarras, @tipoCodigo, @marca, @tipo, @unidade);
				SET @idProduto = (SELECT IDENT_CURRENT('tb_Produto'));
				EXEC usp_adicionarProdutoInvalido CAST(INT,(SELECT TOP(1) id_produto FROM tb_Produto WHERE nome = @nomeProduto AND marca LIKE '%' + @marca + '%' AND tipo = @tipo)), @idProduto, 'Tipo diferente'
			END
			ELSE IF ((SELECT COUNT(*) FROM tb_Produto WHERE nome = @nomeProduto AND marca LIKE '%' + @marca + '%' AND tipo = @tipo AND unidade = @unidade) < 1)) BEGIN
			--Existe, mas com unidade diferente
				INSERT INTO tb_Produto VALUES (@nomeProduto, @codigoDeBarras, @tipoCodigo, @marca, @tipo, @unidade);
				SET @idProduto = (SELECT IDENT_CURRENT('tb_Produto'));
				EXEC usp_adicionarProdutoInvalido CAST(INT,(SELECT TOP(1) id_produto FROM tb_Produto WHERE nome = @nomeProduto AND marca LIKE '%' + @marca + '%' AND tipo = @tipo AND unidade = @unidade)), @idProduto, 'Unidade diferente'
			END
			ELSE BEGIN
			--Produto ja existe, mas sem código
				SET @idProduto = (SELECT TOP(1) id_produto FROM tb_Produto WHERE nome = @nomeProduto AND marca LIKE '%' + @marca + '%' AND tipo = @tipo AND unidade = @unidade);
				UPDATE tb_Produto SET codigoDeBarras = @codigoDeBarras , tipoCodigoDeBarras = @tipoCodigo WHERE id_produto = @idProduto;
			END
		END
		ELSE BEGIN
		--codigo de barras ja existe
			IF ((SELECT COUNT(*) FROM tb_Produto WHERE codigoDeBarras = @codigoDeBarras AND nome = @nomeProduto AND marca LIKE '%' + @marca + '%') = 1) BEGIN
			--produto ja existe com codigo de barras igual
				IF ((SELECT COUNT(*) FROM tb_Produto WHERE nome = @nomeProduto AND marca LIKE '%' + @marca + '%' AND tipo = @tipo) < 1)) BEGIN
					--Existe, mas com tipo diferente
					INSERT INTO tb_Produto VALUES (@nomeProduto, NULL, NULL, @marca, @tipo, @unidade);
					SET @idProduto = (SELECT IDENT_CURRENT('tb_Produto'));
					EXEC usp_adicionarProdutoInvalido CAST(INT,(SELECT TOP(1) id_produto FROM tb_Produto WHERE nome = @nomeProduto AND marca LIKE '%' + @marca + '%' AND tipo = @tipo)), @idProduto, 'Tipo diferente'
				END
				ELSE IF ((SELECT COUNT(*) FROM tb_Produto WHERE nome = @nomeProduto AND marca LIKE '%' + @marca + '%' AND tipo = @tipo AND unidade = @unidade) < 1)) BEGIN
					--Existe, mas com unidade diferente
					INSERT INTO tb_Produto VALUES (@nomeProduto, NULL, NULL, @marca, @tipo, @unidade);
					SET @idProduto = (SELECT IDENT_CURRENT('tb_Produto'));
					EXEC usp_adicionarProdutoInvalido (CAST(INT,(SELECT TOP(1) id_produto FROM tb_Produto WHERE nome = @nomeProduto AND marca LIKE '%' + @marca + '%' AND tipo = @tipo AND unidade = @unidade)), @idProduto, 'Unidade diferente'
				END
				ELSE BEGIN
					--Produto ja existe
					SET @idProduto = (SELECT TOP(1) id_produto FROM tb_Produto WHERE nome = @nomeProduto AND marca LIKE '%' + @marca + '%' AND tipo = @tipo AND unidade = @unidade);
				END
			END
			ELSE BEGIN
			--codigo de barras pertence a outro produto
				IF((SELECT COUNT(*) FROM tb_Produto WHERE nome = @nomeProduto AND marca LIKE '%' + @marca + '%') < 1)) BEGIN
				--Produto não existe
					INSERT INTO tb_Produto VALUES (@nomeProduto, NULL, NULL, @marca, @tipo, @unidade);
					SET @idProduto = (SELECT IDENT_CURRENT('tb_Produto'));
				END
				ELSE IF ((SELECT COUNT(*) FROM tb_Produto WHERE nome = @nomeProduto AND marca LIKE '%' + @marca + '%' AND tipo = @tipo) < 1)) BEGIN
				--Existe, mas com tipo diferente
					INSERT INTO tb_Produto VALUES (@nomeProduto, NULL, NULL, @marca, @tipo, @unidade);
					SET @idProduto = (SELECT IDENT_CURRENT('tb_Produto'));
				END
				ELSE IF ((SELECT COUNT(*) FROM tb_Produto WHERE nome = @nomeProduto AND marca LIKE '%' + @marca + '%' AND tipo = @tipo AND unidade = @unidade) < 1)) BEGIN
				--Existe, mas com unidade diferente
					INSERT INTO tb_Produto VALUES (@nomeProduto, NULL, NULL, @marca, @tipo, @unidade);
					SET @idProduto = (SELECT IDENT_CURRENT('tb_Produto'));
				END
				ELSE BEGIN
				--Produto ja existe
					SET @idProduto = (SELECT TOP(1) id_produto FROM tb_Produto WHERE nome = @nomeProduto AND marca LIKE '%' + @marca + '%' AND tipo = @tipo AND unidade = @unidade);
				END
				EXEC usp_adicionarProdutoInvalido CAST(INT,(SELECT TOP(1) id_produto FROM tb_Produto WHERE codigoDeBarras = @codigoDeBarras)), @idProduto, 'Codigo de barras ja existente'
			END
		END
	END
	RETURN @idProduto
END



--adiciona à lista de Produtos Inválidos
CREATE PROCEDURE usp_adicionarProdutoInvalido
	@produtoAntigo INT output,
	@produtoNovo INT output,
	@ocorrencia varchar(50) output
AS
BEGIN
	INSERT INTO tb_ProdutosInvalidos VALUES (@produtoAntigo, @produtoNovo CAST(INT, (SELECT id_ocorrencia FROM tb_Ocorrencia WHERE ocorrencia = @ocorrencia)));
END



--retorna uma lista de produtos de acordo com os parametros
/*CREATE PROCEDURE usp_pesquisarProduto
	@nomeProduto varchar(50) output,
	@codigoDeBarras varchar(50) output,
	@tipoCodigo varchar(50) output,
	@marca varchar(50) output,
	@tipo varchar(50) output,
	@unidade varchar(50) output
AS
BEGIN

END*/
