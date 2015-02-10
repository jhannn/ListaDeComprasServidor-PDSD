using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ComprasDigital.Model;
using System.Web.Script.Serialization;
using System.Collections;

namespace ComprasDigital.Classes
{
    public class cListaDeItem : cListaDeProduto
    {
        public int idEstabelecimento { get; set; }
		public string nomeEstabelecimento { get; set; }
		public List<cItem> itens { get; set; }
		public int itensTotal { get; set; }
		public int itensEncontrados{ get; set; }
		public double precoDaLista { get; set; }



		public cListaDeItem(tb_ListaDeProduto lista)
			: base(lista)
		{
			var dataContext = new DataClassesDataContext();
			var produtosDaLista = from p in dataContext.tb_ProdutoDaListas where p.id_lista == this.id_listaDeProdutos select p;
			itens = new List<cItem>();
			itensTotal = produtosDaLista.Count();
			foreach (var prod in produtosDaLista)
			{
				itens.Add(new cItem(prod.tb_Produto, prod.quantidade));
			}

			idEstabelecimento = -1;
			nomeEstabelecimento = "-";
			itensEncontrados = 0;
			precoDaLista = 0;
		}



		public cListaDeItem(tb_ListaDeProduto lista, tb_Estabelecimento estab)
			: base(lista)
		{
			var dataContext = new DataClassesDataContext();
			this.idEstabelecimento = estab.id_estabelecimento;
			this.nomeEstabelecimento = estab.nome;
			var produtosDaLista = from p in dataContext.tb_ProdutoDaListas where p.id_lista == this.id_listaDeProdutos select p;
			itens = new List<cItem>();
			itensTotal = produtosDaLista.Count();
			itensEncontrados = 0;
			precoDaLista = 0;
			foreach (var prod in produtosDaLista)
			{
				itens.Add(new cItem(prod.tb_Produto, estab, prod.quantidade));
				if (itens.Last().dataAtual != "-")
				{
					itensEncontrados++;
					precoDaLista += itens.Last().preco * prod.quantidade;
				}
			}
		}



		public static cListaDeItem[] buscarOfertas(tb_ListaDeProduto lista)
		{
			List<cListaDeItem> listas = new List<cListaDeItem>();
			var dataContext = new DataClassesDataContext();
			var estabelecimentos = from e in dataContext.tb_Estabelecimentos select e;
			foreach (var estab in estabelecimentos)
				listas.Add(new cListaDeItem(lista, estab));
			return listas.ToArray();
		}



		public static cListaDeItem buscarMelhorOferta(tb_ListaDeProduto lista)
		{
			cListaDeItem[] listas = buscarOfertas(lista);
			if (listas.Length > 1)
			{
				int idLista = 0;
				double custo = 0;
				for (int i = 0; i < listas.Length; i++)
				{
					if (listas[i].precoDaLista < custo)
					{
						custo = listas[i].precoDaLista;
						idLista = i;
					}
				}
				return listas[idLista];
			}
			else if (listas.Length > 0)
			{
				return listas[0];
			}
			return null;
		}
    }
}