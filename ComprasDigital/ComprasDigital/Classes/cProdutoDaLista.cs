using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace ComprasDigital.Classes
{
    public class cProdutoDaLista
    {
        public string nome { get; set; }
        public int marca { get; set; }
        public int idLista { get; set; }
        public int quantidade{ get; set; }

        public cProdutoDaLista()
        {
            /*vazio*/
        }

        public cProdutoDaLista(string nome, int marca, int idLista, int quantidad = 0)
        {
            this.nome = nome;
            this.marca = marca;
            this.idLista = idLista;
            if (this.quantidade < 0) this.quantidade = 0;
        }


        public string ToJson()
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(this);
        }
    }
}