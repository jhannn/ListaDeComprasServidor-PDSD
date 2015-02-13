using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComprasDigital.Excecoes
{
    public class PesquisaSemResultadosException : OcorreuAlgumErroException
	{
		public PesquisaSemResultadosException()
			: base("Erro de Pesquisa", "A pesquisa não trouxe nenhum resultado")
		{
		}
	}
}