﻿using System;
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
using System.Collections;

namespace ComprasDigital.Servidor
{
    /// <summary>
    /// Summary description for Produto
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class Produto : System.Web.Services.WebService
    {
        //_______________________________________ AUTOCOMPLETE ___________________________________________//
        [WebMethod]
        public string autocomplete(int idUsuario,string token,string nomeProduto)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            if (!cUsuario.usuarioValido(idUsuario, token))
                return js.Serialize(new UsuarioNaoLogadoException()); //retorna a exception UsuarioNaoLogado

             var dataContext = new Model.DataClassesDataContext();
             var produtos = (from p in dataContext.tb_Produtos 
                            where p.nome.Contains(nomeProduto) 
                            select p.nome).Take(5);

             ArrayList listasDeProdutos = new ArrayList();
             foreach (var nome in produtos)
             {
                 listasDeProdutos.Add(nome);
             }

             return js.Serialize(listasDeProdutos);
        }

        //_______________________________________ RETORNAR PRODUTO ___________________________________________//
        [WebMethod]
        public string retornarProdutos(int idUsuario, string token, string nome, string marca)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            if (!cUsuario.usuarioValido(idUsuario, token))
                return js.Serialize(new UsuarioNaoLogadoException()); //retorna a exception UsuarioNaoLogado

            var dataContext = new Model.DataClassesDataContext();
            var produto = from p in dataContext.tb_Produtos 
                           where p.nome.Contains(nome) && p.tb_Marca.marca == marca
                           select p;

            if (produto.Count() < 1) return js.Serialize(new ProdutoNaoEncontradoException());

			return js.Serialize(new cProduto(produto.FirstOrDefault()));
        }

		//_______________________________________ PESQUISAR PRODUTOS ___________________________________________//
		[WebMethod]
		public string pesquisarProdutosMarca(int idUsuario, string token, string marca)
		{
			JavaScriptSerializer js = new JavaScriptSerializer();

			if (!cUsuario.usuarioValido(idUsuario, token))
				return js.Serialize(new UsuarioNaoLogadoException()); //retorna a exception UsuarioNaoLogado

			var dataContext = new Model.DataClassesDataContext();
			var produtosPorMarca = from p in dataContext.tb_Produtos where p.tb_Marca.marca.ToLower() == marca.ToLower() orderby p.nome select p;

			if (produtosPorMarca.Count() < 1) return js.Serialize(new PesquisaSemResultadosException());

			ArrayList produtos = new ArrayList();
			foreach (var prod in produtosPorMarca)
				produtos.Add(new cProduto(prod));

			return js.Serialize(produtos);
		}

		//___________________________________ PESQUISAR PRODUTOS POR NOME ________________________________________//
		[WebMethod]
		public string pesquisarProdutosNome(int idUsuario, string token, string marca, string nome)
		{
			JavaScriptSerializer js = new JavaScriptSerializer();

			if (!cUsuario.usuarioValido(idUsuario, token))
				return js.Serialize(new UsuarioNaoLogadoException()); //retorna a exception UsuarioNaoLogado

			var dataContext = new Model.DataClassesDataContext();
			var produtosPorNome = from p in dataContext.tb_Produtos where p.tb_Marca.marca.ToLower() == marca.ToLower() && p.nome.ToLower().Contains(nome.ToLower()) orderby p.marca select p;

			if (produtosPorNome.Count() < 1) return js.Serialize(new PesquisaSemResultadosException());

			ArrayList produtos = new ArrayList();
			foreach (var prod in produtosPorNome)
				produtos.Add(new cProduto(prod));

			return js.Serialize(produtos);
		}

    }
}

