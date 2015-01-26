CREATE DATABASE SistemaDeCompras;
USE SistemaDeCompras;

CREATE TABLE tb_Usuario
(
	id_usuario INT PRIMARY KEY IDENTITY(1,1),
	nome VARCHAR(50) NOT NULL,
	email VARCHAR(50) NOT NULL,
	senha VARCHAR(100) NOT NULL,
	token VARCHAR(50) NULL
);



CREATE TABLE tb_Estabelecimento
(
	id_estabelecimento INT PRIMARY KEY IDENTITY(1,1),
	nome VARCHAR(50) NOT NULL,
	bairro VARCHAR(100) NOT NULL,
	cidade VARCHAR(100) NOT NULL,
	numero INT NOT NULL
);



CREATE TABLE tb_Marca
(
	id_marca INT PRIMARY KEY IDENTITY(1,1),
	marca VARCHAR(50)
);



CREATE TABLE tb_Tipo
(
	id_tipo INT PRIMARY KEY IDENTITY(1,1),
	tipo varchar(50)
);
INSERT INTO tb_Tipo VALUES ('Outro');
INSERT INTO tb_Tipo VALUES ('Combustivel');
INSERT INTO tb_Tipo VALUES ('Futa, Legume ou Verdura');
INSERT INTO tb_Tipo VALUES ('Eletrônico');
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



CREATE TABLE tb_Produto
(
	id_produto INT PRIMARY KEY IDENTITY(1,1),
	nome VARCHAR(50) NOT NULL, 
	marca INT FOREIGN KEY REFERENCES tb_Marca(id_marca) NOT NULL,
	codigoDeBarras VARCHAR(50),
	tipoCodigoDeBarras VARCHAR(50),
	unidade INT FOREIGN KEY REFERENCES tb_Unidade(id_unidade) NOT NULL,
);



CREATE TABLE tb_ListaDeProdutos
(
	id_listaDeProdutos INT PRIMARY KEY IDENTITY(1,1),
	nome VARCHAR(50),
	id_usuario INT FOREIGN KEY REFERENCES tb_Usuario(id_usuario) NOT NULL
);



CREATE TABLE tb_ProdutoDaLista
(
	id_produto INT FOREIGN KEY REFERENCES tb_Produto(id_produto) NOT NULL,
	id_lista INT FOREIGN KEY REFERENCES tb_ListaDeProdutos(id_listadeProdutos) NOT NULL,
	quantidade INT NOT NULL,
	PRIMARY KEY CLUSTERED (id_produto, id_lista),
);



CREATE TABLE tb_ProdutoInvalido
(
	id_produtoAntigo INT FOREIGN KEY REFERENCES tb_Produto(id_produto) NOT NULL,
	id_produtoNovo INT FOREIGN KEY REFERENCES tb_Produto(id_produto) NOT NULL,
	ocorrencia INT FOREIGN KEY REFERENCES tb_Ocorrencia(id_ocorrencia) NOT NULL,
	quantidadeDeOcorrencias INT NOT NULL,
	PRIMARY KEY CLUSTERED (id_produtoAntigo, id_produtoNovo, ocorrencia)
);



CREATE TABLE tb_Item
(
	preco FLOAT NOT NULL,
	compraMaisRecente DATE NOT NULL,
	id_estabelecimento INT FOREIGN KEY REFERENCES tb_Estabelecimento(id_estabelecimento) NOT NULL,
	id_produto INT FOREIGN KEY REFERENCES tb_Produto(id_produto) NOT NULL,
	PRIMARY KEY CLUSTERED (id_produto, id_estabelecimento)
);



CREATE TABLE tb_ListaDeItens
(
	id_listaDeItens INT PRIMARY KEY IDENTITY(1,1),
	dataDeCompras DATE NOT NULL,
	id_usuario INT FOREIGN KEY REFERENCES tb_Usuario(id_usuario) NOT NULL,
);



CREATE TABLE tb_ItemDaLista
(
	id_lista INT FOREIGN KEY REFERENCES tb_ListaDeItens(id_listaDeItens) NOT NULL,
	nome_produto VARCHAR(50) NOT NULL,
	marca_produto VARCHAR(50) NOT NULL,
	preco FLOAT NOT NULL,
	unidade INT FOREIGN KEY REFERENCES tb_Unidade(id_unidade) NOT NULL,
	quantidade INT NOT NULL,
);