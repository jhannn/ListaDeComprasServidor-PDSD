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
									where pi.id_produtoAntigo == prodAntigo.id_produto && pi.id_produtoNovo == prodNovo.id_produto && pi.ocorrencia == ocorrencia
									select pi;
			
			if (produtosInvalidos.Count() < 1)
			{
				tb_ProdutoInvalido prodInvalido;
				prodInvalido = new tb_ProdutoInvalido();
				prodInvalido.ocorrencia = ocorrencia;
				prodInvalido.quantidadeDeOcorrencias = 3; //de 0(antigo++) a 6(novo++)
				prodInvalido.id_produtoAntigo = prodAntigo.id_produto;
				prodInvalido.id_produtoNovo = prodNovo.id_produto;
				dataContext.tb_ProdutoInvalidos.InsertOnSubmit(prodInvalido);
				dataContext.SubmitChanges();
			}

			//falta colocar o gatilho no banco para resolver o problema!
			//quando atribuir o novo código, tem que verificar se ñ ta dando conflito com algum produto ja existente com código diferente.
			//quando mesclar produtos, tem que ver se o produto mesclado ñ ja tinha conflito com algum outro.
			//quando mesclar produtos por causa da unidade, colocar a unidade nova no que tiver código de barras e excluir os itens dos outros.
		}

		public static void verificarOcorrencias(tb_Produto produto, string codigo = "erro")
		{
			var dataContext = new DataClassesDataContext();
			var produtosInvalidos = from pi in dataContext.tb_ProdutoInvalidos where pi.id_produtoAntigo == produto.id_produto || pi.id_produtoNovo == produto.id_produto select pi;
			int idProdutoCodigo = -1;
			if (codigo != "erro")
				idProdutoCodigo = (from p in dataContext.tb_Produtos where p.codigoDeBarras == codigo select p).SingleOrDefault().id_produto;

			if (produtosInvalidos.Count() >= 1)
			{
				if (produto.codigoDeBarras != null)
				{
					foreach (var item in produtosInvalidos)
					{
						if (item.id_produtoAntigo == produto.id_produto)
							item.quantidadeDeOcorrencias -= 1;
						else//(item.id_produtoNovo == produto.id_produto)
							item.quantidadeDeOcorrencias += 1;
						dataContext.SubmitChanges();
					}
				}
				else
				{
					foreach (var item in produtosInvalidos)
					{
						if (item.ocorrencia == (int)Ocorrencia.UnidadeDiferente)
						{
							if (item.id_produtoAntigo == produto.id_produto)
								item.quantidadeDeOcorrencias -= 1;
							else//(item.id_produtoNovo == produto.id_produto)
								item.quantidadeDeOcorrencias += 1;
							dataContext.SubmitChanges();
						}
						else if (item.ocorrencia == (int)Ocorrencia.CodigoJaExistente && idProdutoCodigo > 0)
						{
							if (item.id_produtoAntigo == produto.id_produto && item.id_produtoNovo == idProdutoCodigo)
								item.quantidadeDeOcorrencias -= 1;
							else if(item.id_produtoNovo == produto.id_produto && item.id_produtoAntigo == idProdutoCodigo)
								item.quantidadeDeOcorrencias += 1;
							dataContext.SubmitChanges();
						}
					}
				}
			}
		}

		public static void qualificarProduto(int idProduto)
		{
			DataClassesDataContext dataContext = new DataClassesDataContext();
			var produtosInvalidos = from p in dataContext.tb_ProdutoInvalidos where p.id_produtoAntigo == idProduto || p.id_produtoNovo == idProduto select p;
			foreach (tb_ProdutoInvalido p in produtosInvalidos)
			{
				if (p.id_produtoNovo == idProduto)
					p.quantidadeDeOcorrencias++;
				else
					p.quantidadeDeOcorrencias--;
					dataContext.tb_ProdutoInvalidos.InsertOnSubmit(p);
				dataContext.SubmitChanges();
			}
		}
	}
}