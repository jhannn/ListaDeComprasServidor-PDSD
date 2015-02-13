using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComprasDigital.Excecoes.Estabelecimento
{
    public class EstabelecimentoNaoExistenteException : OcorreuAlgumErroException
	{
		public EstabelecimentoNaoExistenteException()
			: base("Erro de Estabelecimento", "O estabelecimento solicitado nao existe")
		{
		}
	}
}