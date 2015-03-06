using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ComprasDigital.Model;

namespace ComprasDigital.Classes
{
	public class jsHistoricoDeItem
	{
		public int idProduto { get; set; }
		public string nomeProduto { get; set; }
		public string marcaProduto { get; set; }
		public string unidade { get; set; }
		public double preco { get; set; }
		public int quantidade { get; set; }

		public jsHistoricoDeItem()
		{
		}

		public jsHistoricoDeItem(tb_ItemDaLista item)
		{
			DataClassesDataContext dataContext = new DataClassesDataContext();
			idProduto = Convert.ToInt32(item.id_produto);
			nomeProduto = item.nome_produto;
			marcaProduto = item.marca_produto;
			unidade = dataContext.tb_Unidades.First(u => u.id_unidade == item.unidade).unidade;
			preco = item.preco;
			quantidade = item.quantidade;
		}
	}
}