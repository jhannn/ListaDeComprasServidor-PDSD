﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using ComprasDigital.Classes;
using ComprasDigital.Excecoes.Usuario;

namespace ComprasDigital.Servidor
{
    /// <summary>
    /// Summary description for ListaDeItens
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ListaDeItens : System.Web.Services.WebService
    {

        [WebMethod]
		public string finalizarCheckin(int idUsuario, string token, string lista)
		{
			JavaScriptSerializer js = new JavaScriptSerializer();//O JavaScriptSerializer vai fazer o web service retornar JSON

			//if (!cUsuario.usuarioValido(idUsuario, token))
			//	return js.Serialize(new UsuarioNaoLogadoException()); //retorna a exception UsuarioNaoLogado

			/*/	Exemplo de entrada:
			 *	{"idEstabelecimento":1,"dataDeCompras":"\/Date(1425524400000)\/","idUsuario":1,"itensComprados":
			 *	[
			 *		{"idProduto":1,"nome":"Nescau","marca":"Nestle","codigoDeBarras":null,"tipoCodigo":null,"embalagem":1,"unidade":0,"quantidade":2,"preco":2.5},
			 *		{"idProduto":1,"nome":"milk","marca":"parmalat","codigoDeBarras":null,"tipoCodigo":null,"embalagem":2,"unidade":0,"quantidade":1,"preco":3}
			 *	]}
			/*/
			jsListaDeItens listaDeItens = js.Deserialize<jsListaDeItens>(lista);

			return js.Serialize(listaDeItens);
        }
    }
}
