using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ComprasDigital.Model;

namespace ComprasDigital.Classes
{
	public class cProduto
	{
		public string marca { get; set; }
		public string nome { get; set; }
		public string codigoDeBarras { get; set; }
		public string tipoCodigoDeBarras { get; set; }
		public string tipo { get; set; }
		public string unidade { get; set; }

		public cProduto() { }

		public cProduto(tb_Produto prod)
		{
			marca = prod.tb_Marca.marca;
			codigoDeBarras = prod.codigoDeBarras;
			nome = prod.nome;
			tipo = prod.tb_Tipo.tipo;
			tipoCodigoDeBarras = prod.tipoCodigoDeBarras;
			unidade = prod.tb_Unidade.unidade;
		}

		//sem codigo de barras
		public static tb_Produto criarProduto(string marca, string nome, int tipo, int unidade)
		{
			nome = nome.ToLower();
			if (marca == "") marca = "sem marca";
			else marca = marca.ToLower();
			var dataContext = new DataClassesDataContext();
			var produtos = from p in dataContext.tb_Produtos where p.nome == nome && p.tb_Marca.marca == marca select p;

			tb_Produto novoProduto;
			if (produtos.Count() < 1)
			{/*produto não existe*/
				novoProduto = new tb_Produto();
				novoProduto.nome = nome;
				novoProduto.tipo = tipo;
				novoProduto.unidade = unidade;
				novoProduto.marca = cMarca.criarMarca(marca).id_marca;
				dataContext.tb_Produtos.InsertOnSubmit(novoProduto);
				dataContext.SubmitChanges();
			}
			else
			{/*ja tem algum produto como esse*/
				var prod = from p in produtos where p.unidade == unidade && p.tipo == tipo select p;
				if (prod.Count() == 1)
				{/*produto ja existe*/
					return prod.FirstOrDefault();
				}
				else
				{/*produto está com unidade ou tipo diferentes*/

				}
			}

			return (from p in dataContext.tb_Produtos where p.nome == nome && p.tb_Marca.marca == marca orderby p.nome, p.tb_Marca.marca select p).FirstOrDefault();
		}

		//com codigo de barras
		public static tb_Produto criarProduto(string marca, string nome, string codigoDeBarras, string tipoCodigoDeBarras, int tipo, int unidade)
		{
			nome = nome.ToLower();
			if (marca == "") marca = "sem marca";
			else marca = marca.ToLower();
			var dataContext = new DataClassesDataContext();
			tb_Produto novoProduto;

			var codigo = from p in dataContext.tb_Produtos where p.codigoDeBarras == codigoDeBarras && p.tipoCodigoDeBarras == tipoCodigoDeBarras select p;
			if (codigo.Count() < 1)
			{/*codigo de barras não existe*/
				var produtos = from p in dataContext.tb_Produtos where p.nome == nome && p.tb_Marca.marca == marca select p;
				if (produtos.Count() < 1)
				{/*produto não existe*/
					novoProduto = new tb_Produto();
					novoProduto.nome = nome;
					novoProduto.tipo = tipo;
					novoProduto.unidade = unidade;
					novoProduto.codigoDeBarras = codigoDeBarras;
					novoProduto.tipoCodigoDeBarras = tipoCodigoDeBarras;
					novoProduto.marca = cMarca.criarMarca(marca).id_marca;
					dataContext.tb_Produtos.InsertOnSubmit(novoProduto);
					dataContext.SubmitChanges();
				}
				else
				{/*ja tem algum produto como esse*/
					var prod = from p in produtos where p.unidade == unidade && p.tipo == tipo select p;
					if (prod.Count() == 1)
					{/*produto ja existe*/
						if (prod.SingleOrDefault().codigoDeBarras == null)
						{/*mas não tem código de barras*/
							novoProduto = dataContext.tb_Produtos.Single(p => p.nome == nome && p.tb_Marca.marca == marca);
							novoProduto.codigoDeBarras = codigoDeBarras;
							novoProduto.tipoCodigoDeBarras = tipoCodigoDeBarras;
							dataContext.SubmitChanges();
						}
						else
						{/*mas com um codigo de barras diferente*/

						}
						return prod.FirstOrDefault();
					}
					else
					{/*produto está com unidade ou tipo diferentes*/

					}
				}
			}
			else
			{/*codigo ja existe*/
				var produtos = from p in dataContext.tb_Produtos where p.nome == nome && p.tb_Marca.marca == marca select p;
				if (produtos.Count() < 1)
				{/*produto não existe*/
					novoProduto = new tb_Produto();
					novoProduto.nome = nome;
					novoProduto.tipo = tipo;
					novoProduto.unidade = unidade;
					novoProduto.marca = cMarca.criarMarca(marca).id_marca;
					dataContext.tb_Produtos.InsertOnSubmit(novoProduto);
					dataContext.SubmitChanges();

					cProdutoInvalido.criarProdutoInvalido(codigo.FirstOrDefault(), novoProduto, (int)Ocorrencia.CodigoJaExistente);
				}//-------------------------------------------------- parei aqui ------------------------------------------------------
				else
				{/*ja tem algum produto como esse*/
					var prod = from p in produtos where p.unidade == unidade && p.tipo == tipo select p;
					if (prod.Count() == 1)
					{/*produto ja existe*/
						if (prod.SingleOrDefault().codigoDeBarras == null)
						{/*mas não tem código de barras*/
							novoProduto = dataContext.tb_Produtos.Single(p => p.nome == nome && p.tb_Marca.marca == marca);
							novoProduto.codigoDeBarras = codigoDeBarras;
							novoProduto.tipoCodigoDeBarras = tipoCodigoDeBarras;
							dataContext.SubmitChanges();
						}
						else
						{/*mas com um codigo de barras diferente*/

						}
						return prod.FirstOrDefault();
					}
					else
					{/*produto está com unidade ou tipo diferentes*/

					}
				}
			}

			return (from p in dataContext.tb_Produtos where p.nome == nome && p.tb_Marca.marca == marca orderby p.nome, p.tb_Marca.marca select p).FirstOrDefault();
		}
	}
}