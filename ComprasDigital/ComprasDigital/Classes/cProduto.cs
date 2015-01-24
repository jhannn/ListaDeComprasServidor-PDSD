using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ComprasDigital.Model;

namespace ComprasDigital.Classes
{
	public class cProduto
	{
		public string marca { get; set; }
		public string nome { get; set; }
		public string codigoDeBarras { get; set; }
		public string tipoCodigoDeBarras { get; set; }
		public string tipo { get; set; }
		public string unidade { get; set; }

		public cProduto() { }

		public cProduto(tb_Produto prod)
		{
			marca = prod.tb_Marca.marca;
			codigoDeBarras = prod.codigoDeBarras;
			nome = prod.nome;
			tipo = prod.tb_Tipo.tipo;
			tipoCodigoDeBarras = prod.tipoCodigoDeBarras;
			unidade = prod.tb_Unidade.unidade;
		}

		public static tb_Produto criarProduto()
		{
			tb_Produto novoProduto = new tb_Produto();

			return novoProduto;
		}
	}
}