using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComprasDigital.Excecoes
{
	public class OcorreuAlgumErroException : Exception
	{
		public string erro { get; set; }
		public OcorreuAlgumErroException()
			: base("Ocorreu algum erro! Por favor repita o procedimento!")
		{
			erro = "Erro de Usuário";
		}
	}
}