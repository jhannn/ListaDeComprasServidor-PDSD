USE SistemaDeCompras;

CREATE TABLE tb_Embalagem
(
	id_embalagem INT PRIMARY KEY IDENTITY(1,1),
	embalagem varchar(20)
);
INSERT INTO tb_Embalagem VALUES ('Outra');
INSERT INTO tb_Embalagem VALUES ('Unidade');
INSERT INTO tb_Embalagem VALUES ('Pacote');
INSERT INTO tb_Embalagem VALUES ('Caixa');
INSERT INTO tb_Embalagem VALUES ('Garrafa');
INSERT INTO tb_Embalagem VALUES ('Lata');
INSERT INTO tb_Embalagem VALUES ('Barra');
INSERT INTO tb_Embalagem VALUES ('Peso');

ALTER TABLE tb_Produto ADD embalagem INT;
ALTER TABLE tb_Produto ADD FOREIGN KEY (embalagem) REFERENCES tb_Embalagem(id_embalagem);