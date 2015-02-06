using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ComprasDigital.Model;
using System.Web.Script.Serialization;
using System.Collections;

namespace ComprasDigital.Classes
{
	public class cEstabelecimento
	{
		public string nome { get; set; }
		public string bairro { get; set; }
		public string cidade { get; set; }
		public int id_estabelecimento { get; set; }
		public int numero { get; set; }

		public cEstabelecimento() { }

		public cEstabelecimento(tb_Estabelecimento estab)
		{
			nome = estab.nome;
			bairro = estab.bairro;
			cidade = estab.cidade;
			id_estabelecimento = estab.id_estabelecimento;
			numero = estab.numero;
		}
	}
}