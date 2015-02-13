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

			var produtos = from p in dataContext.tb_Produtos
						   where p.marca == cMarca.criarMarca(marca).id_marca &&
								p.nome == nome &&
								p.unidade == unidade &&
								p.embalagem == embalagem
						   select p;
			var produtoExistente = from p in produtos where p.codigoDeBarras == codigoDeBarras select p;
			if (produtoExistente.Count() == 1) //O produto já existe?
			{//Retorne-o
				cProdutoInvalido.verificarOcorrencias(produtoExistente.SingleOrDefault());
				return produtoExistente.SingleOrDefault();
			}
			else
			{
				var codigo = from p in dataContext.tb_Produtos where p.codigoDeBarras == codigoDeBarras select p;
				if (codigo.Count() > 0)//O codigo já existe?
				{//O codigo JÁ existe!
					tb_Produto produtoComCodigo = dataContext.tb_Produtos.Single(p => p.codigoDeBarras == codigoDeBarras);
					if (produtos.Count() > 0)//O produto já existe?
					{//O produto JÁ existe
						if((from p in produtos where p.codigoDeBarras == null select p).Count() == 1
						&& (from p in produtos where p.codigoDeBarras != null select p).Count() >= 1)
						{//caso tenha la algum conflito de codigo de barras
							//ex.: Variedades Nestlê com código e Variedades Nestlê sem código (por ter dado conflito com outro produto)
							novoProduto = produtos.First(p => p.codigoDeBarras == null);
							cProdutoInvalido.verificarOcorrencias(novoProduto, codigoDeBarras);
							return novoProduto;
						}
						else
						{
							tb_Produto produtoJaExistente = produtos.SingleOrDefault();
							if (produtoJaExistente.codigoDeBarras != null)//Tem código?
							{//TEM código
								novoProduto = new tb_Produto();
								novoProduto.marca = cMarca.criarMarca(marca).id_marca;
								novoProduto.nome = nome;
								novoProduto.embalagem = embalagem;
								novoProduto.unidade = unidade;
								dataContext.tb_Produtos.InsertOnSubmit(novoProduto);
								dataContext.SubmitChanges();
								novoProduto = dataContext.tb_Produtos.Single(p => p.unidade == unidade && p.nome == nome && p.tb_Marca.marca == marca && p.embalagem == embalagem && p.codigoDeBarras == null);
								cProdutoInvalido.criarProdutoInvalido(produtoJaExistente, novoProduto, (int)Ocorrencia.CodigoDiferente);
								cProdutoInvalido.criarProdutoInvalido(produtoComCodigo, novoProduto, (int)Ocorrencia.CodigoJaExistente);
								return novoProduto;
							}
							else
							{//NÃO tem código
								cProdutoInvalido.verificarOcorrencias(produtoJaExistente, codigoDeBarras);
								cProdutoInvalido.criarProdutoInvalido(produtoComCodigo, produtoJaExistente, (int)Ocorrencia.CodigoJaExistente);
								return produtoJaExistente;
							}
						}
					}
					else
					{//O produto NÃO existe
						novoProduto = new tb_Produto();
						novoProduto.marca = cMarca.criarMarca(marca).id_marca;
						novoProduto.nome = nome;
						novoProduto.embalagem = embalagem;
						novoProduto.unidade = unidade;
						dataContext.tb_Produtos.InsertOnSubmit(novoProduto);
						dataContext.SubmitChanges();
						novoProduto = dataContext.tb_Produtos.Single(p => p.unidade == unidade && p.nome == nome && p.tb_Marca.marca == marca && p.embalagem == embalagem && p.codigoDeBarras == null);
						cProdutoInvalido.criarProdutoInvalido(produtoComCodigo, novoProduto, (int)Ocorrencia.CodigoJaExistente);
						return novoProduto;
					}
				}
				else
				{//O codigo NÃO existe!
					if (produtos.Count() > 0)//O produto já existe?
					{//O produto JÁ existe
						tb_Produto produtoJaExistente;
						if ((from p in produtos where p.codigoDeBarras == null select p).Count() == 1
						&& (from p in produtos where p.codigoDeBarras != null select p).Count() >= 1)
						{//caso tenha la algum conflito de codigo de barras
							//ex.: Variedades Nestlê com código e Variedades Nestlê sem código (por ter dado conflito com outro produto)
							produtoJaExistente = produtos.First(p => p.codigoDeBarras != null);
						}
						else
						{
							produtoJaExistente = produtos.SingleOrDefault();
						}
						if (produtoJaExistente.codigoDeBarras != null)//Tem código?
						{//TEM código
							novoProduto = new tb_Produto();
							novoProduto.marca = cMarca.criarMarca(marca).id_marca;
							novoProduto.nome = nome;
							novoProduto.embalagem = embalagem;
							novoProduto.unidade = unidade;
							novoProduto.codigoDeBarras = codigoDeBarras;
							novoProduto.tipoCodigoDeBarras = tipoCodigoDeBarras;
							dataContext.tb_Produtos.InsertOnSubmit(novoProduto);
							dataContext.SubmitChanges();
							novoProduto = dataContext.tb_Produtos.Single(p => p.unidade == unidade && p.nome == nome && p.tb_Marca.marca == marca && p.embalagem == embalagem && p.codigoDeBarras == codigoDeBarras);
							cProdutoInvalido.criarProdutoInvalido(produtoJaExistente, novoProduto, (int)Ocorrencia.CodigoDiferente);
							return novoProduto;
						}
						else
						{//NÃO tem código
							cProdutoInvalido.verificarOcorrencias(produtoJaExistente);
							produtoJaExistente.codigoDeBarras = codigoDeBarras;
							produtoJaExistente.tipoCodigoDeBarras = tipoCodigoDeBarras;
							dataContext.SubmitChanges();
							return produtoJaExistente;
						}
					}
					else
					{//O produto NÃO existe
						novoProduto = new tb_Produto();
						novoProduto.marca = cMarca.criarMarca(marca).id_marca;
						novoProduto.nome = nome;
						novoProduto.embalagem = embalagem;
						novoProduto.unidade = unidade;
						novoProduto.codigoDeBarras = codigoDeBarras;
						novoProduto.tipoCodigoDeBarras = tipoCodigoDeBarras;
						dataContext.tb_Produtos.InsertOnSubmit(novoProduto);
						dataContext.SubmitChanges();
						novoProduto = dataContext.tb_Produtos.Single(p => p.unidade == unidade && p.nome == nome && p.tb_Marca.marca == marca && p.embalagem == embalagem && p.codigoDeBarras == codigoDeBarras);
						return novoProduto;
					}
				}
			}
		}
	}
}