using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Configuration;
using System.Web.Script.Serialization;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using System.Diagnostics;
using System.Web.Script.Services;
using ComprasDigital.Classes;
using ComprasDigital.Excecoes;


namespace ComprasDigital.Servidor
{
    /// <summary>
    /// Summary description for ListaDeProdutos
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class ListaDeProdutos : System.Web.Services.WebService
    {

        //______________________________ CRIAR LISTA __________________________________//
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        [WebMethod]
        public string criarLista(int idUsuario, string token, string nomeLista)
        {
			JavaScriptSerializer js = new JavaScriptSerializer();//O JavaScriptSerializer vai fazer o web service retornar JSON

			if(!cUsuario.usuarioValido(idUsuario, token))
				return js.Serialize(new UsuarioNaoLogadoException()); //retorna a exception UsuarioNaoLogado

			var dataContext = new Model.DataClassesDataContext();
			int quant = 0;
			if (((from l in dataContext.tb_ListaDeProdutos where l.id_usuario == idUsuario && l.nome.ToLower() == nomeLista.ToLower() select l).Count() == 1) &&
				((quant = (from l in dataContext.tb_ListaDeProdutos where l.id_usuario == idUsuario && l.nome.ToLower().StartsWith(nomeLista.ToLower()) select l).Count()) > 1)) nomeLista += "-" + (quant + 1);
		
			Model.tb_ListaDeProduto novaLista = new Model.tb_ListaDeProduto();
			novaLista.id_usuario = idUsuario;
			novaLista.nome = nomeLista;
			dataContext.tb_ListaDeProdutos.InsertOnSubmit(novaLista);
			dataContext.SubmitChanges();
			return js.Serialize("precisa pegar o ID da lista criada");
        }

		//___________________________________ EDITAR LISTA __________________________________//
		[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
		[WebMethod]
		public string editarNomeLista(int idUsuario, string token, int idLista, string novoNomeDaLista)
		{
			JavaScriptSerializer js = new JavaScriptSerializer();

			if (!cUsuario.usuarioValido(idUsuario, token))
				return js.Serialize(new UsuarioNaoLogadoException());

            var dataContext = new Model.DataClassesDataContext();
            var queryLista = (from l in dataContext.tb_ListaDeProdutos 
                             where l.id_listaDeProdutos == idLista 
                             select l).Single();

            queryLista.nome = novoNomeDaLista;
            dataContext.SubmitChanges();
           
			return js.Serialize("Operação realizada com sucesso.");
		}

		//_______________________________________ RETORNAR NOME LISTA _______________________________________//
		[WebMethod]
		public string retornarNomeLista(int idUsuario,string token,int idListaDeProdutos)
		{
            JavaScriptSerializer js = new JavaScriptSerializer();

            if (!cUsuario.usuarioValido(idUsuario, token))
                return js.Serialize(new UsuarioNaoLogadoException()); //retorna a exception UsuarioNaoLogado

            var dataContext = new Model.DataClassesDataContext();
            var nomeLista = from l in dataContext.tb_ListaDeProdutos where l.id_listaDeProdutos == idListaDeProdutos select l;
           
            return js.Serialize(nomeLista.SingleOrDefault().nome);
		}


        //_______________________________________ RETORNAR LISTAS ___________________________________________//
        [WebMethod]
        public string retornarListas(int idUsuario,string token)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            if (!cUsuario.usuarioValido(idUsuario, token))
                return js.Serialize(new UsuarioNaoLogadoException()); //retorna a exception UsuarioNaoLogado

            List<cListaDeProdutos> listas = new List<cListaDeProdutos>();

            var dataContext = new Model.DataClassesDataContext();
            var selectListas = from l in dataContext.tb_ListaDeProdutos where l.id_usuario == idUsuario select l;

            foreach(var list in selectListas)
            {
                listas.Add( new cListaDeProdutos(list.id_listaDeProdutos,list.nome) );
            }

            return js.Serialize(listas);
        }

        //___________________________________ REMOVER LISTA _____________________________________________//
		[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        [WebMethod]
        public string removerLista(int idUsuario,string token,int idLista)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            if(!cUsuario.usuarioValido(idUsuario,token))
                return js.Serialize(new UsuarioNaoLogadoException());

            var dataContext = new Model.DataClassesDataContext();

            //======== Deletando produtos da lista ===========//
            var queryProduto = from p in dataContext.tb_ProdutoDaListas
                               where p.id_lista == idLista
                               select p;

            foreach (var prod in queryProduto)
            {
                dataContext.tb_ProdutoDaListas.DeleteOnSubmit(prod);
                dataContext.SubmitChanges();
            }

            //============ Deletando a lista =============//
            var queryLista = from l in dataContext.tb_ListaDeProdutos
                             where l.id_listaDeProdutos == idLista
                             select l;

            foreach (var list in queryLista)
            {
                dataContext.tb_ListaDeProdutos.DeleteOnSubmit(list);
                dataContext.SubmitChanges();
            }
    
            return js.Serialize("Lista deletada com sucesso.");
		}

		//_______________________________________ CADASTRAR PRODUTOS NA LISTA ___________________________________________//
		[WebMethod]
		public string cadastrarProduto(string nomeProduto, string codigoDeBarras, string tipoCodigo, int quantidade, int idLista)
		{
			int resultado = 0;
			JavaScriptSerializer js = new JavaScriptSerializer();
			//cProduto produto = js.Deserialize<cProduto>(produtoJson);

			String ConexaoBanco = ConfigurationManager.ConnectionStrings["BancoDeDados"].ConnectionString;
			using (SqlConnection conexao = new SqlConnection(ConexaoBanco))
			{
				conexao.Open();
				using (SqlCommand cmd = new SqlCommand("usp_criarProduto", conexao)) //producer a ser executada
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("@idLista", idLista); //parametros
					cmd.Parameters.AddWithValue("@nomeProduto", nomeProduto); //parametros
					cmd.Parameters.AddWithValue("@codigoDeBarras", codigoDeBarras); //parametros
					cmd.Parameters.AddWithValue("@tipoCodigo", tipoCodigo); //parametros
					cmd.Parameters.AddWithValue("@quantidade", quantidade); //parametros

					SqlParameter returnValue = new SqlParameter(); //variavel para salvar o retorno
					returnValue.Direction = ParameterDirection.ReturnValue;
					cmd.Parameters.Add(returnValue);

					cmd.ExecuteNonQuery();
					resultado = (Int32)returnValue.Value; //atribuição do resultado de retorno a variavel resultado
				}
			}

			return js.Serialize(resultado);
		}


		//_______________________________________ REMOVER PRODUTO DA LISTA ___________________________________________//
		[WebMethod]
		public string removerProdutoDaLista(int idUsuario,string token,string nomeProduto)
		{
            JavaScriptSerializer js = new JavaScriptSerializer();

            if (!cUsuario.usuarioValido(idUsuario, token))
                return js.Serialize(new UsuarioNaoLogadoException());

            var dataContext = new Model.DataClassesDataContext();
            var querryProdutos = from p in dataContext.tb_ProdutoDaListas
                                 where p.nome_produto == nomeProduto
                                 select p;

            foreach(var prod in querryProdutos)
            {
                dataContext.tb_ProdutoDaListas.DeleteOnSubmit(prod);
                dataContext.SubmitChanges();
            }
            return js.Serialize("Produto da lista removido.");
		}


		//_______________________________________ RETORNAR PRODUTOS DA LISTA ___________________________________________//
		[WebMethod]
		public string retornarProdutosDaLista(int idUsuario,string token,int idLista)
		{
			JavaScriptSerializer js = new JavaScriptSerializer();

            if (!cUsuario.usuarioValido(idUsuario, token))
                return js.Serialize(new UsuarioNaoLogadoException());

            var dataContext = new Model.DataClassesDataContext();
            var queryProdutos = from p in dataContext.tb_ProdutoDaListas
                                where p.id_lista == idLista
                                select p;

            ArrayList produtos = new ArrayList();
            foreach(var prod in queryProdutos)
            {
                produtos.Add(new cProdutoDaLista(prod.nome_produto,prod.marca_produto,prod.id_lista,prod.quantidade));
            }

            return js.Serialize(produtos);
		}  
    }
}
