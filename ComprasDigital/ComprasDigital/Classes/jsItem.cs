using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComprasDigital.Classes
{
	public class jsItem
	{
		public int idProduto { get; set; }
		public string nome { get; set; }
		public string marca { get; set; }
		public string codigoDeBarras  { get; set; }
		public string tipoCodigo  { get; set; }
		public int embalagem { get; set; }
		public int unidade { get; set; }
		public int quantidade { get; set; }
		public double preco { get; set; }
	}
}