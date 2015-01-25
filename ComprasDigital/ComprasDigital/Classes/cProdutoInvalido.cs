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
			var produtosInvalidos = from pi in dataContext.tb_ProdutoInvalidos
									where pi.nome_produtoNovo == prodNovo.nome
										&& pi.marca_produtoNovo == prodNovo.marca
										&& pi.nome_produtoAntigo == prodAntigo.nome
										&& pi.marca_produtoAntigo == prodAntigo.marca
										&& pi.ocorrencia == ocorrencia
									select pi;

			tb_ProdutoInvalido prodInvalido;
			if (produtosInvalidos.Count() < 1)
			{
				prodInvalido = new tb_ProdutoInvalido();
				prodInvalido.nome_produtoNovo = prodNovo.nome;
				prodInvalido.marca_produtoNovo = prodNovo.marca;
				prodInvalido.nome_produtoAntigo = prodAntigo.nome;
				prodInvalido.marca_produtoAntigo = prodAntigo.marca;
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