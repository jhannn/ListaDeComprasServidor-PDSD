using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ComprasDigital.Model;

namespace ComprasDigital.Classes
{
	public class cProdutoDaLista
	{
		public int id_produto { get; set; }
		public string marca { get; set; }
		public string nome { get; set; }
		public string codigoDeBarras { get; set; }
		public string tipoCodigoDeBarras { get; set; }
		public string unidade { get; set; }
		public int quantidade { get; set; }

		public cProdutoDaLista(tb_ProdutoDaLista prod)
		{
			marca = prod.tb_Produto.tb_Marca.marca;
			codigoDeBarras = prod.tb_Produto.codigoDeBarras;
			nome = prod.tb_Produto.nome;
			id_produto = prod.id_produto;
			tipoCodigoDeBarras = prod.tb_Produto.tipoCodigoDeBarras;
			unidade = prod.tb_Produto.tb_Unidade.unidade;
			quantidade = prod.quantidade;
		}
	}
}