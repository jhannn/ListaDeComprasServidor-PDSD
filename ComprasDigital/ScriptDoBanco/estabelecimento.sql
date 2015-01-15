USE SistemaDeCompras;

--Retorna os produtos do estabelecimento E com nome contendo a string passada e a marca
CREATE PROCEDURE usp_produtosDoEstabelecimentoPorNome
	@idEstabelecimento varchar(50) output,
	@parteDoNome varchar(50) output,
	@marca varchar(50) output
AS
BEGIN
	RETURN (
			SELECT P.id_produto, P.nome, P.codigoDeBarras, P.tipoCodigoDeBarras, P.marca
			FROM tb_Item AS I
			INNER JOIN tb_Produto AS P
			INNER JOIN tb_Estabelecimento AS E
			ON	I.id_produto = P.id_produto
			AND	I.id_estabelecimento = E.id_estabelecimento
			AND P.marca = @marca
			AND P.nome LIKE '%' + @parteDoNome + '%'
			);
END


--Retorna os produtos do estabelecimento E com o código de barras passado
CREATE PROCEDURE usp_produtosDoEstabelecimentoPorNome
	@idEstabelecimento varchar(50) output,
	@codigo varchar(50) output
AS
BEGIN
	RETURN (
			SELECT P.id_produto, P.nome, P.codigoDeBarras, P.tipoCodigoDeBarras, P.marca
			FROM tb_Item AS I
			INNER JOIN tb_Produto AS P
			INNER JOIN tb_Estabelecimento AS E
			ON	I.id_produto = P.id_produto
			AND	I.id_estabelecimento = E.id_estabelecimento
			AND P.codigoDeBarras = @codigo
			);
END
