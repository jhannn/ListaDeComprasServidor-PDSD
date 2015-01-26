using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ComprasDigital.Model;

namespace ComprasDigital.Classes
{
	public class cListaDeProduto
	{
		public int id_listaDeProdutos{ get; set; }
		public string nome { get; set; }
		public int id_usuario{ get; set; }
		public ArrayList produtosDaLista { get; set; }

		public cListaDeProduto(tb_ListaDeProduto lista, bool comProdutos = false)
		{
			id_listaDeProdutos = lista.id_listaDeProdutos;
			nome = lista.nome;
			id_usuario = lista.id_usuario;
			produtosDaLista = null;

			if (comProdutos)
			{
				produtosDaLista = new ArrayList();
				foreach (tb_ProdutoDaLista prod in lista.tb_ProdutoDaListas)
					produtosDaLista.Add(new cProdutoDaLista(prod));
			}
		}
	}
}