using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComprasDigital.Excecoes
{
	public class EstabelecimentoExistenteException : Exception
	{
		public string erro { get; set; }
		public EstabelecimentoExistenteException()
			: base("O estabelecimento ja existe!")
		{
			erro = "Erro de Estabelecimento!";
		}
	}
}