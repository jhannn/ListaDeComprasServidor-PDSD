using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ComprasDigital.Model;
using System.Web.Script.Serialization;
using System.Collections;

namespace ComprasDigital.Classes
{
    public class cPrecoPorEstabelecimento
    {
        public string nomeEstabelecimento { get; set; }
		public int precoLista{ get; set; }
		public int produtosEncontrados{ get; set; }
		public int produtosNaoEncontrados{ get; set; }


        public cPrecoPorEstabelecimento() { }

        public cPrecoPorEstabelecimento(string nome,int preco,int encontrados,int naoEncontrados)
		{
			nomeEstabelecimento = nome;
            precoLista = preco;
            produtosEncontrados = encontrados;
            produtosNaoEncontrados = naoEncontrados;
		}

        //_____________________________________ RETORNAR PREÇO POR ESTABELECIMENTO _______________________________________//
        public object retornarPrecoEstabelecimento(int idEstabelecimento,string nome,int idLista)
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

            cPrecoPorEstabelecimento estab = new cPrecoPorEstabelecimento(nome,totalDaLista, produtosEncontradosNoEstabelecimento, produtos.Count - produtosEncontradosNoEstabelecimento);

            return estab;
        }

        //________________________________________ ORDENAR ESTABELECIMENTO POR PREÇO E RELAÇÃO _________________________________//
    }
}