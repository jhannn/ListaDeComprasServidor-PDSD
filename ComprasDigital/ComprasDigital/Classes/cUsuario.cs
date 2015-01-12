using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace ComprasDigital.Classes
{
	public class cUsuario
	{
		public int id { get; set; }
		public string nome { get; set; }
		public string email { get; set; }
		public string token { get; set; }
		private string senha;

		public cUsuario(int id, string nome, string email, string token, string senha)
		{
			this.id = id;
			this.nome = nome;
			this.email = email;
			this.token = token;
			this.senha = senha;
		}

		public string ToJson()
		{
			JavaScriptSerializer js = new JavaScriptSerializer();
			return js.Serialize(this);
		}

		public static bool usuarioValido(int idUsuario, string token)
		{
			var usuario = new List<string>();

			String ConexaoBanco = ConfigurationManager.ConnectionStrings["BancoDeDados"].ConnectionString;
			SqlConnection conexao = new SqlConnection(ConexaoBanco);
			SqlCommand cmd = new SqlCommand();
			SqlDataReader reader;

			//SQL "injector" 
			cmd.CommandText = "SELECT nome FROM tb_Usuario WHERE token = '" + token + "' AND id_usuario = " + idUsuario + "";
			cmd.CommandType = CommandType.Text;
			cmd.Connection = conexao;

			conexao.Open();
			reader = cmd.ExecuteReader();
			while (reader.Read())
			{
				conexao.Close();
				return true;
			}
			conexao.Close();

			return false;
		}
	}
}