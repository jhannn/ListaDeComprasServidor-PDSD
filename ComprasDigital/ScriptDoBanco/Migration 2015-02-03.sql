DROP TABLE tb_Item

CREATE TABLE tb_Item
(
	id_item INT PRIMARY KEY IDENTITY(1,1),
	id_estabelecimento INT FOREIGN KEY REFERENCES tb_Estabelecimento(id_estabelecimento) NOT NULL,
	id_produto INT FOREIGN KEY REFERENCES tb_Produto(id_produto) NOT NULL,
	preco FLOAT NOT NULL,
	qualificacao INT NOT NULL,
	data DATE NOT NULL
);



--O item válido será aquele que tiver data mais recente e maior qualificação.
--Ao final do dia, deverá ser criada uma rotina pra remover os produtos do dia anterior que tiverem menor qualificação.