using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ComprasDigital.Model;

namespace ComprasDigital.Classes
{
	public class jsHistoricoDeLista
	{
		public string nomeEstabelecimento { get; set; }
		public int idEstabelecimento { get; set; }
		public string dataDeCompras { get; set; }
		public double precoTotal { get; set; }
		public int idLista { get; set; }
		public List<jsHistoricoDeItem> itens { get; set; }

		public jsHistoricoDeLista()
		{
		}

		public jsHistoricoDeLista(tb_ListaDeIten lista)
		{
			nomeEstabelecimento = "";
			idEstabelecimento = 0;
			dataDeCompras = lista.dataDeCompras.ToString();
			idLista = lista.id_listaDeItens;
			precoTotal = 0;
			DataClassesDataContext dataContext = new DataClassesDataContext();
			var itensDaLista = from i in dataContext.tb_ItemDaListas where i.id_lista == lista.id_listaDeItens select i;
			foreach(tb_ItemDaLista item in itensDaLista)
			{
				precoTotal += item.preco * item.quantidade;
				itens.Add(new jsHistoricoDeItem(item));
			}
		}
	}
}