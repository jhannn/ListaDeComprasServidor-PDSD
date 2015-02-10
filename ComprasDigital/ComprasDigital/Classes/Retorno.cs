using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComprasDigital.Servidor
{
	public class Retorno
	{
		public String retorno { get; set; }

		public Retorno()
		{
			this.retorno = "ACK";
		}
	}
}