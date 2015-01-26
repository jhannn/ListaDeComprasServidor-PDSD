using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ComprasDigital.Model;

namespace ComprasDigital.Classes
{
	public class cProdutoInvalido
	{
		public cProdutoInvalido() { }

		public static void criarProdutoInvalido(tb_Produto prodAntigo, tb_Produto prodNovo, int ocorrencia)
		{
			var dataContext = new DataClassesDataContext();
			
			tb_ProdutoInvalido prodInvalido;
			if (produtosInvalidos.Count() < 1)
			{
				prodInvalido = new tb_ProdutoInvalido();
				
				prodInvalido.ocorrencia = ocorrencia;
				prodInvalido.quantidadeDeOcorrencias = 5; //de 0(antigo++) a 10(novo++)
				dataContext.tb_ProdutoInvalidos.InsertOnSubmit(prodInvalido);
				dataContext.SubmitChanges();
			}
			else
			{
				prodInvalido = dataContext.tb_ProdutoInvalidos.Single(pi => pi.nome_produtoNovo == prodNovo.nome && pi.marca_produtoNovo == prodNovo.marca && pi.nome_produtoAntigo == prodAntigo.nome && pi.marca_produtoAntigo == prodAntigo.marca);
				prodInvalido.quantidadeDeOcorrencias++;
				dataContext.SubmitChanges();
			}

			/*tem que fazer invertido tb... colocando o produto novo como antigo e o antigo como novo*/
		}
	}
}