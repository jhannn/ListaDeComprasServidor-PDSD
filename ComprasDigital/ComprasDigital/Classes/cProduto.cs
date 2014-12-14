﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace ComprasDigital.Classes
{
	public class cProduto
	{
		public string nome { get; set; }
		public string codigoDeBarras { get; set; }
		public string tipoCodigo { get; set; }
		public int id { get; set; }

		public cProduto()
		{
			/*vazio*/
		}

		public cProduto(int id, string nome, string codigoDeBarras, string tipoCodigo)
		{
			this.id = id;
			this.nome = nome;
			this.codigoDeBarras = codigoDeBarras;
			this.tipoCodigo = tipoCodigo;
			if (this.codigoDeBarras.Trim() == "") this.codigoDeBarras = null;
			if (this.tipoCodigo.Trim() == "") this.tipoCodigo = null;
		}

		public cProduto(int id, string nome)
		{
			this.id = id;
			this.nome = nome;
		}

		public string ToJson()
		{
			JavaScriptSerializer js = new JavaScriptSerializer();
			return js.Serialize(this);
		}
	}
}