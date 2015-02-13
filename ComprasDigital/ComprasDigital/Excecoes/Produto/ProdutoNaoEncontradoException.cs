using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComprasDigital.Excecoes.Produto
{
    public class ProdutoNaoEncontradoException : OcorreuAlgumErroException
    {
        public ProdutoNaoEncontradoException()
			: base("Erro de Produto", "O produto solicitado não existe")
		{
		}
    }
}