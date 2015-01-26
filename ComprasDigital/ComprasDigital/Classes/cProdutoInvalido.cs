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
			var produtosInvalidos = from pi in dataContext.tb_ProdutoInvalidos where pi.id_produtoAntigo == prodAntigo.id_produto && pi.id_produtoNovo == prodNovo.id_produto && pi.ocorrencia == ocorrencia select pi;
			
			tb_ProdutoInvalido prodInvalido;
			if (produtosInvalidos.Count() == 1)
			{
				prodInvalido = dataContext.tb_ProdutoInvalidos.Single(pi => pi.id_produtoNovo == prodNovo.id_produto && pi.id_produtoAntigo == pi.id_produtoAntigo && pi.ocorrencia == ocorrencia);
				prodInvalido.quantidadeDeOcorrencias++;
				dataContext.SubmitChanges();
			}
			else
			{
				produtosInvalidos = from pi in dataContext.tb_ProdutoInvalidos where pi.id_produtoAntigo == prodNovo.id_produto && pi.id_produtoNovo == prodAntigo.id_produto && pi.ocorrencia == ocorrencia select pi;
				if (produtosInvalidos.Count() == 1)
				{
					prodInvalido = dataContext.tb_ProdutoInvalidos.Single(pi => pi.id_produtoAntigo == prodNovo.id_produto && pi.id_produtoNovo == prodAntigo.id_produto && pi.ocorrencia == ocorrencia);
					prodInvalido.quantidadeDeOcorrencias--;
					dataContext.SubmitChanges();
				}
				else
				{
					prodInvalido = new tb_ProdutoInvalido();
					prodInvalido.ocorrencia = ocorrencia;
					prodInvalido.quantidadeDeOcorrencias = 3; //de 0(antigo++) a 6(novo++)
					dataContext.tb_ProdutoInvalidos.InsertOnSubmit(prodInvalido);
					dataContext.SubmitChanges();
				}
			}

			//falta colocar o gatilho no banco para resolver o problema!
		}
	}
}