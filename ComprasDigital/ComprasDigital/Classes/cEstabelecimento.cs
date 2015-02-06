using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ComprasDigital.Model;
using System.Web.Script.Serialization;
using System.Collections;

namespace ComprasDigital.Classes
{
	public class cEstabelecimento
	{
		public string nome { get; set; }
		public string bairro { get; set; }
		public string cidade { get; set; }
		public int id_estabelecimento { get; set; }
		public int numero { get; set; }

		public cEstabelecimento() { }

		public cEstabelecimento(tb_Estabelecimento estab)
		{
			nome = estab.nome;
			bairro = estab.bairro;
			cidade = estab.cidade;
			id_estabelecimento = estab.id_estabelecimento;
			numero = estab.numero;
		}



        //_____________________________________ RETORNAR PREÇO POR ESTABELECIMENTO _______________________________________//
        public object retornarPrecoEstabelecimento(int idEstabelecimento, int idLista)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            var totalDaLista = 0;
            var produtosEncontradosNoEstabelecimento = 0;

            var dataContext = new Model.DataClassesDataContext();
            var produtoDaLista = from pl in dataContext.tb_ProdutoDaListas
                                 where pl.id_lista == idLista
                                 select pl;

            ArrayList produtos = new ArrayList();
            ArrayList quantidade = new ArrayList();
            foreach (var prod in produtoDaLista)
            {
                produtos.Add(prod.id_produto); //--pegando produtos da lista
                quantidade.Add(prod.quantidade); //--pegando as quantidades
            }



            var itens = from i in dataContext.tb_Items
                        where i.id_estabelecimento == idEstabelecimento
                        select i;

            ArrayList estabelecimentoValor = new ArrayList();
            foreach (var item in itens)
            {
                for (var i = 0; i < produtos.Count; i++)
                {
                    if (item.id_produto == Convert.ToInt32(produtos[i]))
                    {
                        produtosEncontradosNoEstabelecimento++;
                        totalDaLista += Convert.ToInt32(item.preco) * Convert.ToInt32(quantidade[i]);
                    }
                }
            }

            ArrayList retorno = new ArrayList();
            retorno.Add(totalDaLista);//valor da lista
            retorno.Add(produtos.Count - produtosEncontradosNoEstabelecimento);//produtos nao encontrados
            retorno.Add(produtosEncontradosNoEstabelecimento); //produtos encontrados

            return retorno;
        }
	}
}