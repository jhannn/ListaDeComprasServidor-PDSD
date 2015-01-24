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
    /// Summary description for Estabelecimento
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Estabelecimento : System.Web.Services.WebService
    {

        [WebMethod]
        public string cadastrarEstabelecimento(int idUsuario, string token, string nome, string bairro, string cidade, int numero)
        {
			JavaScriptSerializer js = new JavaScriptSerializer();

			if (!cUsuario.usuarioValido(idUsuario, token))
				return js.Serialize(new UsuarioNaoLogadoException()); //retorna a exception UsuarioNaoLogado

			var dataContext = new Model.DataClassesDataContext();
			var estabelecimentos = from estabelecimento in dataContext.tb_Estabelecimentos where estabelecimento.nome == nome && estabelecimento.bairro == bairro && estabelecimento.cidade == cidade select estabelecimento;
			if (estabelecimentos.Count() == 0)
			{
				Model.tb_Estabelecimento novoEstabelecimento = new Model.tb_Estabelecimento();
				novoEstabelecimento.nome = nome.ToLower();
				novoEstabelecimento.bairro = bairro.ToLower();
				novoEstabelecimento.cidade = cidade.ToLower();
				novoEstabelecimento.numero = numero;
				dataContext.tb_Estabelecimentos.InsertOnSubmit(novoEstabelecimento);
				dataContext.SubmitChanges();

				return js.Serialize("OK");
			}
			return js.Serialize(new cEstabelecimento(estabelecimentos.FirstOrDefault()));
        }

		[WebMethod]
		public string autoCompleteEstabelecimento(int idUsuario, string token, string nome)
		{
			JavaScriptSerializer js = new JavaScriptSerializer();

			if (!cUsuario.usuarioValido(idUsuario, token))
				return js.Serialize(new UsuarioNaoLogadoException()); //retorna a exception UsuarioNaoLogado

			var dataContext = new Model.DataClassesDataContext();
			var estabelecimentos = (from estabelecimento in dataContext.tb_Estabelecimentos where estabelecimento.nome.ToLower().StartsWith(nome.ToLower()) orderby estabelecimento.nome select estabelecimento.nome).Take(5);

			ArrayList listasDeEstabelecimento = new ArrayList();
			foreach (var nomeEstab in estabelecimentos)
			{
				listasDeEstabelecimento.Add(nomeEstab);
			}
			return js.Serialize(listasDeEstabelecimento);

		}

		[WebMethod]
		public string listarEstabelecimento(int idUsuario, string token, string nome, string bairro, string cidade)
		{
			JavaScriptSerializer js = new JavaScriptSerializer();

			if (!cUsuario.usuarioValido(idUsuario, token))
				return js.Serialize(new UsuarioNaoLogadoException()); //retorna a exception UsuarioNaoLogado

			var dataContext = new Model.DataClassesDataContext();
			var estabelecimentos = from estabelecimento in dataContext.tb_Estabelecimentos where estabelecimento.nome.ToLower().StartsWith(nome.ToLower())
																								&& estabelecimento.bairro.ToLower().StartsWith(bairro.ToLower())
																								&& estabelecimento.cidade.ToLower().StartsWith(cidade.ToLower()) select estabelecimento;
			if (estabelecimentos.Count() > 0)
			{
				ArrayList listasDeEstabelecimento = new ArrayList();
				foreach (var estab in estabelecimentos)
				{
					listasDeEstabelecimento.Add(new cEstabelecimento(estab));
				}
				return js.Serialize(listasDeEstabelecimento);
			}
			return js.Serialize(new EstabelecimentoNaoExistenteException());
			//Caso não encontre nenhum estabelecimento, informar "Nenhum estabelecimento encontrado"
		}

		[WebMethod]
		public string visualizarEstabelecimento(int idUsuario, string token, int id)
		{
			JavaScriptSerializer js = new JavaScriptSerializer();

			if (!cUsuario.usuarioValido(idUsuario, token))
				return js.Serialize(new UsuarioNaoLogadoException()); //retorna a exception UsuarioNaoLogado

			var dataContext = new Model.DataClassesDataContext();
			var estabelecimentos = from estabelecimento in dataContext.tb_Estabelecimentos where estabelecimento.id_estabelecimento == id select estabelecimento;
			if (estabelecimentos.Count() == 1)
			{
				var estab = estabelecimentos.SingleOrDefault();
				return js.Serialize(new cEstabelecimento(estab)); //FirstOrDefault()
			}
			return js.Serialize(new EstabelecimentoNaoExistenteException());
		}

		[WebMethod]
		public string editarEstabelecimento(int idUsuario, string token, int id, string nome, string bairro, string cidade, int numero)
		{
			JavaScriptSerializer js = new JavaScriptSerializer();

			if (!cUsuario.usuarioValido(idUsuario, token))
				return js.Serialize(new UsuarioNaoLogadoException()); //retorna a exception UsuarioNaoLogado

			var dataContext = new Model.DataClassesDataContext();
			var estabelecimentos = from estabelecimento in dataContext.tb_Estabelecimentos where estabelecimento.id_estabelecimento == id select estabelecimento;
			if (estabelecimentos.Count() == 1)
			{
				Model.tb_Estabelecimento objEstabelecimento = dataContext.tb_Estabelecimentos.Single(estabelecimento => estabelecimento.id_estabelecimento == id);
				objEstabelecimento.nome = nome;
				objEstabelecimento.bairro = bairro;
				objEstabelecimento.cidade = cidade;
				objEstabelecimento.numero = numero;
				dataContext.SubmitChanges();

				return js.Serialize("OK");
			}
			return js.Serialize(new OcorreuAlgumErroException());
		}
    }
}
