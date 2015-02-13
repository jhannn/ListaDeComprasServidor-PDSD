using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComprasDigital.Excecoes.Usuario
{
    public class SenhaEmailNaoConferemException : OcorreuAlgumErroException
	{
		public string erro { get; set; }
		public SenhaEmailNaoConferemException()
			: base("Erro de Usuário", "A senha ou o email não conferem")
		{
		}
	}
}