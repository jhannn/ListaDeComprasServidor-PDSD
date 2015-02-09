﻿using System;
using System.Collections;
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
using ComprasDigital.Classes;
using ComprasDigital.Excecoes;
using ComprasDigital.Excecoes.ListaDeProduto;


namespace ComprasDigital.Servidor
{
    /// <summary>
    /// Summary description for ListaDeProdutos
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class ListaDeProdutos : System.Web.Services.WebService
    {

        //______________________________ CRIAR LISTA __________________________________//
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        [WebMethod]
        public string criarLista(int idUsuario, string token, string nomeLista)
        {
			JavaScriptSerializer js = new JavaScriptSerializer();//O JavaScriptSerializer vai fazer o web service retornar JSON

			if(!cUsuario.usuarioValido(idUsuario, token))
				return js.Serialize(new UsuarioNaoLogadoException()); //retorna a exception UsuarioNaoLogado

			var dataContext = new Model.DataClassesDataContext();
			var listas = from l in dataContext.tb_ListaDeProdutos where l.id_usuario == idUsuario && l.nome.ToLower() == nomeLista.ToLower() select l;
			if (listas.Count() == 1)
			{
				int quant = 2 + (from l in dataContext.tb_ListaDeProdutos where l.id_usuario == idUsuario && l.nome.ToLower().StartsWith(nomeLista.ToLower() + "-") select l).Count();
				nomeLista += "-" + quant;
			}
		
			Model.tb_ListaDeProduto novaLista = new Model.tb_ListaDeProduto();
			novaLista.id_usuario = idUsuario;
			novaLista.nome = nomeLista;
			dataContext.tb_ListaDeProdutos.InsertOnSubmit(novaLista);
			dataContext.SubmitChanges();
			novaLista = (from l in dataContext.tb_ListaDeProdutos where l.nome == nomeLista && l.id_usuario == idUsuario select l).SingleOrDefault();
			return js.Serialize(new cListaDeProduto(novaLista));
        }



		//___________________________________ EDITAR LISTA __________________________________//
		[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
		[WebMethod]
		public string editarNomeLista(int idUsuario, string token, int idLista, string novoNomeDaLista)
		{
			JavaScriptSerializer js = new JavaScriptSerializer();

			if (!cUsuario.usuarioValido(idUsuario, token))
				return js.Serialize(new UsuarioNaoLogadoException());

            var dataContext = new Model.DataClassesDataContext();
            var queryLista = (from l in dataContext.tb_ListaDeProdutos 
                             where l.id_listaDeProdutos == idLista 
                             select l).Single();

			if (queryLista.id_listaDeProdutos < 1)
				return js.Serialize(new ListaNaoEncontradaException());

			var listas = from l in dataContext.tb_ListaDeProdutos where l.id_usuario == idUsuario && l.nome.ToLower() == novoNomeDaLista.ToLower() select l;
			if (listas.Count() == 1)
			{
				int quant = 2 + (from l in dataContext.tb_ListaDeProdutos where l.id_usuario == idUsuario && l.nome.ToLower().StartsWith(novoNomeDaLista.ToLower() + "-") select l).Count();
				novoNomeDaLista += "-" + quant;
			}

            queryLista.nome = novoNomeDaLista;
            dataContext.SubmitChanges();
           
			return js.Serialize("Ok");
		}



		//_________________________________________ RETORNAR LISTA ________________________________________//
		[WebMethod]
		public string retornarLista(int idUsuario, string token, int idListaDeProdutos)
		{
            JavaScriptSerializer js = new JavaScriptSerializer();

            if (!cUsuario.usuarioValido(idUsuario, token))
                return js.Serialize(new UsuarioNaoLogadoException()); //retorna a exception UsuarioNaoLogado

            var dataContext = new Model.DataClassesDataContext();

			var queryLista = (from l in dataContext.tb_ListaDeProdutos
							  where l.id_listaDeProdutos == idListaDeProdutos
							  select l);

			if (queryLista.Count() != 1)
				return js.Serialize(new ListaNaoEncontradaException());

            return js.Serialize(new cListaDeProduto(queryLista.FirstOrDefault(), true));
		}



        //_______________________________________ RETORNAR LISTAS ___________________________________________//
        [WebMethod]
        public string listarListas(int idUsuario, string token)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            if (!cUsuario.usuarioValido(idUsuario, token))
                return js.Serialize(new UsuarioNaoLogadoException()); //retorna a exception UsuarioNaoLogado

			ArrayList listas = new ArrayList();

            var dataContext = new Model.DataClassesDataContext();
            var selectListas = from l in dataContext.tb_ListaDeProdutos where l.id_usuario == idUsuario select l;

            foreach(var list in selectListas)
            {
                listas.Add(new cListaDeProduto(list) );
            }

            return js.Serialize(listas);
        }



        //___________________________________ REMOVER LISTA _____________________________________________//
		[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        [WebMethod]
        public string removerLista(int idUsuario, string token, int idLista)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            if(!cUsuario.usuarioValido(idUsuario,token))
                return js.Serialize(new UsuarioNaoLogadoException());

            var dataContext = new Model.DataClassesDataContext();

			var lista = from l in dataContext.tb_ListaDeProdutos where l.id_listaDeProdutos == idLista select l;

			if (lista.Count() < 1)
				return js.Serialize(new ListaNaoEncontradaException());

			Model.tb_ListaDeProduto listaDeletada = lista.FirstOrDefault();

			foreach (var prod in listaDeletada.tb_ProdutoDaListas)
				dataContext.tb_ProdutoDaListas.DeleteOnSubmit(prod);
			dataContext.tb_ListaDeProdutos.DeleteOnSubmit(listaDeletada);

			dataContext.SubmitChanges();
    
            return js.Serialize("OK");
		}



		//_______________________________________ CADASTRAR PRODUTOS NA LISTA ___________________________________________//
		[WebMethod]
		public string cadastrarProduto(int idUsuario, string token, int idLista, int idProduto, int quantidade)
		{
			JavaScriptSerializer js = new JavaScriptSerializer();

			if (!cUsuario.usuarioValido(idUsuario, token))
				return js.Serialize(new UsuarioNaoLogadoException());

			return "";
		}
		
		

		//_______________________________________ REMOVER PRODUTO DA LISTA ___________________________________________//
		[WebMethod]
		public string removerProdutoDaLista(int idUsuario,string token,int idProduto)
		{
            JavaScriptSerializer js = new JavaScriptSerializer();

            if (!cUsuario.usuarioValido(idUsuario, token))
                return js.Serialize(new UsuarioNaoLogadoException());

            var dataContext = new Model.DataClassesDataContext();
            var querryProdutos = from p in dataContext.tb_ProdutoDaListas
                                 where p.id_produto == idProduto
                                 select p;

            foreach(var prod in querryProdutos)
            {
                dataContext.tb_ProdutoDaListas.DeleteOnSubmit(prod);
                dataContext.SubmitChanges();
            }
            return js.Serialize("Produto da lista removido.");
		}
    }
}
