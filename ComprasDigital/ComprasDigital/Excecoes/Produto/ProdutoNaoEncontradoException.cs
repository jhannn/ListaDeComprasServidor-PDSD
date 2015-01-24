using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComprasDigital.Excecoes
{
    public class ProdutoNaoEncontradoException : Exception
    {  
		public string erro { get; set; }
        public ProdutoNaoEncontradoException()
			: base("O produto solicitado não existe")
		{
			erro = "Erro de Produto";
		}
    }
}