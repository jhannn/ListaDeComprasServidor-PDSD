using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComprasDigital.Excecoes
{
	public class SenhaEmailNaoConferemException: Exception
	{
		public string erro { get; set; }
		public SenhaEmailNaoConferemException()
			: base("A senha ou o email não conferem!")
		{
			erro = "Erro de Usuário";
		}
	}
}