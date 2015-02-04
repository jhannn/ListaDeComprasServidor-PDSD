USE SistemaDeCompras;


--Usuario
SELECT id_usuario AS id, token, nome, email, senha
FROM tb_Usuario;

--Listas dos usuarios
SELECT u.id_usuario AS idUsuario, u.nome AS nomeUsuario, l.id_listaDeProdutos AS idLista, l.nome AS nomeLista
FROM tb_ListaDeProdutos AS l
INNER JOIN tb_Usuario AS u ON u.id_usuario = l.id_usuario;

--Produtos de alguma lista
DECLARE @idLista INT;
SET @idLista = 2;
SELECT pdl.quantidade, m.marca, p.nome, e.embalagem, u.unidade
FROM tb_ProdutoDaLista AS pdl
INNER JOIN tb_ListaDeProdutos AS l ON l.id_listaDeProdutos = @idLista
									AND l.id_listaDeProdutos = pdl.id_lista
INNER JOIN tb_Produto AS p ON p. id_produto = pdl.id_produto
INNER JOIN tb_Marca AS m ON p.marca = m.id_marca
INNER JOIN tb_Unidade AS u ON p.unidade = u.id_unidade
INNER JOIN tb_Embalagem AS e ON p.embalagem = e.id_embalagem;

--Unidades
SELECT id_unidade AS id, unidade
FROM tb_Unidade;

--Marcas
SELECT id_marca AS id, marca
FROM tb_Marca;

--Embalagens
SELECT id_embalagem AS id, embalagem
FROM tb_Embalagem

--Estabelecimentos
SELECT id_estabelecimento AS id, nome, bairro, cidade, numero
FROM tb_Estabelecimento;

--Produtos
SELECT p.id_produto AS id, m.marca, p.nome, e.embalagem, u.unidade, p.codigoDeBarras, p.tipoCodigoDeBarras
FROM tb_Produto AS p
INNER JOIN tb_Marca AS m ON p.marca = m.id_marca
INNER JOIN tb_Unidade AS u ON p.unidade = u.id_unidade
INNER JOIN tb_Embalagem AS e ON p.embalagem = e.id_embalagem
ORDER BY m.marca, p.nome, e.embalagem, u.unidade

--Produtos de algum estabelecimento
DECLARE @idEstabelecimento INT;
SET @idEstabelecimento = 3;
SELECT i.data, i.preco, i.qualificacao, p.id_produto AS idProduto, m.marca, p.nome, e.embalagem, u.unidade
FROM tb_Item AS i
INNER JOIN tb_Estabelecimento AS estab ON estab.id_estabelecimento = @idEstabelecimento
										AND estab.id_estabelecimento = i.id_estabelecimento
INNER JOIN tb_Produto AS p ON p.id_produto = i.id_produto
INNER JOIN tb_Marca AS m ON p.marca = m.id_marca
INNER JOIN tb_Unidade AS u ON p.unidade = u.id_unidade
INNER JOIN tb_Embalagem AS e ON p.embalagem = e.id_embalagem
ORDER BY i.data DESC, i.qualificacao DESC;