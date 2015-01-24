using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComprasDigital.Excecoes
{
	public class UsuarioExistenteException : Exception
	{
		public string erro { get; set; }
		public UsuarioExistenteException()
			: base("Já existe um usuário com este email!")
		{
			erro = "Erro de Usuário";
		}
	}
}