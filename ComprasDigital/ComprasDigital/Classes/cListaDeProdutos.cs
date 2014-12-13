using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace ComprasDigital.Classes
{
	public class cListaDeProdutos
	{
		public int id { get; set; }
		public string nome { get; set; }
		public cUsuario usuario { get; set; }
		public cProduto[] produtos { get; set; }

		public cListaDeProdutos(int id, string nome)
		{
			this.id = id;
			this.nome = nome;
		}

		public cListaDeProdutos(int id, string nome, cUsuario usuario)
		{
			this.id = id;
			this.nome = nome;
			this.usuario = usuario;
		}

		public cListaDeProdutos(int id, string nome, cProduto[] produtos)
		{
			this.id = id;
			this.nome = nome;
			this.produtos = produtos;
		}

		public cListaDeProdutos(int id, string nome, cUsuario usuario, cProduto[] produtos)
		{
			this.id = id;
			this.nome = nome;
			this.usuario = usuario;
			this.produtos = produtos;
		}

		public string ToJson()
		{
			JavaScriptSerializer js = new JavaScriptSerializer();
			return js.Serialize(this);
		}
	}
}