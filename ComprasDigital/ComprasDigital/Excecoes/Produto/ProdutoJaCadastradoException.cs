using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComprasDigital.Excecoes.Produto
{
    public class ProdutoJaCadastradoException : OcorreuAlgumErroException
    {
        public ProdutoJaCadastradoException()
            : base("Erro de Produto", "O produto já está cadastrado!")
        {
        }
    }
}