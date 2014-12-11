using System;
using System.Collections.Generic;
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
	}
}