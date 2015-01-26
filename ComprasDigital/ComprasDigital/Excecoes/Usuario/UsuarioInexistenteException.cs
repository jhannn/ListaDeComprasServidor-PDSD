using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComprasDigital.Excecoes
{
	public class UsuarioInexistenteException : Exception
	{
		public string erro { get; set; }
		public UsuarioInexistenteException()
			: base("O usuário não existe")
		{
			erro = "Erro de Usuário";
		}
	}
}