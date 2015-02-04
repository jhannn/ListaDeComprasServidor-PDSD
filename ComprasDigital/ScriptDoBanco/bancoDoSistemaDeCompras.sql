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
INSERT INTO tb_Unidade VALUES (5, 'ML');
INSERT INTO tb_Unidade VALUES (6, 'Penca, Molho');



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
	id_item INT PRIMARY KEY IDENTITY(1,1),
	id_estabelecimento INT FOREIGN KEY REFERENCES tb_Estabelecimento(id_estabelecimento) NOT NULL,
	id_produto INT FOREIGN KEY REFERENCES tb_Produto(id_produto) NOT NULL,
	preco FLOAT NOT NULL,
	qualificacao INT NOT NULL,
	data DATE NOT NULL
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





INSERT INTO tb_Usuario VALUES ('teste_banco', 'teste_banco', '123', '1');
INSERT INTO tb_Usuario VALUES ('teste_banco2', 'teste_banco2', '456', '2');

INSERT INTO tb_Marca VALUES ('');
INSERT INTO tb_Marca VALUES ('garoto');
INSERT INTO tb_Marca VALUES ('nestle');
INSERT INTO tb_Marca VALUES ('parmalat');

INSERT INTO tb_Estabelecimento VALUES ('Nordestão', 'ZN', 'Natal', 0);
INSERT INTO tb_Estabelecimento VALUES ('Carrefour', 'ZN', 'Natal', 0);
INSERT INTO tb_Estabelecimento VALUES ('Nordestão', 'ZS', 'Natal', 0);

INSERT INTO tb_Produto VALUES('banana',1 , null, null, 2, 1);
INSERT INTO tb_Produto VALUES('maçã',1 , null, null, 2, 1);
INSERT INTO tb_Produto VALUES('gasolina',1 , null, null, 4, 1);
INSERT INTO tb_Produto VALUES('alcool',1 , null, null, 4, 1);
INSERT INTO tb_Produto VALUES('alcool',1 , null, null, 1, 5);
INSERT INTO tb_Produto VALUES('variedades',3 , null, null, 1, 4);
INSERT INTO tb_Produto VALUES('neston 200ml',3 , null, null, 1, 5);
INSERT INTO tb_Produto VALUES('nescau 200ml',3 , null, null, 1, 5);
INSERT INTO tb_Produto VALUES('alpino 200ml',3 , null, null, 1, 5);
INSERT INTO tb_Produto VALUES('farinha lactea 400g',3 , null, null, 1, 6);
INSERT INTO tb_Produto VALUES('baton',2 , null, null, 1, 2);
INSERT INTO tb_Produto VALUES('talento 100g',2 , null, null, 1, 7);
INSERT INTO tb_Produto VALUES('talento',2 , null, null, 1, 2);
INSERT INTO tb_Produto VALUES('castanha de caju',2 , null, null, 1, 7);
INSERT INTO tb_Produto VALUES('leite integral',4 , null, null, 1, 4);
INSERT INTO tb_Produto VALUES('milk',4 , null, null, 1, 4);
INSERT INTO tb_Produto VALUES('classico semidesnatado',4 , null, null, 1, 4);

--('Nordestão', 'ZN', 'Natal', 0);
INSERT INTO tb_Item VALUES(1, 01, 1.99, 5, GETDATE());	--banana
INSERT INTO tb_Item VALUES(1, 01, 2.00, 1, GETDATE()-2);--banana de outra data
INSERT INTO tb_Item VALUES(1, 01, 1.95, 1, GETDATE()-9);--banana de outra data
INSERT INTO tb_Item VALUES(1, 01, 2.00, 1, GETDATE());	--banana com preço errado
INSERT INTO tb_Item VALUES(1, 02, 3.50, 7, GETDATE());	--maçã
INSERT INTO tb_Item VALUES(1, 02, 3.50, 7, GETDATE()-3);--maçã de outra data
INSERT INTO tb_Item VALUES(1, 02, 3.50, 7, GETDATE()-9);--maçã de outra data
INSERT INTO tb_Item VALUES(1, 05, 2.30, 6, GETDATE());	--alcool garrafa
INSERT INTO tb_Item VALUES(1, 05, 2.00, 2, GETDATE());	--alcool garrafa com preço errado
INSERT INTO tb_Item VALUES(1, 05, 2.00, 2, GETDATE()-3);--alcool garrafa outra data
INSERT INTO tb_Item VALUES(1, 06, 3.75, 5, GETDATE());	--variedades
INSERT INTO tb_Item VALUES(1, 06, 3.80, 5, GETDATE()-3);--variedades de outra data
INSERT INTO tb_Item VALUES(1, 07, 2.50, 2, GETDATE());	--neston 200ml
INSERT INTO tb_Item VALUES(1, 07, 2.40, 2, GETDATE());	--neston 200ml
INSERT INTO tb_Item VALUES(1, 08, 2.55, 5, GETDATE());	--nescau 200ml
INSERT INTO tb_Item VALUES(1, 08, 2.50, 1, GETDATE());	--nescau 200ml com preço errado
INSERT INTO tb_Item VALUES(1, 08, 2.50, 1, GETDATE());	--nescau 200ml com preço errado
INSERT INTO tb_Item VALUES(1, 09, 2.90, 2, GETDATE()-2);--alpino 200ml outra data
INSERT INTO tb_Item VALUES(1, 10, 5.55, 6, GETDATE());	--farinha lactea 400g
INSERT INTO tb_Item VALUES(1, 11, 1.00, 2, GETDATE());	--Baton
INSERT INTO tb_Item VALUES(1, 16, 2.00, 3, GETDATE());	--Milk

--('Nordestão', 'ZS', 'Natal', 0);
INSERT INTO tb_Item VALUES(3, 01, 2.99, 5, GETDATE());	--banana
INSERT INTO tb_Item VALUES(3, 01, 3.00, 1, GETDATE()-2);--banana de outra data
INSERT INTO tb_Item VALUES(3, 01, 2.95, 1, GETDATE()-9);--banana de outra data
INSERT INTO tb_Item VALUES(3, 01, 3.00, 1, GETDATE());	--banana com preço errado
INSERT INTO tb_Item VALUES(3, 02, 4.50, 7, GETDATE());	--maçã
INSERT INTO tb_Item VALUES(3, 02, 4.50, 7, GETDATE()-3);--maçã de outra data
INSERT INTO tb_Item VALUES(3, 02, 4.50, 7, GETDATE()-9);--maçã de outra data
INSERT INTO tb_Item VALUES(3, 05, 3.30, 6, GETDATE());	--alcool garrafa
INSERT INTO tb_Item VALUES(3, 05, 3.00, 2, GETDATE());	--alcool garrafa com preço errado
INSERT INTO tb_Item VALUES(3, 05, 3.00, 2, GETDATE()-3);--alcool garrafa outra data
INSERT INTO tb_Item VALUES(3, 06, 4.75, 5, GETDATE());	--variedades
INSERT INTO tb_Item VALUES(3, 06, 4.80, 5, GETDATE()-3);--variedades de outra data
INSERT INTO tb_Item VALUES(3, 07, 3.50, 2, GETDATE());	--neston 200ml
INSERT INTO tb_Item VALUES(3, 07, 3.40, 2, GETDATE());	--neston 200ml
INSERT INTO tb_Item VALUES(3, 08, 3.55, 5, GETDATE());	--nescau 200ml
INSERT INTO tb_Item VALUES(3, 08, 3.50, 1, GETDATE());	--nescau 200ml com preço errado
INSERT INTO tb_Item VALUES(3, 08, 3.50, 1, GETDATE());	--nescau 200ml com preço errado
INSERT INTO tb_Item VALUES(3, 09, 3.90, 2, GETDATE()-2);--alpino 200ml outra data
INSERT INTO tb_Item VALUES(3, 10, 6.55, 6, GETDATE());	--farinha lactea 400g
INSERT INTO tb_Item VALUES(3, 11, 2.00, 2, GETDATE());	--Baton
INSERT INTO tb_Item VALUES(3, 16, 3.00, 3, GETDATE());	--Milk

--('Carrefour', 'ZN', 'Natal', 0);
INSERT INTO tb_Item VALUES(2, 01, 2.15, 5, GETDATE());	--banana
INSERT INTO tb_Item VALUES(2, 01, 2.00, 1, GETDATE()-2);--banana de outra data
INSERT INTO tb_Item VALUES(2, 01, 1.95, 1, GETDATE()-9);--banana de outra data
INSERT INTO tb_Item VALUES(2, 01, 2.00, 1, GETDATE());	--banana com preço errado
INSERT INTO tb_Item VALUES(2, 05, 2.20, 6, GETDATE());	--alcool garrafa
INSERT INTO tb_Item VALUES(2, 05, 2.00, 2, GETDATE());	--alcool garrafa com preço errado
INSERT INTO tb_Item VALUES(2, 05, 2.00, 2, GETDATE()-3);--alcool garrafa outra data
INSERT INTO tb_Item VALUES(2, 06, 3.70, 5, GETDATE());	--variedades
INSERT INTO tb_Item VALUES(2, 06, 3.80, 5, GETDATE()-3);--variedades de outra data
INSERT INTO tb_Item VALUES(2, 07, 2.50, 2, GETDATE());	--neston 200ml
INSERT INTO tb_Item VALUES(2, 07, 2.40, 2, GETDATE());	--neston 200ml
INSERT INTO tb_Item VALUES(2, 08, 2.55, 5, GETDATE());	--nescau 200ml
INSERT INTO tb_Item VALUES(2, 09, 2.90, 2, GETDATE()-2);--alpino 200ml outra data
INSERT INTO tb_Item VALUES(2, 10, 5.50, 6, GETDATE());	--farinha lactea 400g

INSERT INTO tb_ListaDeProdutos VALUES ('Lista A', 1);
INSERT INTO tb_ListaDeProdutos VALUES ('Lista B', 1);
INSERT INTO tb_ListaDeProdutos VALUES ('Lista a', 2);

INSERT INTO tb_ProdutoDaLista VALUES (10, 1, 2);	--Lista A	-Farinha Lactea 400g
INSERT INTO tb_ProdutoDaLista VALUES (08, 1, 3);	--Lista A	-Nescau 200ml
INSERT INTO tb_ProdutoDaLista VALUES (11, 1, 3);	--Lista A	-Baton
INSERT INTO tb_ProdutoDaLista VALUES (06, 1, 1);	--Lista A	-Variedades
INSERT INTO tb_ProdutoDaLista VALUES (16, 1, 2);	--Lista A	-Milk
INSERT INTO tb_ProdutoDaLista VALUES (01, 1, 1);	--Lista A	-Banana
INSERT INTO tb_ProdutoDaLista VALUES (02, 1, 1);	--Lista A	-Maçã

INSERT INTO tb_ProdutoDaLista VALUES (10, 2, 1);	--Lista B	-Farinha Lactea 400g
INSERT INTO tb_ProdutoDaLista VALUES (11, 2, 3);	--Lista B	-Baton
INSERT INTO tb_ProdutoDaLista VALUES (01, 2, 1);	--Lista B	-Banana
INSERT INTO tb_ProdutoDaLista VALUES (02, 2, 1);	--Lista B	-Maçã

INSERT INTO tb_ProdutoDaLista VALUES (10, 3, 3);	--Lista C	-Farinha Lactea 400g
INSERT INTO tb_ProdutoDaLista VALUES (16, 3, 2);	--Lista C	-Milk
INSERT INTO tb_ProdutoDaLista VALUES (01, 3, 1);	--Lista C	-Banana
INSERT INTO tb_ProdutoDaLista VALUES (02, 3, 1);	--Lista C	-Maçã