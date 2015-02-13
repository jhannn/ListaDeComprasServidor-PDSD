using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComprasDigital.Excecoes.Usuario
{
    public class UsuarioNaoLogadoException : OcorreuAlgumErroException
	{
		public string erro { get; set; }
		public UsuarioNaoLogadoException()
			: base("Erro de Usuário", "O usuário não está logado")
		{
		}
	}
}