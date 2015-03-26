using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ComprasDigital.Model;
using System.Web.Script.Serialization;
using System.Collections;
using System.Text;

namespace ComprasDigital.Classes
{
	public class cEstabelecimento
	{
		public string nome { get; set; }
		public string bairro { get; set; }
		public string cidade { get; set; }
		public int id_estabelecimento { get; set; }
		public int numero { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public string imagem_estabelecimento { get; set; }

		public cEstabelecimento() { }

		public cEstabelecimento(tb_Estabelecimento estab)
		{
			nome = estab.nome;
			bairro = estab.bairro;
			cidade = estab.cidade;
			id_estabelecimento = estab.id_estabelecimento;
			numero = estab.numero;
            latitude = Convert.ToDouble(estab.latitude);
            longitude = Convert.ToDouble(estab.longitude);
            if (estab.imagem_estabelecimento != null)
                imagem_estabelecimento = Convert.ToBase64String(estab.imagem_estabelecimento.ToArray());
            else
                imagem_estabelecimento = "null";
		}
	}
}