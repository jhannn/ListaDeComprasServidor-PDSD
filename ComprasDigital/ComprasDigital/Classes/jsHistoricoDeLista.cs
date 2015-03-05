using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComprasDigital.Classes
{
	public class jsHistoricoDeLista
	{
		public string nomeEstabelecimento { get; set; }
		public int idEstabelecimento { get; set; }
		public string dataDeCompras { get; set; }
		public double precoTotal { get; set; }
		public List<jsHistoricoDeItem> itens { get; set; }
	}
}