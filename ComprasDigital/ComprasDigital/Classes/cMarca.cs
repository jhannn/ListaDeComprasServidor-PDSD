using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ComprasDigital.Model;

namespace ComprasDigital.Classes
{
	public class cMarca
	{
		public cMarca() { }

		public static tb_Marca criarMarca(string nome)
		{
			var dataContext = new DataClassesDataContext();
			var marca = from m in dataContext.tb_Marcas where m.marca.ToLower() == nome.ToLower() select m;
			if (marca.Count() >= 1) return marca.FirstOrDefault();

			tb_Marca novaMarca = new tb_Marca();
			novaMarca.marca = nome;
			dataContext.tb_Marcas.InsertOnSubmit(novaMarca);
			dataContext.SubmitChanges();
			return (from m in dataContext.tb_Marcas where m.marca.ToLower() == nome.ToLower() select m).FirstOrDefault();
		}
	}
}