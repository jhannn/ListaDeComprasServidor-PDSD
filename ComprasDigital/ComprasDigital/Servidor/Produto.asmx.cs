using System;
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
using System.Collections;

namespace ComprasDigital.Servidor
{
    /// <summary>
    /// Summary description for Produto
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class Produto : System.Web.Services.WebService
    {
        //_______________________________________ AUTOCOMPLETE ___________________________________________//
        [WebMethod]
        public string autocomplete(string nome)
        {
            var produtos = new List<string>();

            String ConexaoBanco = ConfigurationManager.ConnectionStrings["BancoDeDados"].ConnectionString;
            SqlConnection conexao = new SqlConnection(ConexaoBanco);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

				//SQL "injector" 
				cmd.CommandText = "SELECT id_produto, nome FROM tb_Produto WHERE nome LIKE '"+ nome + "%'";
				cmd.CommandType = CommandType.Text;
				cmd.Connection = conexao;

				conexao.Open();

				reader = cmd.ExecuteReader();
				while (reader.Read())
				{
					produtos.Add(reader["nome"].ToString());
				}
				conexao.Close();
				JavaScriptSerializer js = new JavaScriptSerializer();
				return js.Serialize(produtos);
        }

        //_______________________________________ RETORNAR PRODUTOS ___________________________________________//
        [WebMethod]
        public string retornarProdutos(int idUsuario,string token,string nome,int marca)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            if (!cUsuario.usuarioValido(idUsuario, token))
                return js.Serialize(new UsuarioNaoLogadoException()); //retorna a exception UsuarioNaoLogado

            var dataContext = new Model.DataClassesDataContext();
            var produtos = from p in dataContext.tb_Produtos where p.nome.Contains(nome) select p;

            if (produtos.Count() < 1) return js.Serialize("new ProdutoNaoEncontradoException()");

            ArrayList listasDeProdutos = new ArrayList();
            foreach (var prod in produtos)
                listasDeProdutos.Add(prod);

            return js.Serialize(listasDeProdutos);
        }

        //_______________________________________ PESQUISAR PRODUTOS ___________________________________________//
        [WebMethod]
        public string pesquisarProdutos()
        {
            return "";
        }

    }
}

