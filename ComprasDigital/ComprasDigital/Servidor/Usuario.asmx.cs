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
using System.Web.Security; //pacote de seguraça do ASP.NET
using ComprasDigital.Classes;
using System.Net.Mail;
using System.Net;


namespace ComprasDigital.Servidor
{
    /// <summary>
    /// Summary description for Usuario
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class Usuario : System.Web.Services.WebService
    {


        //_______________________________________ FAZER LOGIN _______________________________________//
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        [WebMethod]
        public string fazerLogin(string email, string senha, string token)
        {
            int resultado;
            string senhaCriptografada = FormsAuthentication.HashPasswordForStoringInConfigFile(senha, "sha1"); //criptografando a senha
            string senhaRecuperada = senha;


            String ConexaoBanco = ConfigurationManager.ConnectionStrings["BancoDeDados"].ConnectionString;
            using (SqlConnection conexao = new SqlConnection(ConexaoBanco))
            {
                conexao.Open();
                using (SqlCommand cmd = new SqlCommand("usp_fazerLogin", conexao)) //producer a ser executada
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@email", email); //parametros
                    cmd.Parameters.AddWithValue("@senha", senhaCriptografada); //parametros
                    cmd.Parameters.AddWithValue("@senhaRecuperada", senhaRecuperada); //parametros)
                    cmd.Parameters.AddWithValue("@token", token); //parametros

                    SqlParameter returnValue = new SqlParameter(); //variavel para salvar o retorno
                    returnValue.Direction = ParameterDirection.ReturnValue;
                    cmd.Parameters.Add(returnValue);

                    cmd.ExecuteNonQuery();
                    resultado = (Int32)returnValue.Value; //atribuição do resultado de retorno a variavel resultado
                }
				conexao.Close();
            }
			
            JavaScriptSerializer js = new JavaScriptSerializer();
            if (resultado == -1) //deu treta
            {
                return js.Serialize("-1");
            }
            else //usuario cadastrado
            {
                return js.Serialize(resultado.ToString());
            }

        }


        //_______________________________________ CADASTRAR USUARIO _______________________________________//
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        [WebMethod]
        public string cadastrarUsuario(string nomeUsuario, string email, string senha, string token)
        {

            int resultado;
            JavaScriptSerializer js = new JavaScriptSerializer();
            string senhaCriptografada = FormsAuthentication.HashPasswordForStoringInConfigFile(senha, "sha1"); //criptografando a senha

            try
            {

             
                String ConexaoBanco = ConfigurationManager.ConnectionStrings["BancoDeDados"].ConnectionString;
                using (SqlConnection conexao = new SqlConnection(ConexaoBanco))
                {
                    conexao.Open();
                    using (SqlCommand cmd = new SqlCommand("usp_cadastrarUsuario", conexao)) //producer a ser executada
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@nomeUsuario", nomeUsuario); //parametros
                        cmd.Parameters.AddWithValue("@email", email); //parametros
                        cmd.Parameters.AddWithValue("@senha", senhaCriptografada); //parametros
                        cmd.Parameters.AddWithValue("@token", token); //parametros

                        SqlParameter returnValue = new SqlParameter(); //variavel para salvar o retorno
                        returnValue.Direction = ParameterDirection.ReturnValue;
                        cmd.Parameters.Add(returnValue);

                        cmd.ExecuteNonQuery();
                        resultado = (Int32)returnValue.Value; //atribuição do resultado de retorno a variavel resultado
                    }
                }

                if (resultado == -1) //tudo ok
                {
                    return js.Serialize("0");
                }
                else //ja possui uma conta com esse email
                {
                    return js.Serialize("1");
                }

            }
            catch (Exception msg)
            {
                msg.ToString();
                return js.Serialize("2");
            }

        }


        //_______________________________________ VERIFICAR LOGIN _______________________________________//
		[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
		[WebMethod]
		public string verificarLogin(string email, string token)
		{
			int resultado;

			String ConexaoBanco = ConfigurationManager.ConnectionStrings["BancoDeDados"].ConnectionString;
			using (SqlConnection conexao = new SqlConnection(ConexaoBanco))
			{
				conexao.Open();
				using (SqlCommand cmd = new SqlCommand("usp_verificarLogin", conexao)) //producer a ser executada
				{
					cmd.CommandType = CommandType.StoredProcedure;

					cmd.Parameters.AddWithValue("@email", email); //parametros
					cmd.Parameters.AddWithValue("@token", token); //parametros

					SqlParameter returnValue = new SqlParameter(); //variavel para salvar o retorno
					returnValue.Direction = ParameterDirection.ReturnValue;
					cmd.Parameters.Add(returnValue);

					cmd.ExecuteNonQuery();
					resultado = (Int32)returnValue.Value; //atribuição do resultado de retorno a variavel resultado
				}
			}

			JavaScriptSerializer js = new JavaScriptSerializer();
			if (resultado == -1) //deu treta
			{
				return js.Serialize("-1");
			}
			else //usuario cadastrado
			{
				return js.Serialize("0");
			}

		}


        //_______________________________________ ATUALIZAR SENHA _______________________________________//
		[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
		[WebMethod]
		public string atualizarSenhaUsuario(string email, string senha, string novaSenha)
		{

			int resultado;
			JavaScriptSerializer js = new JavaScriptSerializer();
			string senhaCriptografada = FormsAuthentication.HashPasswordForStoringInConfigFile(senha, "sha1"); //criptografando a senha
			string novaSenhaCriptografada = FormsAuthentication.HashPasswordForStoringInConfigFile(novaSenha, "sha1");

				String ConexaoBanco = ConfigurationManager.ConnectionStrings["BancoDeDados"].ConnectionString;
				using (SqlConnection conexao = new SqlConnection(ConexaoBanco))
				{
					conexao.Open();
					using (SqlCommand cmd = new SqlCommand("usp_atualizarSenhaUsuario", conexao)) //producer a ser executada
					{
						cmd.CommandType = CommandType.StoredProcedure;

						cmd.Parameters.AddWithValue("@email", email); //parametros
						cmd.Parameters.AddWithValue("@senha", senhaCriptografada); //parametros
						cmd.Parameters.AddWithValue("@novaSenha", novaSenhaCriptografada); //parametros


						SqlParameter returnValue = new SqlParameter(); //variavel para salvar o retorno
						returnValue.Direction = ParameterDirection.ReturnValue;
						cmd.Parameters.Add(returnValue);

						cmd.ExecuteNonQuery();
						resultado = (Int32)returnValue.Value; //atribuição do resultado de retorno a variavel resultado
					}
				}

				if (resultado == 1) //tudo ok
				{
					return js.Serialize("0");
				}
				else //ja possui uma conta com esse email
				{
					return js.Serialize("1");
				}
		}


        //_______________________________________ LOGOUT _______________________________________//
		[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
		[WebMethod]
		public string logout(string email)
		{

			int resultado;
			JavaScriptSerializer js = new JavaScriptSerializer();

			String ConexaoBanco = ConfigurationManager.ConnectionStrings["BancoDeDados"].ConnectionString;
			using (SqlConnection conexao = new SqlConnection(ConexaoBanco))
			{
				conexao.Open();
				using (SqlCommand cmd = new SqlCommand("usp_logout", conexao)) //producer a ser executada
				{
					cmd.CommandType = CommandType.StoredProcedure;

					cmd.Parameters.AddWithValue("@email", email); //parametros

					SqlParameter returnValue = new SqlParameter(); //variavel para salvar o retorno
					returnValue.Direction = ParameterDirection.ReturnValue;
					cmd.Parameters.Add(returnValue);

					cmd.ExecuteNonQuery();
					resultado = (Int32)returnValue.Value; //atribuição do resultado de retorno a variavel resultado
				}
			}

			if (resultado == 1) //tudo ok
			{
				return js.Serialize("0");
			}
			else //ja possui uma conta com esse email
			{
				return js.Serialize("1");
			}
		}


        //_______________________________________ RECUPERAR SENHA _______________________________________//
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
		[WebMethod]
        public string recuperarSenha(string emailUsuario)
        {


            String ConexaoBanco = ConfigurationManager.ConnectionStrings["BancoDeDados"].ConnectionString;
            SqlConnection conexao = new SqlConnection(ConexaoBanco);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;
            JavaScriptSerializer js = new JavaScriptSerializer();
            string nome = "";
            string senha = "";


            //SQL "injector" 
            cmd.CommandText = "SELECT * FROM tb_Usuario WHERE email = '"+emailUsuario+"'";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = conexao;

            conexao.Open();

            reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                nome = reader["nome"].ToString();
                senha = reader["senha"].ToString();
            }

            try
            {
                if (nome != "") //tudo ok
                {
                    //enviar email
                    SmtpClient cliente = new SmtpClient();
                    cliente.Host = "smtp.gmail.com";
                    cliente.EnableSsl = true;
                    cliente.Credentials = new NetworkCredential("sistemadecomprasdigitais@gmail.com", "comprasdigitais"); //email e sennha 

                    cliente.Send("sistemadecomprasdigitais@gmail.com", emailUsuario,
                    "Recuperar senha", "Olá " + nome + "! Sua senha provisória é: " + senha); //1º email do remetende, 2º email do destinario, 3º titulo do email, 4º conteudo//

                    return js.Serialize("0");
                }
                else
                {
                    return js.Serialize("1");
                }

            }catch(Exception ex)
            {
                return js.Serialize("2");
            }
        }

    }
}
