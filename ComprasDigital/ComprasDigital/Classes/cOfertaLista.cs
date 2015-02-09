using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ComprasDigital.Model;
using System.Web.Script.Serialization;
using System.Collections;

namespace ComprasDigital.Classes
{
    public class cOfertaLista
    {
        public int idEstabelecimento { get; set; }
        public string nomeEstabelecimento { get; set; }
		public double precoLista{ get; set; }
		public int itensEncontrados{ get; set; }
		public int itensTotal{ get; set; }


		public cOfertaLista() { }

		public cOfertaLista(int idEstab, string nome, int preco, int encontrados, int totalDeProdutos)
		{
			idEstabelecimento = idEstab;
			nomeEstabelecimento = nome;
			precoLista = preco;
			itensEncontrados = encontrados;
			itensTotal = totalDeProdutos;
		}

		public cOfertaLista(int idList, int idEstab)
		{
			var dataContext = new DataClassesDataContext();
			this.idEstabelecimento = idEstab;
			this.itensTotal = (from p in dataContext.tb_ProdutoDaListas where p.id_lista == idList select p).Count();
			this.nomeEstabelecimento = dataContext.tb_Estabelecimentos.First(e => e.id_estabelecimento == idEstab).ToString();
			var itens = from i in dataContext.tb_Items join p in dataContext.tb_ProdutoDaListas on i.id_produto equals p.id_produto where p.id_lista == idList && i.id_estabelecimento == idEstab select i;
			this.itensEncontrados = itens.Count();
			this.precoLista = itens.Sum();
		}
    }
}