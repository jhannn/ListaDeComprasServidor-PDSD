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

        [WebMethod]
        public void cadastrarProduto(string codigoDeBarras, string nomeDoProduto, string formatoCodigoDeBarras)
        {

            //
        }

        //_______________________________________ RETORNAR PRODUTOS ___________________________________________//
        [WebMethod]
        public string retornarProdutos(string nome)
        {
            List<cProduto> produtos = new List<cProduto>();

            String ConexaoBanco = ConfigurationManager.ConnectionStrings["BancoDeDados"].ConnectionString;
            SqlConnection conexao = new SqlConnection(ConexaoBanco);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;


            //SQL "injector" 
            cmd.CommandText = "SELECT TOP 5 id_produto, nome FROM tb_Produto WHERE nome LIKE '%" + nome + "%'";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = conexao;

            conexao.Open();

            reader = cmd.ExecuteReader();

            while (reader.Read())
            {
				produtos.Add(new cProduto(Convert.ToInt32(reader["id_produto"]), reader["nome"].ToString()));
            }

            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(produtos);
        }

    }
}

