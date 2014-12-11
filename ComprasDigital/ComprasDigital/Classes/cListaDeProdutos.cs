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

		public cListaDeProdutos()
		{
			
		}

		public string ToJson()
		{
			JavaScriptSerializer js = new JavaScriptSerializer();
			return js.Serialize(this);
		}
	}
}