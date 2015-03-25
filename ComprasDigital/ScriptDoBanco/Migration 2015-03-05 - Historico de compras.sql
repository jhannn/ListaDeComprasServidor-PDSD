USE SistemaDeCompras;
ALTER TABLE tb_ListaDeItens ADD id_estabelecimento INT;
ALTER TABLE tb_ListaDeItens ADD FOREIGN KEY (id_estabelecimento) REFERENCES tb_Estabelecimento(id_estabelecimento);
ALTER TABLE tb_ItemDaLista ADD id_produto INT;
ALTER TABLE tb_ItemDaLista ADD FOREIGN KEY (id_produto) REFERENCES tb_Produto(id_produto);