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
			: base("Não há nenhum produto cadastrado.")
		{
			erro = "Erro de Produto";
		}
    }
}