using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComprasDigital.Excecoes.Estabelecimento
{
    public class EstabelecimentoExistenteException : OcorreuAlgumErroException
	{
		public EstabelecimentoExistenteException()
			: base("Erro de Estabelecimento", "O estabelecimento ja existe")
		{
		}
	}
}