CREATE DATABASE SistemaDeCompras;
USE SistemaDeCompras;

CREATE TABLE tb_estabelecimento
(
	id_estab INT PRIMARY KEY IDENTITY(1,1),
	nome VARCHAR(50) NOT NULL,
	bairro VARCHAR(100) NOT NULL,
	cidade VARCHAR(100) NOT NULL,
	numero INT NOT NULL
);

CREATE TABLE tb_usuario
(
	id_use INT PRIMARY KEY IDENTITY(1,1),
	nome VARCHAR(50) NOT NULL,
	email VARCHAR(50) NOT NULL,
	senha VARCHAR(100) NOT NULL,
	token VARCHAR(50) NOT NULL
);

CREATE TABLE tb_produto
(
	id_produt INT PRIMARY KEY IDENTITY(1,1),
	nome VARCHAR(50) NOT NULL, 
	codDeBarras VARCHAR(50) NOT NULL,
	tipoCodigoDeBarras VARCHAR(50)
);

CREATE TABLE tb_item
(
	id_item INT PRIMARY KEY IDENTITY(1,1),
	preco FLOAT NOT NULL,
	qualificacao INT NOT NULL,
	compraMaisRecente DATE NOT NULL,
	id_estab INT FOREIGN KEY REFERENCES tb_estabelecimento(id_estab) NOT NULL,
	id_produt INT FOREIGN KEY REFERENCES tb_produto(id_produt) NOT NULL
);

CREATE TABLE tb_ListaDeItens
(
	id_listaI INT PRIMARY KEY IDENTITY(1,1),
	dataDeCompras DATE NOT NULL,
	id_use INT FOREIGN KEY REFERENCES tb_usuario(id_use) NOT NULL
);

CREATE TABLE tb_ListaDeProdutos
(
	id_listaP INT PRIMARY KEY IDENTITY(1,1),
	nome VARCHAR(50),
	id_use INT FOREIGN KEY REFERENCES tb_usuario(id_use) NOT NULL
);

CREATE TABLE tb_ItemDaLista
(
	id_item INT FOREIGN KEY REFERENCES tb_Item(id_item) NOT NULL,
	id_listaI INT FOREIGN KEY REFERENCES tb_ListaDeItens(id_listaI) NOT NULL,
	quantidade INT
);

CREATE TABLE tb_ProdutoDaLista
(
	id_produt INT FOREIGN KEY REFERENCES tb_Produto(id_produt) NOT NULL,
	id_listaP INT FOREIGN KEY REFERENCES tb_ListaDeProdutos(id_listaP) NOT NULL,
	quantidade INT
);








