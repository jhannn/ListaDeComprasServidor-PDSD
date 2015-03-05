using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComprasDigital.Classes
{
	public class jsListaDeItens
	{
		public int idEstabelecimento { get; set; }
		public DateTime dataDeCompras { get; set; }
		public int idUsuario { get; set; }
		public jsItem[] itensComprados { get; set; }
	}
}