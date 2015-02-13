using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComprasDigital.Excecoes.Usuario
{
    public class UsuarioExistenteException : OcorreuAlgumErroException
	{
		public string erro { get; set; }
		public UsuarioExistenteException()
			: base("Erro de Usuário", "Já existe um usuário com este email")
		{
		}
	}
}