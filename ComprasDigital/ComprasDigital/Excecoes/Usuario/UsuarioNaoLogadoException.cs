using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComprasDigital.Excecoes
{
	public class UsuarioNaoLogadoException : Exception
	{
		public string erro { get; set; }
		public UsuarioNaoLogadoException()
			: base("O usuário não está logado")
		{
			erro = "Erro de Usuário";
		}
	}
}