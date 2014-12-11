using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace ComprasDigital.Classes
{
	public class cEstabelecimento
	{
		public string nome { get; set; }
		public string bairro { get; set; }
		public string cidade { get; set; }
		public int numero { get; set; }
		public int id { get; set; }

		public cEstabelecimento(int id, string nome, string bairro, string cidade, int numero)
		{
			this.nome = nome;
			this.cidade = cidade;
			this.bairro = bairro;
			this.numero = numero;
			this.id = id;
		}

		public string ToJson()
		{
			JavaScriptSerializer js = new JavaScriptSerializer();
			return js.Serialize(this);
		}
	}
}