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

CREATE TABLE tb_Marca
(
	id_marca INT PRIMARY KEY IDENTITY(1,1),
	marca VARCHAR(50)
);

--______ Enumerators ______--

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
	marca INT FOREIGN KEY REFERENCES tb_Marca(id_marca) NOT NULL,
	nome VARCHAR(50) NOT NULL, 
	codigoDeBarras VARCHAR(50),
	tipoCodigoDeBarras VARCHAR(50),
	tipo INT FOREIGN KEY REFERENCES tb_Tipo(id_tipo) NOT NULL,
	unidade INT FOREIGN KEY REFERENCES tb_Unidade(id_unidade) NOT NULL,
	PRIMARY KEY CLUSTERED (nome, marca),
);

CREATE TABLE tb_ListaDeProdutos
(
	id_listaDeProdutos INT PRIMARY KEY IDENTITY(1,1),
	nome VARCHAR(50),
	id_usuario INT FOREIGN KEY REFERENCES tb_Usuario(id_usuario) NOT NULL
);

CREATE TABLE tb_ProdutoDaLista
(
	nome_produto VARCHAR(50) NOT NULL,
	marca_produto INT NOT NULL,
	id_lista INT FOREIGN KEY REFERENCES tb_ListaDeProdutos(id_listadeProdutos) NOT NULL,
	quantidade INT NOT NULL,
	PRIMARY KEY CLUSTERED (nome_produto, marca_produto, id_lista),
	FOREIGN KEY (nome_produto, marca_produto) REFERENCES tb_Produto(nome, marca)
);

CREATE TABLE tb_Item
(
	preco FLOAT NOT NULL,
	qualificacao INT NOT NULL,
	compraMaisRecente DATE NOT NULL,
	id_estabelecimento INT FOREIGN KEY REFERENCES tb_Estabelecimento(id_estabelecimento) NOT NULL,
	nome_produto VARCHAR(50) NOT NULL,
	marca_produto INT NOT NULL,
	FOREIGN KEY (nome_produto, marca_produto) REFERENCES tb_Produto(nome, marca),
	PRIMARY KEY CLUSTERED (nome_produto, marca_produto, id_estabelecimento)
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
	quantidade INT NOT NULL,
	estabelecimento_item INT NOT NULL,
	nome_item VARCHAR(50) NOT NULL,
	marca_item INT NOT NULL,
	FOREIGN KEY (nome_item, marca_item, estabelecimento_item) REFERENCES tb_Item(nome_produto, marca_produto, id_estabelecimento),
	PRIMARY KEY CLUSTERED (nome_item, marca_item, estabelecimento_item, id_lista)
);

CREATE TABLE tb_ProdutoInvalido
(
	nome_produtoAntigo VARCHAR(50) NOT NULL,
	marca_produtoAntigo INT NOT NULL,
	nome_produtoNovo VARCHAR(50) NOT NULL,
	marca_produtoNovo INT NOT NULL,
	ocorrencia INT FOREIGN KEY REFERENCES tb_Ocorrencia(id_ocorrencia) NOT NULL,
	quantidadeDeOcorrencias INT NOT NULL,
	FOREIGN KEY (nome_produtoAntigo, marca_produtoAntigo) REFERENCES tb_Produto(nome, marca),
	FOREIGN KEY (nome_produtoNovo, marca_produtoNovo) REFERENCES tb_Produto(nome, marca),
	PRIMARY KEY CLUSTERED (nome_produtoAntigo, marca_produtoAntigo, nome_produtoNovo, marca_produtoNovo, ocorrencia)
);


select * from tb_Produto
select * from tb_Marca
select * from tb_Usuario
select * from tb_ListaDeProdutos

insert into tb_Marca values('red');
insert into tb_Produto values(1,'red','4324','aaa',1,1);
insert into tb_Usuario values('jon','jon','123','123');
insert into tb_ListaDeProdutos values('semanal',1);
insert into tb_ListaDeProdutos values('mensal',1);

