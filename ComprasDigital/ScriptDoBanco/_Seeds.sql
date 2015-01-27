USE SistemaDeCompras;
--Seeds



INSERT INTO tb_Marca VALUES ('');
INSERT INTO tb_Marca VALUES ('garoto');
INSERT INTO tb_Marca VALUES ('nestle');
INSERT INTO tb_Marca VALUES ('parmalat');

INSERT INTO tb_Estabelecimento VALUES ('Nordestão', 'ZN', 'Natal', 0);
INSERT INTO tb_Estabelecimento VALUES ('Carrefour', 'ZN', 'Natal', 0);
INSERT INTO tb_Estabelecimento VALUES ('Nordestão', 'ZS', 'Natal', 0);



DECLARE @vazio INT;
SET @vazio = (SELECT id_marca FROM tb_Marca WHERE marca = '');
DECLARE @garoto INT;
SET @garoto = (SELECT id_marca FROM tb_Marca WHERE marca = 'garoto');
DECLARE @nestle INT;
SET @nestle = (SELECT id_marca FROM tb_Marca WHERE marca = 'nestle');
DECLARE @parmalat INT;
SET @parmalat = (SELECT id_marca FROM tb_Marca WHERE marca = 'parmalat');

DECLARE @Outra INT;
SET @Outra = (SELECT id_embalagem FROM tb_Embalagem WHERE embalagem = 'Outra');
DECLARE @Unidade INT;
SET @Unidade = (SELECT id_embalagem FROM tb_Embalagem WHERE embalagem = 'Unidade');
DECLARE @Pacote INT;
SET @Pacote = (SELECT id_embalagem FROM tb_Embalagem WHERE embalagem = 'Pacote');
DECLARE @Caixa INT;
SET @Caixa = (SELECT id_embalagem FROM tb_Embalagem WHERE embalagem = 'Caixa');
DECLARE @Garrafa INT;
SET @Garrafa = (SELECT id_embalagem FROM tb_Embalagem WHERE embalagem = 'Garrafa');
DECLARE @Lata INT;
SET @Lata = (SELECT id_embalagem FROM tb_Embalagem WHERE embalagem = 'Lata');
DECLARE @Barra INT;
SET @Barra = (SELECT id_embalagem FROM tb_Embalagem WHERE embalagem = 'Barra');
DECLARE @Peso INT;
SET @Peso = (SELECT id_embalagem FROM tb_Embalagem WHERE embalagem = 'Peso');

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