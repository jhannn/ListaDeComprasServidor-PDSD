using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ComprasDigital.Model;

namespace ComprasDigital.Classes
{
	public class cUsuario
	{
		public int id_usuario { get; set; }
		public string email { get; set; }
		public string nome { get; set; }
		public string token { get; set; }

		public cUsuario() { }

		public cUsuario(tb_Usuario usuario)
		{
			id_usuario = usuario.id_usuario;
			email = usuario.email;
			nome = usuario.nome;
			token = usuario.token;
		}

		public static bool usuarioValido(int idUsuario, string token)
		{
			var dataContext = new DataClassesDataContext();
			var usuarioLogado = from u in dataContext.tb_Usuarios where u.id_usuario == idUsuario && u.token == token select u;
			if (usuarioLogado.Count() == 1) return true;
			return false;
		}
    }
}