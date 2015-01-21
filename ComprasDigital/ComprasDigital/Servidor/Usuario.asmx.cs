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
using ComprasDigital.Excecoes;
using System.Collections;


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
            string senhaCriptografada = FormsAuthentication.HashPasswordForStoringInConfigFile(senha, "sha1"); //criptografando a senha
            string senhaRecuperada = senha;

			JavaScriptSerializer js = new JavaScriptSerializer();

			var dataContext = new Model.DataClassesDataContext();
			var usuarios = from users in dataContext.tb_Usuarios where users.email == email && users.senha == senhaCriptografada select users;
			if (usuarios.Count() != 1) return js.Serialize(new UsuarioInexistenteException());

			Model.tb_Usuario usuarioLogado = new Model.tb_Usuario();
			foreach(var usuario in usuarios)
			{
				usuarioLogado.id_usuario = usuario.id_usuario;
				usuarioLogado.nome = usuario.nome;
				usuarioLogado.email = usuario.email;
				break;
			}

			Model.tb_Usuario objUsuario = dataContext.tb_Usuarios.Single(usuario => usuario.email == email);
			objUsuario.token = token;
			dataContext.SubmitChanges();

			return js.Serialize(usuarioLogado);
        }


        //_______________________________________ CADASTRAR USUARIO _______________________________________//
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        [WebMethod]
        public string cadastrarUsuario(string nomeUsuario, string email, string senha, string token)
        {

			string senhaCriptografada = FormsAuthentication.HashPasswordForStoringInConfigFile(senha, "sha1"); //criptografando a senha
			string senhaRecuperada = senha;

			JavaScriptSerializer js = new JavaScriptSerializer();

			var dataContext = new Model.DataClassesDataContext();
			var usuarios = from users in dataContext.tb_Usuarios where users.email == email select users;
			if (usuarios.Count()>1) return js.Serialize(new UsuarioExistenteException());

			Model.tb_Usuario novoUsuario = new Model.tb_Usuario();
			novoUsuario.nome = nomeUsuario;
			novoUsuario.email = email;
			novoUsuario.senha = senhaCriptografada;
			dataContext.tb_Usuarios.InsertOnSubmit(novoUsuario);
			dataContext.SubmitChanges();

			return js.Serialize("Ok");

        }


        //_______________________________________ VERIFICAR LOGIN _______________________________________//
		[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
		[WebMethod]
		public string verificarLogin(string email, string token)
		{
			JavaScriptSerializer js = new JavaScriptSerializer();

			var dataContext = new Model.DataClassesDataContext();
			var usuarios = from users in dataContext.tb_Usuarios where users.email == email && users.token == token select users;
			if (usuarios.Count() != 1) return js.Serialize(new UsuarioNaoLogadoException());

			return "OK";
		}


        //_______________________________________ ATUALIZAR SENHA _______________________________________//
		[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
		[WebMethod]
		public string atualizarSenhaUsuario(string email, string senha, string novaSenha)
		{
			JavaScriptSerializer js = new JavaScriptSerializer();
			string senhaCriptografada = FormsAuthentication.HashPasswordForStoringInConfigFile(senha, "sha1"); //criptografando a senha
			string novaSenhaCriptografada = FormsAuthentication.HashPasswordForStoringInConfigFile(novaSenha, "sha1");
			
			var dataContext = new Model.DataClassesDataContext();
			var usuarios = from users in dataContext.tb_Usuarios where users.email == email && users.senha == senhaCriptografada select users;
			if (usuarios.Count() != 1) return js.Serialize(new UsuarioInexistenteException());

			Model.tb_Usuario objUsuario = dataContext.tb_Usuarios.Single(usuario => usuario.email == email);
			objUsuario.senha = novaSenhaCriptografada;
			dataContext.SubmitChanges();

			return js.Serialize("OK");			
		}


        //_______________________________________ LOGOUT _______________________________________//
		[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
		[WebMethod]
		public string logout(string email, string token)
		{
			JavaScriptSerializer js = new JavaScriptSerializer();
			var dataContext = new Model.DataClassesDataContext();
			var usuarios = from users in dataContext.tb_Usuarios where users.email==email && users.token==token select users;
			if (usuarios.Count() != 1) return js.Serialize(new UsuarioNaoLogadoException());
			Model.tb_Usuario objUsuario = dataContext.tb_Usuarios.Single(usuario => usuario.email == email);
			objUsuario.token = "";
			dataContext.SubmitChanges();

			return "Ok";
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
			cmd.CommandText = "SELECT * FROM tb_Usuario WHERE email = '" + emailUsuario + "'";
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
					"Recuperar senha", "Olá " + nome + "! Sua senha provisória é: " + senha.Substring(0, 6)); //1º email do remetende, 2º email do destinario, 3º titulo do email, 4º conteudo//

					return js.Serialize("0");
				}
				else
				{
					return js.Serialize("1");
				}

			}
			catch (Exception ex)
			{
				return js.Serialize("2");
			}
		}

    }
}
