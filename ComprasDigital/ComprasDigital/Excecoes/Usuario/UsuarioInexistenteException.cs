using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComprasDigital.Excecoes.Usuario
{
    public class UsuarioInexistenteException : OcorreuAlgumErroException
	{
		public string erro { get; set; }
		public UsuarioInexistenteException()
			: base("Erro de Usuário", "O usuário não existe")
		{
		}
	}
}