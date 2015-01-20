﻿using System;
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

			var dataContext = new Model.EntidadesSistemaDeCompras();
			var listasDoUsuario = from l in dataContext.tb_ListaDeProdutos where l.id_usuario == idUsuario && l.nome.ToLower().StartsWith(nomeLista.ToLower()) select l;
			if (listasDoUsuario.Count() > 1) nomeLista += "_" + (listasDoUsuario.Count() + 1);
			//ArrayList listas = new ArrayList();
			//foreach (var list in listasDoUsuario)
			//	listas.Add(new cListaDeProdutos(list.id_listaDeProdutos, list.nome));
			//tb_ListaDeProdutos novaLista = new tb_ListaDeProdutos { id_usuario = idUsuario, nome = nomeLista };
			Model.tb_ListaDeProdutos novaLista = new Model.tb_ListaDeProdutos();
			novaLista.id_usuario = idUsuario;
			novaLista.nome = nomeLista;
			dataContext.tb_ListaDeProdutos.InsertOnSubmit(novaLista);
			dataContext.SubmitChanges();
			return js.Serialize(novaLista); // ta dando erro!
        }

		//___________________________________Editar Lista__________________________________//
		[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
		[WebMethod]
		public string editarNomeLista(int idLista, string novoNomeDaLista, int idUsuario, string token)
		{
			int resultado = 0;
			JavaScriptSerializer js = new JavaScriptSerializer();

			if (!cUsuario.usuarioValido(idUsuario, token))
				return js.Serialize("-1"); //retorna o id -1

			String ConexaoBanco = ConfigurationManager.ConnectionStrings["BancoDeDados"].ConnectionString;
			using (SqlConnection conexao = new SqlConnection(ConexaoBanco))
			{
				conexao.Open();
				using (SqlCommand cmd = new SqlCommand("usp_editarNomeListaDeCompras", conexao)) //producer a ser executada
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("@idLista", idLista); //parametros
					cmd.Parameters.AddWithValue("@nome", novoNomeDaLista); //parametros
					cmd.Parameters.AddWithValue("@idUsuario", idUsuario); //parametros

					SqlParameter returnValue = new SqlParameter(); //variavel para salvar o retorno
					returnValue.Direction = ParameterDirection.ReturnValue;
					cmd.Parameters.Add(returnValue);

					cmd.ExecuteNonQuery();
					resultado = (Int32)returnValue.Value; //atribuição do resultado de retorno a variavel resultado
				}
			}

			return js.Serialize(resultado);
		}

		//_______________________________________ RETORNAR NOME LISTA _______________________________________//
		[WebMethod]
		public string retornarNomeLista(int idLista)
		{
			JavaScriptSerializer js = new JavaScriptSerializer();
			string nomeLista = "";

			String ConexaoBanco = ConfigurationManager.ConnectionStrings["BancoDeDados"].ConnectionString;
			SqlConnection conexao = new SqlConnection(ConexaoBanco);
			SqlCommand cmd = new SqlCommand();
			SqlDataReader reader;


			//SQL "injector" 
			cmd.CommandText = "SELECT nome FROM tb_ListaDeProdutos WHERE id_listaDeProdutos = '" + idLista + "'";
			cmd.CommandType = CommandType.Text;
			cmd.Connection = conexao;

			conexao.Open();

			reader = cmd.ExecuteReader();

			while (reader.Read())
			{
				nomeLista = reader["nome"].ToString();	
			}

			conexao.Close();

			return js.Serialize(nomeLista);
		}


        //_______________________________________ RETORNAR LISTAS ___________________________________________//
        [WebMethod]
        public string retornarListas(int idUsuario)
        {
			JavaScriptSerializer js = new JavaScriptSerializer();
			List<cListaDeProdutos> listas = new List<cListaDeProdutos>();

            String ConexaoBanco = ConfigurationManager.ConnectionStrings["BancoDeDados"].ConnectionString;
            SqlConnection conexao = new SqlConnection(ConexaoBanco);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;


            //SQL "injector" 
            cmd.CommandText = "SELECT nome,id_listaDeProdutos FROM tb_ListaDeProdutos WHERE id_usuario = '" + idUsuario + "'";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = conexao;

            conexao.Open();

            reader = cmd.ExecuteReader();

            while (reader.Read())
            {
				listas.Add(new cListaDeProdutos(Convert.ToInt32(reader["id_listaDeProdutos"]),
												reader["nome"].ToString()) );
            }

			conexao.Close();

            return js.Serialize(listas);
        }

        //___________________________________ EXCLUIR LISTA _____________________________________________//
		[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        [WebMethod]
        public string excluirLista(int idLista, int idUsuario, string token)
        {
			int resultado = 0;
			JavaScriptSerializer js = new JavaScriptSerializer();

			if (!cUsuario.usuarioValido(idUsuario, token))
				return js.Serialize("-1"); //retorna o id -1

			String ConexaoBanco = ConfigurationManager.ConnectionStrings["BancoDeDados"].ConnectionString;
			using (SqlConnection conexao = new SqlConnection(ConexaoBanco))
			{
				conexao.Open();
				using (SqlCommand cmd = new SqlCommand("usp_excluirListaDeCompras", conexao)) //producer a ser executada
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("@idLista", idLista); //parametros
					cmd.Parameters.AddWithValue("@idUsuario", idUsuario); //parametros

					SqlParameter returnValue = new SqlParameter(); //variavel para salvar o retorno
					returnValue.Direction = ParameterDirection.ReturnValue;
					cmd.Parameters.Add(returnValue);

					cmd.ExecuteNonQuery();
					resultado = (Int32)returnValue.Value; //atribuição do resultado de retorno a variavel resultado
				}
			}

			return js.Serialize(resultado);
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
			cmd.CommandText = "SELECT p.nome, p.id_produto, pl.quantidade FROM tb_Produto AS p INNER JOIN tb_ProdutoDaLista AS pl ON pl.id_listaP = '" + idLista + "' AND pl.id_produto = p.id_produto";
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
