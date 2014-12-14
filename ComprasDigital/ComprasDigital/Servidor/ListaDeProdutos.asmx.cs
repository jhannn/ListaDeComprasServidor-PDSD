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
        public string criarLista(string nomeLista, int idUsuario, string token)
        {
            int resultado = 0;

            String ConexaoBanco = ConfigurationManager.ConnectionStrings["BancoDeDados"].ConnectionString;
            using (SqlConnection conexao = new SqlConnection(ConexaoBanco))
            {
                conexao.Open();
                using (SqlCommand cmd = new SqlCommand("usp_criarListaDeCompras", conexao)) //producer a ser executada
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@nomeLista", nomeLista); //parametros
                    cmd.Parameters.AddWithValue("@idUsuario", idUsuario); //parametros
                    cmd.Parameters.AddWithValue("@token", token); //parametros

                    SqlParameter returnValue = new SqlParameter(); //variavel para salvar o retorno
                    returnValue.Direction = ParameterDirection.ReturnValue;
                    cmd.Parameters.Add(returnValue);

                    cmd.ExecuteNonQuery();
                    resultado = (Int32)returnValue.Value; //atribuição do resultado de retorno a variavel resultado
                }
            }

            JavaScriptSerializer js = new JavaScriptSerializer();//O JavaScriptSerializer vai fazer o web service retornar JSON

            if (resultado == -1)//algum erro ocorreu
            {
                return js.Serialize("-1"); //retorna o id -1
            }
            else //tudo ok
                return js.Serialize(resultado.ToString()); //Converte e retorna os dados em JSON do id da lista
        }

        //___________________________________ EXCLUIR LISTA _____________________________________________//
        [WebMethod]
        public void excluirLista(int idLista, int idUsuario)
        {
            String ConexaoBanco = ConfigurationManager.ConnectionStrings["BancoDeDados"].ConnectionString;
            SqlConnection conexao = new SqlConnection(ConexaoBanco);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;


            //SQL "injector" 
            cmd.CommandText = "DELETE tb_ListaDeProdutos WHERE id_listaDeProdutos = '" + idLista + "' AND id_usuario = '" + idUsuario + "'";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = conexao;

            conexao.Open();
            reader = cmd.ExecuteReader();
			conexao.Close();
        }

		//_______________________________________ CADASTRAR PRODUTOS NA LISTA ___________________________________________//
		[WebMethod]
		public string cadastrarProdutos(string produtoJson, int quantidade, int idLista)
		{
			int resultado = 0;
			JavaScriptSerializer js = new JavaScriptSerializer();
			cProduto produto = js.Deserialize<cProduto>(produtoJson);

			String ConexaoBanco = ConfigurationManager.ConnectionStrings["BancoDeDados"].ConnectionString;
			using (SqlConnection conexao = new SqlConnection(ConexaoBanco))
			{
				conexao.Open();
				using (SqlCommand cmd = new SqlCommand("usp_criarProduto", conexao)) //producer a ser executada
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("@idLista", idLista); //parametros
					cmd.Parameters.AddWithValue("@nomeProduto", produto.nome); //parametros
					cmd.Parameters.AddWithValue("@codigoDeBarras", produto.codigoDeBarras); //parametros
					cmd.Parameters.AddWithValue("@tipoCodigo", produto.tipoCodigo); //parametros
					cmd.Parameters.AddWithValue("@quantidade", idLista); //parametros

					SqlParameter returnValue = new SqlParameter(); //variavel para salvar o retorno
					returnValue.Direction = ParameterDirection.ReturnValue;
					cmd.Parameters.Add(returnValue);

					cmd.ExecuteNonQuery();
					resultado = (Int32)returnValue.Value; //atribuição do resultado de retorno a variavel resultado
				}
			}

			return js.Serialize(resultado);
		}


		//_______________________________________ EXCLUIR PRODUTO DA LISTA ___________________________________________//
		[WebMethod]
		public void removerProduto(int idProduto, int idLista)
		{
			String ConexaoBanco = ConfigurationManager.ConnectionStrings["BancoDeDados"].ConnectionString;
			using (SqlConnection conexao = new SqlConnection(ConexaoBanco))
			{
				conexao.Open();
				using (SqlCommand cmd = new SqlCommand("usp_removerProdutoDaLista", conexao)) //producer a ser executada
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("@idLista", idLista); //parametros
					cmd.Parameters.AddWithValue("@idProduto", idProduto); //parametros

					cmd.ExecuteNonQuery();
				}
			}
		}


		//_______________________________________ LISTAR PRODUTOS DA LISTA ___________________________________________//
		[WebMethod]
		public string listarProdutosDaLista(int idLista)
		{
			JavaScriptSerializer js = new JavaScriptSerializer();
			List<cProduto> produtos = new List<cProduto>();

			String ConexaoBanco = ConfigurationManager.ConnectionStrings["BancoDeDados"].ConnectionString;
			SqlConnection conexao = new SqlConnection(ConexaoBanco);
			SqlCommand cmd = new SqlCommand();
			SqlDataReader reader;


			//SQL "injector" 
			cmd.CommandText = "SELECT p.nome, p.id_produto, pl.quantidade FROM tb_Produto AS p INNER JOIN tb_ProdutoDaLista AS pl ON pl.id_lista = '" + idLista + "' AND pl.id_produto = p.id_produto";
			cmd.CommandType = CommandType.Text;
			cmd.Connection = conexao;

			conexao.Open();
			reader = cmd.ExecuteReader();
			while (reader.Read())
			{
				produtos.Add(new cProduto(Convert.ToInt32(reader["id_produto"]),
											reader["nome"].ToString(),
											Convert.ToInt32(reader["quantidade"])));
			}
			conexao.Close();

			return js.Serialize(produtos);
		}
    }
}
