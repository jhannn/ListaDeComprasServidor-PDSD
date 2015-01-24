using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComprasDigital.Excecoes
{
	public class PesquisaSemResultadosException : Exception
	{
		public string erro { get; set; }
		public PesquisaSemResultadosException()
			: base("A pesquisa não trouxe nenhum resultado!")
		{
			erro = "Erro de Pesquisa";
		}
	}
}