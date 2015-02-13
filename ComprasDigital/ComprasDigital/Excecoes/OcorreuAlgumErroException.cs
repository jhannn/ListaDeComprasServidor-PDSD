using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ComprasDigital.Classes;

namespace ComprasDigital.Excecoes
{
    public class OcorreuAlgumErroException : Retorno
	{
        public string erro { get; set; }
        public string mensagem { get; set; }

		public OcorreuAlgumErroException()
			: base()
		{
            retorno = "NAK";
            mensagem = "Ocorreu algum erro! Por favor repita o procedimento";
			erro = "Erro de Genérico";
		}

        protected OcorreuAlgumErroException(string erro, string mensagem)
            : base()
        {
            retorno = "NAK";
            this.mensagem = mensagem;
            this.erro = erro;
        }
	}
}