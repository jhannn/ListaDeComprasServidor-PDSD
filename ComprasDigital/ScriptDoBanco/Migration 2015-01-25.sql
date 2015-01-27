USE SistemaDeCompras;
--Migration 25/01/2015



DROP TABLE tb_ItemDaLista
DROP TABLE tb_Item
DROP TABLE tb_ProdutosInvalidos
DROP TABLE tb_ProdutoDaLista
DROP TABLE tb_Produto



CREATE TABLE tb_Produto
(
	id_produto INT PRIMARY KEY IDENTITY(1,1),
	nome VARCHAR(50) NOT NULL, 
	marca INT FOREIGN KEY REFERENCES tb_Marca(id_marca) NOT NULL,
	codigoDeBarras VARCHAR(50),
	tipoCodigoDeBarras VARCHAR(50),
	unidade INT FOREIGN KEY REFERENCES tb_Unidade(id_unidade) NOT NULL,
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



CREATE TABLE tb_ItemDaLista
(
	id_lista INT FOREIGN KEY REFERENCES tb_ListaDeItens(id_listaDeItens) NOT NULL,
	nome_produto VARCHAR(50) NOT NULL,
	marca_produto VARCHAR(50) NOT NULL,
	preco FLOAT NOT NULL,
	unidade INT FOREIGN KEY REFERENCES tb_Unidade(id_unidade) NOT NULL,
	quantidade INT NOT NULL,
);