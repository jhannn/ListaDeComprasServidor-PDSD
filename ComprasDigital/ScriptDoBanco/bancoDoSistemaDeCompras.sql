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
	tipoCodigoDeBarras VARCHAR(50),
	marca VARCHAR(50)
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
	id_usuario INT FOREIGN KEY REFERENCES tb_Usuario(id_usuario) NOT NULL,
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
	quantidade INT NOT NULL,
	unidade INT FOREIGN KEY REFERENCES tb_Unidade(id_unidade) NOT NULL
);

CREATE TABLE tb_ProdutoDaLista
(
	id_produto INT FOREIGN KEY REFERENCES tb_Produto(id_produto) NOT NULL,
	id_listaP INT FOREIGN KEY REFERENCES tb_ListaDeProdutos(id_listadeProdutos) NOT NULL,
	quantidade INT NOT NULL,
	unidade INT FOREIGN KEY REFERENCES tb_Unidade(id_unidade) NOT NULL
);

CREATE TABLE tb_ProdutosInvalidos
(
	id_produto INT PRIMARY KEY IDENTITY(1,1),
	nome VARCHAR(50) NOT NULL, 
	marca VARCHAR(50), 
	codigoDeBarras VARCHAR(50),
	tipoCodigoDeBarras VARCHAR(50),
	ocorrencias INT
);



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
