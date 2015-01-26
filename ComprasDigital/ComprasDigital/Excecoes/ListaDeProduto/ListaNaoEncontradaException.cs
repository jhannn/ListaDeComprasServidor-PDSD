using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComprasDigital.Excecoes.ListaDeProduto
{
	public class ListaNaoEncontradaException : Exception
	{
		public string erro { get; set; }
		public ListaNaoEncontradaException()
			: base("A lista solicitada não existe")
		{
			erro = "Erro de Lista de Produto";
		}
	}
}