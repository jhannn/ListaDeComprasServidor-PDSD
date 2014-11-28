using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Services;

namespace ComprasDigital.Servidor
{
    /// <summary>
    /// Summary description for Produto
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Produto : System.Web.Services.WebService
    {

        [WebMethod]
        public void cadastrarProduto(string codigoDeBarras, string nomeDoProduto, string formatoCodigoDeBarras)
        {

            //Alterar local de conexão
            SqlConnection sqlConnection1 = new SqlConnection("Data Source=BRUNO;Initial Catalog=SistemaDeCompras;Integrated Security=True");
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            //SQL "injector" 
            cmd.CommandText = "INSERT INTO tb_produto VALUES (nomeDoProduto,codigoDeBarras,formatoCodigoDeBarras)";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnection1;

            sqlConnection1.Open();

            reader = cmd.ExecuteReader();


        }

    }
}
