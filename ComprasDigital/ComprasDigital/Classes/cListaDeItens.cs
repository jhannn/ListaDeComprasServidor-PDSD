using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace ComprasDigital.Classes
{
	public class cListaDeItens
	{
		public int id { get; set; }
		public string nome { get; set; }
		public cUsuario usuario { get; set; }
		public cItem[] itens { get; set; }
		public DateTime dataDeCompra { get; set; }

		public cListaDeItens()
		{

		}

		public string ToJson()
		{
			JavaScriptSerializer js = new JavaScriptSerializer();
			return js.Serialize(this);
		}
	}
}