using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComprasDigital.Excecoes
{
	public class EstabelecimentoNaoExistenteException : Exception
	{
		public string erro { get; set; }
		public EstabelecimentoNaoExistenteException()
			: base("O estabelecimento solicitado nao existe!")
		{
			erro = "Erro de Estabelecimento!";
		}
	}
}