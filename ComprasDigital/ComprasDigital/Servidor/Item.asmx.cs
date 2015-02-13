using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using ComprasDigital.Classes;
using System.Web.Script.Serialization;
using ComprasDigital.Excecoes.Usuario;

namespace ComprasDigital.Servidor
{
    /// <summary>
    /// Summary description for Item
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Item : System.Web.Services.WebService
	{
		//_______________________________________ PESQUISAR ITENS DE PRODUTO ___________________________________________//
		[WebMethod]
		public string pesquisarItens(int idUsuario, string token, int idProduto)
		{
			JavaScriptSerializer js = new JavaScriptSerializer();

			if (!cUsuario.usuarioValido(idUsuario, token))
				return js.Serialize(new UsuarioNaoLogadoException());

			var dataContext = new Model.DataClassesDataContext();
			List<cItem> itens = new List<cItem>();
			Model.tb_Produto produto = dataContext.tb_Produtos.First(p => p.id_produto == idProduto);
			var estabelecimentos = from e in dataContext.tb_Estabelecimentos select e;
			foreach (var estab in estabelecimentos)
				itens.Add(new cItem(produto));

			return js.Serialize(itens);
		}



		//_______________________________________ ITEM NO ESTABELECIMENTO ___________________________________________//
		[WebMethod]
		public string itemNoEstabelecimento(int idUsuario, string token, int idProduto, int idEstabelecimento)
		{
			JavaScriptSerializer js = new JavaScriptSerializer();

			if (!cUsuario.usuarioValido(idUsuario, token))
				return js.Serialize(new UsuarioNaoLogadoException());

			var dataContext = new Model.DataClassesDataContext();
			Model.tb_Produto produto = dataContext.tb_Produtos.First(p => p.id_produto == idProduto);
			Model.tb_Estabelecimento estabelecimento = dataContext.tb_Estabelecimentos.First(e => e.id_estabelecimento == idEstabelecimento);
			return js.Serialize(new cItem(produto, estabelecimento));
		}
    }
}
