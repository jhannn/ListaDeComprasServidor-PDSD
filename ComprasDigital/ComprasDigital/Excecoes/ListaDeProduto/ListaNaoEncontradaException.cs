using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComprasDigital.Excecoes.ListaDeProduto
{
    public class ListaNaoEncontradaException : OcorreuAlgumErroException
	{
		public string erro { get; set; }
		public ListaNaoEncontradaException()
			: base("Erro de Lista de Produto", "A lista solicitada não existe")
		{
		}
	}
}