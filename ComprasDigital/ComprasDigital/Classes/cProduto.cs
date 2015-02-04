using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ComprasDigital.Model;

namespace ComprasDigital.Classes
{
	public class cProduto
	{
		public int id_produto { get; set; }
		public string marca { get; set; }
		public string nome { get; set; }
		public string codigoDeBarras { get; set; }
		public string tipoCodigoDeBarras { get; set; }
		public string embalagem { get; set; }
		public string unidade { get; set; }

		public cProduto(tb_Produto prod)
		{
			marca = prod.tb_Marca.marca;
			codigoDeBarras = prod.codigoDeBarras;
			nome = prod.nome;
			id_produto = prod.id_produto;
			tipoCodigoDeBarras = prod.tipoCodigoDeBarras;
			unidade = prod.tb_Unidade.unidade;
			embalagem = prod.tb_Embalagem.embalagem;
		}



		private static void gravarProduto(string marca, string nome, int unidade, int embalagem, string codigoDeBarras = null, string tipoCodigoDeBarras = null)
		{
			var dataContext = new DataClassesDataContext();
			tb_Produto novoProduto = new tb_Produto();
			novoProduto.nome = nome;
			novoProduto.unidade = unidade;
			novoProduto.marca = cMarca.criarMarca(marca).id_marca;
			novoProduto.embalagem = embalagem;
			novoProduto.codigoDeBarras = codigoDeBarras;
			novoProduto.tipoCodigoDeBarras = tipoCodigoDeBarras;
			dataContext.tb_Produtos.InsertOnSubmit(novoProduto);
			dataContext.SubmitChanges();
		}



		//sem codigo de barras
		public static tb_Produto criarProduto(string marca, string nome, int unidade, int embalagem)
		{
			nome = nome.ToLower();
			marca = marca.ToLower();
			var dataContext = new DataClassesDataContext();
			var produtos = from p in dataContext.tb_Produtos where p.nome == nome && p.tb_Marca.marca == marca && p.embalagem == embalagem select p;

			tb_Produto novoProduto;
			if (produtos.Count() < 1)
			{/*produto não existe*/
				gravarProduto(marca, nome, unidade, embalagem);

				novoProduto = (from p in dataContext.tb_Produtos where p.nome == nome && p.tb_Marca.marca == marca && p.unidade == unidade && p.embalagem == embalagem orderby p.nome, p.tb_Marca.marca select p).FirstOrDefault();
			}
			else
			{/*ja tem algum produto como esse*/
				var prod = from p in produtos where p.unidade == unidade orderby p.codigoDeBarras select p;
				if (prod.Count() == 1)
				{/*produto ja existe*/
					tb_Produto retorno = prod.FirstOrDefault();
					cProdutoInvalido.verificarOcorrencias(retorno);
					return retorno;

				}
				else
				{/*produto está com unidade diferentes*/
					tb_Produto produtoAntigo = (from p in produtos orderby p.codigoDeBarras orderby p.id_produto select p).FirstOrDefault();
					gravarProduto(marca, nome, unidade, embalagem);
					novoProduto = (from p in dataContext.tb_Produtos where p.nome == nome && p.tb_Marca.marca == marca && p.unidade == unidade orderby p.nome, p.tb_Marca.marca select p).FirstOrDefault();
					cProdutoInvalido.criarProdutoInvalido(produtoAntigo, novoProduto, (int)Ocorrencia.UnidadeDiferente);
				}
			}

			return novoProduto;
		}



		//com codigo de barras
		public static tb_Produto criarProduto(string marca, string nome, int unidade, int embalagem, string codigoDeBarras, string tipoCodigoDeBarras)
		{
			nome = nome.ToLower();
			marca = marca.ToLower();
			var dataContext = new DataClassesDataContext();
			tb_Produto novoProduto;

			var codigo = from p in dataContext.tb_Produtos where p.codigoDeBarras == codigoDeBarras select p;

			if (codigo.Count() < 1)
			{//O código não existe
				var produtos = from p in dataContext.tb_Produtos where p.nome == nome && p.tb_Marca.marca == marca && p.embalagem == embalagem select p;

				if (produtos.Count() < 1)
				{//O produto não existe
					//cria novo produto
					gravarProduto(marca, nome, unidade, embalagem, codigoDeBarras, tipoCodigoDeBarras);
					novoProduto = (from p in dataContext.tb_Produtos where p.nome == nome && p.tb_Marca.marca == marca && p.unidade == unidade && p.embalagem == embalagem orderby p.nome, p.tb_Marca.marca select p).FirstOrDefault();
				}
				else
				{//ja tem algum produto como esse. checar se ele tem codigo
					var prodComCodigo = from p in produtos where p.codigoDeBarras != null orderby p.codigoDeBarras select p;
					var prodComUnidade = from p in produtos where p.unidade == unidade select p;
					if (prodComCodigo.Count() < 1)
					{//Nenhuma das versões do produto possuem código de barras
						//se tiver com a mesma unidade, coloca o código nele; se não, cria outro
						if(prodComUnidade.Count() < 1)
						{//cria um novo produto
							gravarProduto(marca, nome, unidade, embalagem, codigoDeBarras, tipoCodigoDeBarras);
						}
						else
						{//atualiza o antigo
							novoProduto = (prodComUnidade).SingleOrDefault();
							cProdutoInvalido.verificarOcorrencias(novoProduto);
							novoProduto.codigoDeBarras = codigoDeBarras;
							novoProduto.tipoCodigoDeBarras = tipoCodigoDeBarras;
							dataContext.SubmitChanges();
						}
					}
					else
					{//algum dos produtos tem código de barras
						//checa se o que tem código é a mesma unidade
						prodComUnidade = from p in prodComCodigo where p.unidade == unidade select p;
						if(prodComUnidade.Count() < 1)
						{//cria um novo produto
							gravarProduto(marca, nome, unidade, embalagem, codigoDeBarras, tipoCodigoDeBarras);
						}
						else
						{// é o mesmo produto, mas com código diferente
							//criar produto inválido: 2 codigos pra 1 produto
							tb_Produto produtoAntigo = (from p in prodComUnidade orderby p.codigoDeBarras select p).FirstOrDefault();
							gravarProduto(marca, nome, unidade, embalagem, codigoDeBarras, tipoCodigoDeBarras);
							novoProduto = (from p in dataContext.tb_Produtos where p.codigoDeBarras == codigoDeBarras orderby p.nome, p.tb_Marca.marca select p).FirstOrDefault();
							cProdutoInvalido.criarProdutoInvalido(produtoAntigo, novoProduto, (int)Ocorrencia.CodigoDiferente);
						}
					}
				}
				novoProduto = (from p in dataContext.tb_Produtos where p.codigoDeBarras == codigoDeBarras orderby p.nome, p.tb_Marca.marca select p).FirstOrDefault();
			}
			else
			{//o código de barras ja existe
				tb_Produto produtoAntigo;
				var produtos = from p in dataContext.tb_Produtos where p.nome == nome && p.tb_Marca.marca == marca && p.embalagem == embalagem select p;
				if(produtos.Count() < 1)
				{//o produto não existe
					//criar produto inválido: 1 código pra 2 produtos
					produtoAntigo = (codigo).FirstOrDefault();
					gravarProduto(marca, nome, unidade, embalagem);
					novoProduto = (from p in dataContext.tb_Produtos where p.codigoDeBarras == codigoDeBarras orderby p.nome, p.tb_Marca.marca select p).FirstOrDefault();
					cProdutoInvalido.criarProdutoInvalido(produtoAntigo, novoProduto, (int)Ocorrencia.CodigoJaExistente);
				}
				else
				{//o produto já existe
					var prodComCodigo = (from p in produtos where p.codigoDeBarras == codigoDeBarras select p).SingleOrDefault();
					var prodComUnidade = from p in produtos where p.unidade == unidade select p;
					//o código pertence a ele?
					if (prodComUnidade.Count() == 1)
					{//retorna o produto
						if (prodComUnidade.SingleOrDefault().id_produto == prodComCodigo.id_produto)
						{
							novoProduto = prodComUnidade.FirstOrDefault();
							cProdutoInvalido.verificarOcorrencias(novoProduto);
						}
						else
						{

						}
					}
					else
					{//apenas gera o produto inválido
						novoProduto = (prodComUnidade).FirstOrDefault();
						cProdutoInvalido.verificarOcorrencias(novoProduto);
						cProdutoInvalido.criarProdutoInvalido(produtoAntigo, novoProduto, (int)Ocorrencia.CodigoJaExistente);
					}

					produtoAntigo = prodComUnidade.FirstOrDefault();
					//existe com a mesma unidade?
					if (prodComUnidade.Count() < 1)
					{//a unidade existente é diferente
						//cadastra um novo e cria um produto inválido
						produtoAntigo = (codigo).FirstOrDefault();
						gravarProduto(marca, nome, unidade, embalagem);
						novoProduto = (from p in dataContext.tb_Produtos where p.codigoDeBarras == codigoDeBarras orderby p.nome, p.tb_Marca.marca select p).FirstOrDefault();
						cProdutoInvalido.criarProdutoInvalido(produtoAntigo, novoProduto, (int)Ocorrencia.CodigoJaExistente);
					}
					else
					{
					}
				}
			}

			return novoProduto;
		}
	}
}