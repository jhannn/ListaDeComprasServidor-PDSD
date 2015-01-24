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

			JavaScriptSerializer js = new JavaScriptSerializer();

			var dataContext = new Model.DataClassesDataContext();
			var usuarios = from users in dataContext.tb_Usuarios 
						   where users.email == email && 
						   (users.senha == senhaCriptografada || users.senha.Substring(0,6) == senha) 
						   select users;

			if (usuarios.Count() == 1)
			{
				var usuariosTokens = from users in dataContext.tb_Usuarios where users.token == token select users;
				if (usuariosTokens.Count() > 0)
				{
					Model.tb_Usuario objUsuario = dataContext.tb_Usuarios.Single(usuario => usuario.token == token);
					objUsuario.token = "";
					dataContext.SubmitChanges();
				}

				Model.tb_Usuario objUsuario1 = dataContext.tb_Usuarios.Single(usuario => usuario.email == email);
				objUsuario1.token = token;
				dataContext.SubmitChanges();

				return js.Serialize(new cUsuario(usuarios.FirstOrDefault()));
			}
			
			return js.Serialize(new SenhaEmailNaoConferemException());
        }


        //_______________________________________ CADASTRAR USUARIO _______________________________________//
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        [WebMethod]
        public string cadastrarUsuario(string nomeUsuario, string email, string senha, string token)
        {

			string senhaCriptografada = FormsAuthentication.HashPasswordForStoringInConfigFile(senha, "sha1"); //criptografando a senha

			JavaScriptSerializer js = new JavaScriptSerializer();

			var dataContext = new Model.DataClassesDataContext();
			var usuarios = from users in dataContext.tb_Usuarios where users.email == email select users;
			if (usuarios.Count() == 0)
			{
				var usuariosTokens = from users in dataContext.tb_Usuarios where users.token == token select users;
				if (usuariosTokens.Count() > 0)
				{
					Model.tb_Usuario objUsuario = dataContext.tb_Usuarios.Single(usuario => usuario.token == token);
					objUsuario.token = "";
					dataContext.SubmitChanges();
				}

				Model.tb_Usuario novoUsuario = new Model.tb_Usuario();
				novoUsuario.nome = nomeUsuario;
				novoUsuario.email = email;
				novoUsuario.senha = senhaCriptografada;
				novoUsuario.token = token;
				dataContext.tb_Usuarios.InsertOnSubmit(novoUsuario);
				dataContext.SubmitChanges();

				return js.Serialize(new cUsuario((from users in dataContext.tb_Usuarios where users.email == email select users).FirstOrDefault()));
			}
			return js.Serialize(new UsuarioExistenteException());
        }


        //_______________________________________ VERIFICAR LOGIN _______________________________________//
		[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
		[WebMethod]
		public string verificarLogin(int idUsuario, string token)
		{
			JavaScriptSerializer js = new JavaScriptSerializer();

			if(!cUsuario.usuarioValido(idUsuario, token))
				return js.Serialize(new UsuarioNaoLogadoException());

			return js.Serialize("OK"); 
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
			if (usuarios.Count() != 1){
				return js.Serialize(new UsuarioInexistenteException());
			}
			else if (usuarios.Count() == 1)
			{
				Model.tb_Usuario objUsuario = dataContext.tb_Usuarios.Single(usuario => usuario.email == email);
				objUsuario.senha = novaSenhaCriptografada;
				dataContext.SubmitChanges();
				return js.Serialize("OK");		
			}
			return js.Serialize(new OcorreuAlgumErroException());			
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

			return js.Serialize("OK");
		}


        //_______________________________________ RECUPERAR SENHA _______________________________________//
		[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
		[WebMethod]
		public string recuperarSenha(string emailUsuario)
		{
			JavaScriptSerializer js = new JavaScriptSerializer();

			var dataContext = new Model.DataClassesDataContext();
			var contemUsuario = from users in dataContext.tb_Usuarios where users.email == emailUsuario select users;

			try
			{
				if (contemUsuario.Count() == 1) //sistema possui um usuário cadastrado
				{
					//enviar email
					SmtpClient cliente = new SmtpClient();
					cliente.Host = "smtp.gmail.com";
					cliente.EnableSsl = true;
					cliente.Credentials = new NetworkCredential("sistemadecomprasdigitais@gmail.com", "comprasdigitais"); //email e sennha 

					cliente.Send("sistemadecomprasdigitais@gmail.com", emailUsuario,
					"Recuperar senha", "Olá, " + contemUsuario.First().nome + "! Sua senha provisória é: " + contemUsuario.First().senha.Substring(0, 6)); //1º email do remetende, 2º email do destinario, 3º titulo do email, 4º conteudo//

					return js.Serialize("Operação realizada com sucesso!");
				}
				else //sistema nao possui um usuário cadastrado
				{
					return js.Serialize(new UsuarioInexistenteException());
				}

			}
			catch (Exception ex)
			{
				return js.Serialize(ex);
			}
		}

    }
}
