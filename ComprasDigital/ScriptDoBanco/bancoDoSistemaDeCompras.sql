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



CREATE TABLE tb_Marca
(
	id_marca INT PRIMARY KEY IDENTITY(1,1),
	marca VARCHAR(25)
);



CREATE TABLE tb_Tipo
(
	id_tipo INT PRIMARY KEY,
	tipo varchar(25)
);
INSERT INTO tb_Tipo VALUES (1, 'Outro');
INSERT INTO tb_Tipo VALUES (2, 'Combustivel');
INSERT INTO tb_Tipo VALUES (3, 'Fruta, Legume ou Verdura');
INSERT INTO tb_Tipo VALUES (4, 'Eletrônico');
--Adicionar outros



CREATE TABLE tb_Unidade
(
	id_unidade INT PRIMARY KEY,
	unidade varchar(15)
);
INSERT INTO tb_Unidade VALUES (1, 'Unidade');
INSERT INTO tb_Unidade VALUES (2, 'KG');
INSERT INTO tb_Unidade VALUES (3, 'Gramas');
INSERT INTO tb_Unidade VALUES (4, 'Litro');



CREATE TABLE tb_Embalagem
(
	id_embalagem INT PRIMARY KEY,
	embalagem varchar(15)
);
INSERT INTO tb_Embalagem VALUES (1, 'Outra');
INSERT INTO tb_Embalagem VALUES (2, 'Unidade');
INSERT INTO tb_Embalagem VALUES (3, 'Pacote');
INSERT INTO tb_Embalagem VALUES (4, 'Caixa');
INSERT INTO tb_Embalagem VALUES (5, 'Garrafa');
INSERT INTO tb_Embalagem VALUES (6, 'Lata');
INSERT INTO tb_Embalagem VALUES (7, 'Barra');
INSERT INTO tb_Embalagem VALUES (8, 'Peso');



CREATE TABLE tb_Ocorrencia
(
	id_ocorrencia INT PRIMARY KEY,
	ocorrencia varchar(30)
);
INSERT INTO tb_Ocorrencia VALUES (1, 'Codigo de barras ja existente');
INSERT INTO tb_Ocorrencia VALUES (2, 'Codigo de barras diferente');
INSERT INTO tb_Ocorrencia VALUES (3, 'Tipo diferente');
INSERT INTO tb_Ocorrencia VALUES (4, 'Unidade diferente');



CREATE TABLE tb_Estabelecimento
(
	id_estabelecimento INT PRIMARY KEY IDENTITY(1,1),
	nome VARCHAR(50) NOT NULL,
	bairro VARCHAR(100) NOT NULL,
	cidade VARCHAR(100) NOT NULL,
	numero INT NOT NULL
);



CREATE TABLE tb_Produto
(
	id_produto INT PRIMARY KEY IDENTITY(1,1),
	nome VARCHAR(40) NOT NULL, 
	marca INT FOREIGN KEY REFERENCES tb_Marca(id_marca) NOT NULL,
	codigoDeBarras VARCHAR(50),
	tipoCodigoDeBarras VARCHAR(50),
	unidade INT FOREIGN KEY REFERENCES tb_Unidade(id_unidade) NOT NULL,
	embalagem INT FOREIGN KEY REFERENCES tb_Embalagem(id_embalagem) NOT NULL
);



CREATE TABLE tb_ListaDeProdutos
(
	id_listaDeProdutos INT PRIMARY KEY IDENTITY(1,1),
	nome VARCHAR(33),
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
	nome_produto VARCHAR(40) NOT NULL,
	marca_produto VARCHAR(25) NOT NULL,
	preco FLOAT NOT NULL,
	unidade INT FOREIGN KEY REFERENCES tb_Unidade(id_unidade) NOT NULL,
	quantidade INT NOT NULL,
);





/*			#----------------------------------------------#			*/
/*			|     _____   _____   _____   ___     _____    |			*/
/*			|    /       |       |       |   \   /         |			*/
/*			|    \____   |____   |____   |    |  \____     |			*/
/*			|         \  |       |       |    |       \    |			*/
/*			|    _____/  |_____  |_____  |___/   _____/    |			*/
/*			|											   |			*/
/*			#----------------------------------------------#			*/





INSERT INTO tb_Marca VALUES ('');
INSERT INTO tb_Marca VALUES ('garoto');
INSERT INTO tb_Marca VALUES ('nestle');
INSERT INTO tb_Marca VALUES ('parmalat');

INSERT INTO tb_Estabelecimento VALUES ('Nordestão', 'ZN', 'Natal', 0);
INSERT INTO tb_Estabelecimento VALUES ('Carrefour', 'ZN', 'Natal', 0);
INSERT INTO tb_Estabelecimento VALUES ('Nordestão', 'ZS', 'Natal', 0);

INSERT INTO tb_Produto VALUES('banana',@vazio , null, null, 2, @Outra);
INSERT INTO tb_Produto VALUES('maçã',@vazio , null, null, 2, @Outra);
INSERT INTO tb_Produto VALUES('gasolina',@vazio , null, null, 4, @Outra);
INSERT INTO tb_Produto VALUES('alcool',@vazio , null, null, 4, @Outra);
INSERT INTO tb_Produto VALUES('alcool',@vazio , null, null, 1, @Garrafa);
INSERT INTO tb_Produto VALUES('variedades',@nestle , null, null, 1, @Caixa);
INSERT INTO tb_Produto VALUES('neston 200ml',@nestle , null, null, 1, @Garrafa);
INSERT INTO tb_Produto VALUES('nescau 200ml',@nestle , null, null, 1, @Garrafa);
INSERT INTO tb_Produto VALUES('alpino 200ml',@nestle , null, null, 1, @Garrafa);
INSERT INTO tb_Produto VALUES('farinha lactea 400g',@nestle , null, null, 1, @Lata);
INSERT INTO tb_Produto VALUES('baton',@garoto , null, null, 1, @Unidade);
INSERT INTO tb_Produto VALUES('talento 100g',@garoto , null, null, 1, @Barra);
INSERT INTO tb_Produto VALUES('talento',@garoto , null, null, 1, @Unidade);
INSERT INTO tb_Produto VALUES('castanha de caju',@garoto , null, null, 1, @Barra);
INSERT INTO tb_Produto VALUES('leite integral',@parmalat , null, null, 1, @Caixa);
INSERT INTO tb_Produto VALUES('milk',@parmalat , null, null, 1, @Caixa);
INSERT INTO tb_Produto VALUES('classico semidesnatado',@parmalat , null, null, 1, @Caixa);



SELECT id_unidade AS id, unidade
FROM tb_Unidade;

SELECT id_marca AS id, marca
FROM tb_Marca;

SELECT id_embalagem AS id, embalagem
FROM tb_Embalagem

SELECT id_estabelecimento AS id, nome, bairro, cidade, numero
FROM tb_Estabelecimento;

SELECT p.id_produto AS id, m.marca, p.nome, e.embalagem, u.unidade, p.codigoDeBarras, p.tipoCodigoDeBarras
FROM tb_Produto AS p
INNER JOIN tb_Marca AS m ON p.marca = m.id_marca
INNER JOIN tb_Unidade AS u ON p.unidade = u.id_unidade
INNER JOIN tb_Embalagem AS e ON p.embalagem = e.id_embalagem
ORDER BY m.marca, p.nome, e.embalagem, u.unidade