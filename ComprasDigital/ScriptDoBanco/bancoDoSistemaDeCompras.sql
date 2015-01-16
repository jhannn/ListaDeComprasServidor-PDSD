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
	marca VARCHAR(50),
	tipo INT FOREIGN KEY REFERENCES tb_Tipo(id_tipo) NOT NULL,
	unidade INT FOREIGN KEY REFERENCES tb_Unidade(id_unidade) NOT NULL
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
	produtoAntigo INT FOREIGN KEY REFERENCES tb_Produto(id_produto) NOT NULL,
	produtoNovo INT FOREIGN KEY REFERENCES tb_Produto(id_produto) NOT NULL,
	ocorrencias INT FOREIGN KEY REFERENCES tb_Ocorrencia(id_ocorrencia) NOT NULL
);

--______ Enumerators ______--

CREATE TABLE tb_Tipo
(
	id_tipo INT PRIMARY KEY IDENTITY(1,1),
	tipo varchar(50)
);
INSERT INTO tb_Unidade VALUES ('Outro');
INSERT INTO tb_Unidade VALUES ('Combustivel');
INSERT INTO tb_Unidade VALUES ('Futa, Legume ou Verdura');
INSERT INTO tb_Unidade VALUES ('Eletrônico');
--Adicionar outros

CREATE TABLE tb_Unidade
(
	id_unidade INT PRIMARY KEY IDENTITY(1,1),
	unidade varchar(50)
);
INSERT INTO tb_Unidade VALUES ('Unidade');
INSERT INTO tb_Unidade VALUES ('KG');
INSERT INTO tb_Unidade VALUES ('Gramas');
INSERT INTO tb_Unidade VALUES ('Litro');

CREATE TABLE tb_Ocorrencia
(
	id_ocorrencia INT PRIMARY KEY IDENTITY(1,1),
	ocorrencia varchar(50)
);
INSERT INTO tb_Ocorrencia VALUES ('Codigo de barras ja existente');
INSERT INTO tb_Ocorrencia VALUES ('Codigo de barras diferente do existente');
INSERT INTO tb_Ocorrencia VALUES ('Tipo diferente');
INSERT INTO tb_Ocorrencia VALUES ('Unidade diferente');