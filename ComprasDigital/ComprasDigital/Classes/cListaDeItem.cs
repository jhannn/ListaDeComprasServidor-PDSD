using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ComprasDigital.Model;
using System.Web.Script.Serialization;
using System.Collections;

namespace ComprasDigital.Classes
{
    class cListaDeItem : cListaDeProduto
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
			//Fazer uma funcionalidade que possa mesclar resultados
			//Ex.: Lista tem item A, B, C e D
			//Nordestão tem item A, B e C, sendo a mais barata e outro estabelecimento tem os itens C e D, com C sendo mais barato que no Nordestão
			//Então o sistema sugere criar 2 listas e fazer as compras nos 2 estabelecimentos.
			cListaDeItem[] listas = buscarOfertas(lista);
			if (listas.Length > 1)
			{
				int idLista = 0;
				double custo = 0;
				int quantidadeEncontrada = 0;
				for (int i = 0; i < listas.Length; i++)
				{
					if (listas[i].itensEncontrados > quantidadeEncontrada)
					{
						custo = listas[i].precoDaLista;
						idLista = i;
						quantidadeEncontrada = listas[i].itensEncontrados;
					}
					else if (listas[i].itensEncontrados == quantidadeEncontrada && listas[i].precoDaLista < custo)
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

		public static jsHistoricoDeLista cadastrarLista(jsListaDeItens listaDeItens)
		{
			DataClassesDataContext dataContext = new DataClassesDataContext();
			tb_ListaDeIten li = new tb_ListaDeIten();
			li.dataDeCompras = listaDeItens.dataDeCompras;
			li.id_usuario = listaDeItens.idUsuario;
			dataContext.tb_ListaDeItens.InsertOnSubmit(li);
			li = dataContext.tb_ListaDeItens.First(l => l.id_usuario == listaDeItens.idUsuario && l.dataDeCompras.Equals(listaDeItens.dataDeCompras));
			tb_Produto produtoNovo;
			tb_Item itemNovo;
			foreach (jsItem item in listaDeItens.itensComprados)
			{
				if (item.idProduto > 0)
				{//produto ja existente
					produtoNovo = dataContext.tb_Produtos.First(p => p.id_produto == item.idProduto);
					var itens = from i in dataContext.tb_Items where i.data == listaDeItens.dataDeCompras && i.id_estabelecimento == listaDeItens.idEstabelecimento && i.id_produto == produtoNovo.id_produto && i.preco == item.preco select i;
					cProdutoInvalido.qualificarProduto(produtoNovo.id_produto);
					if (itens.Count() < 1)
					{
						itemNovo = new tb_Item();
						itemNovo.id_produto = produtoNovo.id_produto;
						itemNovo.preco = item.preco;
						itemNovo.id_estabelecimento = listaDeItens.idEstabelecimento;
						itemNovo.data = listaDeItens.dataDeCompras;
						itemNovo.qualificacao = 1;
					}
					else
					{
						itemNovo = itens.First();
						item.quantidade++;
					}
					dataContext.tb_Items.InsertOnSubmit(itemNovo);
				}
				else
				{//produto novo ou editado
					if (item.codigoDeBarras != null)
						produtoNovo = cProduto.criarProduto(item.marca, item.nome, item.unidade, item.embalagem);
					else
						produtoNovo = cProduto.criarProduto(item.marca, item.nome, item.unidade, item.embalagem, item.codigoDeBarras, item.tipoCodigo);
					
					itemNovo = new tb_Item();
					itemNovo.id_produto = produtoNovo.id_produto;
					itemNovo.preco = item.preco;
					itemNovo.id_estabelecimento = listaDeItens.idEstabelecimento;
					itemNovo.data = listaDeItens.dataDeCompras;
					itemNovo.qualificacao = 1;
					dataContext.tb_Items.InsertOnSubmit(itemNovo);
				}
			}
			dataContext.SubmitChanges();
			return new jsHistoricoDeLista(dataContext.tb_ListaDeItens.First(l =>l.id_usuario == listaDeItens.idUsuario && l.dataDeCompras == listaDeItens.dataDeCompras));
		}
	}
}