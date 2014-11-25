using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace ComprasDigital.Servidor
{
    /// <summary>
    /// Summary description for Usuario
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Usuario : System.Web.Services.WebService
    {

        [WebMethod]
        public void fazerLogin(string email, string senha)
        {
            //Alterar local de conexão
            SqlConnection sqlConnection1 = new SqlConnection("Data Source=MARCELO;Initial Catalog=SistemaDeCompras;Integrated Security=True");
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            //SQL "injector" 
            cmd.CommandText = "SELECT * FROM tb_usuario";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnection1;

            sqlConnection1.Open();

            reader = cmd.ExecuteReader();
            // Data is accessible through the DataReader object here.

            while (reader.Read())
            {
                if (email == reader["email"].ToString() && senha == reader["senha"].ToString())
                {

                }
            }

            sqlConnection1.Close();

        }
    }
}
