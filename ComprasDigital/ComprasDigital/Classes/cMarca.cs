using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ComprasDigital.Model;

namespace ComprasDigital.Classes
{
	public class cMarca
	{
		public cMarca() { }

		public static tb_Marca criarMarca()
		{
			tb_Marca novaMarca = new tb_Marca();

			return novaMarca;
		}
	}
}