using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace ComprasDigital.Classes
{
	public class cUsuario
	{
		public int id { get; set; }
		public string nome { get; set; }
		public string email { get; set; }
		public string token { get; set; }
		private string senha;

		public cUsuario(int id, string nome, string email, string token, string senha)
		{
			this.id = id;
			this.nome = nome;
			this.email = email;
			this.token = token;
			this.senha = senha;
		}

		public string ToJson()
		{
			JavaScriptSerializer js = new JavaScriptSerializer();
			return js.Serialize(this);
		}

		public static bool usuarioValido(int idUsuario, string token)
		{
			var dataContext = new Model.DataClassesDataContext();
			var usuarioLogado = from u in dataContext.tb_Usuarios where u.id_usuario == idUsuario && u.token == token select u;
			if (usuarioLogado.Count() == 1) return true;
			return false;
		}

        internal static bool usuarioValido()
        {
            throw new NotImplementedException();
        }
    }
}