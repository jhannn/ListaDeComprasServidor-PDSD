using ComprasDigital.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComprasDigital.Classes
{
	class cItem : cProduto
	{
		public int id_estabelecimento { get; set; }
		public string nomeEstabelecimento { get; set; }
		public string dataAtual { get; set; }
		public double preco { get; set; }
		public int quantidade { get; set; }

		public cItem(tb_Produto prod, int qt = 0)
			: base(prod)
		{
			id_estabelecimento = -1;
			nomeEstabelecimento = "-";
			quantidade = qt;
			dataAtual = "-";
			preco = 0;
		}

		public cItem(tb_Produto prod, tb_Estabelecimento estab, int qt = 0)
			: base(prod)
		{
			id_estabelecimento = estab.id_estabelecimento;
			nomeEstabelecimento = estab.nome;
			quantidade = qt;
			var dataContext = new DataClassesDataContext();
			DateTime dataMaisAtual = dataContext.tb_Items.Max(i => i.data);
			var itens = from i in dataContext.tb_Items where i.id_estabelecimento == this.id_estabelecimento && i.id_produto == this.id_produto && i.data == dataMaisAtual orderby i.qualificacao select i;
			if (itens.Count() < 1)
			{
				dataAtual = "-";
				preco = 0;
			}
			else
			{
				dataAtual = itens.First().data.ToString();
				preco = itens.First().preco;
			}
		}
	}
}
