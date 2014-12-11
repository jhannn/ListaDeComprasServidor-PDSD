using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Configuration;
using System.Web.Script.Serialization;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using System.Diagnostics;
using System.Web.Script.Services;
using System.Web.Security; //pacote de seguraça do ASP.NET


namespace ComprasDigital.Servidor
{
    /// <summary>
    /// Summary description for Usuario
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class Usuario : System.Web.Services.WebService
    {


        //_______________________________________ FAZER LOGIN _______________________________________//
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        [WebMethod]
        public string fazerLogin(string email, string senha, string token)
        {
            int resultado;
            string senhaCriptografada = FormsAuthentication.HashPasswordForStoringInConfigFile(senha, "sha1"); //criptografando a senha


            String ConexaoBanco = ConfigurationManager.ConnectionStrings["BancoDeDados"].ConnectionString;
            using (SqlConnection conexao = new SqlConnection(ConexaoBanco))
            {
                conexao.Open();
                using (SqlCommand cmd = new SqlCommand("usp_fazerLogin", conexao)) //producer a ser executada
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@email", email); //parametros
                    cmd.Parameters.AddWithValue("@senha", senhaCriptografada); //parametros
                    cmd.Parameters.AddWithValue("@token", token); //parametros

                    SqlParameter returnValue = new SqlParameter(); //variavel para salvar o retorno
                    returnValue.Direction = ParameterDirection.ReturnValue;
                    cmd.Parameters.Add(returnValue);

                    cmd.ExecuteNonQuery();
                    resultado = (Int32)returnValue.Value; //atribuição do resultado de retorno a variavel resultado
                }
            }

            JavaScriptSerializer js = new JavaScriptSerializer();
            if (resultado == -1) //deu treta
            {
                return js.Serialize("-1");
            }
            else //usuario cadastrado
            {
                return js.Serialize(resultado.ToString());
            }

        }



        //_______________________________________ CADASTRAR USUARIO _______________________________________//
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        [WebMethod]
        public string cadastrarUsuario(string nomeUsuario, string email, string senha, string token)
        {

            int resultado;
            JavaScriptSerializer js = new JavaScriptSerializer();
            string senhaCriptografada = FormsAuthentication.HashPasswordForStoringInConfigFile(senha, "sha1"); //criptografando a senha

            try
            {

             
                String ConexaoBanco = ConfigurationManager.ConnectionStrings["BancoDeDados"].ConnectionString;
                using (SqlConnection conexao = new SqlConnection(ConexaoBanco))
                {
                    conexao.Open();
                    using (SqlCommand cmd = new SqlCommand("usp_cadastrarUsuario", conexao)) //producer a ser executada
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@nomeUsuario", nomeUsuario); //parametros
                        cmd.Parameters.AddWithValue("@email", email); //parametros
                        cmd.Parameters.AddWithValue("@senha", senhaCriptografada); //parametros
                        cmd.Parameters.AddWithValue("@token", token); //parametros

                        SqlParameter returnValue = new SqlParameter(); //variavel para salvar o retorno
                        returnValue.Direction = ParameterDirection.ReturnValue;
                        cmd.Parameters.Add(returnValue);

                        cmd.ExecuteNonQuery();
                        resultado = (Int32)returnValue.Value; //atribuição do resultado de retorno a variavel resultado
                    }
                }

                if (resultado == -1) //tudo ok
                {
                    return js.Serialize("0");
                }
                else //ja possui uma conta com esse email
                {
                    return js.Serialize("1");
                }

            }
            catch (Exception msg)
            {
                msg.ToString();
                return js.Serialize("2");
            }

        }

    }
}
