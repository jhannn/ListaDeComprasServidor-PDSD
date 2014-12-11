using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace ComprasDigital.Classes
{
	public class cItem
	{
		public int id { get; set; }
		public cProduto produto { get; set; }
		public cEstabelecimento estabelecimento { get; set; }
		public DateTime compraMaisRecente { get; set; }
		public int qualificacao { get; set; }

		public cItem(int id, cProduto produto, cEstabelecimento estabelecimento, DateTime compraMaisRecente, int qualificacao)
		{
			this.id = id;
			this.produto = produto;
			this.estabelecimento = estabelecimento;
			this.compraMaisRecente = compraMaisRecente;
			this.qualificacao = qualificacao;
		}

		public string ToJson()
		{
			JavaScriptSerializer js = new JavaScriptSerializer();
			return js.Serialize(this);
		}
	}
}