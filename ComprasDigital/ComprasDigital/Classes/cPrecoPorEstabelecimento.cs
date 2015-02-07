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
        public int idEstabelecimento { get; set; }
        public string nomeEstabelecimento { get; set; }
		public int precoLista{ get; set; }
		public int produtosEncontrados{ get; set; }
		public int totalProdutos{ get; set; }


        public cPrecoPorEstabelecimento() { }

        public cPrecoPorEstabelecimento(int idEstab,string nome,int preco,int encontrados,int totalDeProdutos)
		{
            idEstabelecimento = idEstab;
			nomeEstabelecimento = nome;
            precoLista = preco;
            produtosEncontrados = encontrados;
            totalProdutos = totalDeProdutos;
		}

        //_____________________________________ RETORNAR PREÇO POR ESTABELECIMENTO _______________________________________//
        public object retornarPrecoEstabelecimento(int idEstabelecimento,string nome,int idLista)
        {
            var totalDaLista = 0; //valor total da lista para o determinado estabelecimento
            var produtosEncontradosNoEstabelecimento = 0; //numeros de produtos da lista encontrados no estabalecimento

            var dataContext = new Model.DataClassesDataContext();
            var produtoDaLista = from pl in dataContext.tb_ProdutoDaListas //select nos produtos da lista
                                 where pl.id_lista == idLista
                                 select pl;

            ArrayList produtos = new ArrayList(); //array para armazenar os produtos da lista
            ArrayList quantidade = new ArrayList(); //array para armazenar a quantidade de cada produto
            foreach (var prod in produtoDaLista)
            {
                produtos.Add(prod.id_produto); //--pegando produtos da lista
                quantidade.Add(prod.quantidade); //--pegando as quantidades
            }


            var itens = from i in dataContext.tb_Items //selec nos itens que estão contidos no determinado estabelecimento
                        where i.id_estabelecimento == idEstabelecimento
                        select i;


            foreach (var item in itens)
            {
                for (var i = 0; i < produtos.Count; i++)
                {
                    if (item.id_produto == Convert.ToInt32(produtos[i])) //se o item estiver na lista 
                    {
                        produtosEncontradosNoEstabelecimento++; //incrementa produtos encontrados
                        totalDaLista += Convert.ToInt32(item.preco) * Convert.ToInt32(quantidade[i]); //soma ao total da lista o valor do item multiplicado por sua quantidade;
                    }
                }
            }
            //paremetros(idEstab,nome,valorLista,produtosEncontrados,totalDeProdutos);
            cPrecoPorEstabelecimento estabelecimentos = new cPrecoPorEstabelecimento(idEstabelecimento,nome,totalDaLista, produtosEncontradosNoEstabelecimento,produtos.Count);//cria um objeto para retorno

            return estabelecimentos;
        }
    }
}