//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ComprasDigital.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class tb_Usuario
    {
        public tb_Usuario()
        {
            this.tb_ListaDeItens = new HashSet<tb_ListaDeItens>();
            this.tb_ListaDeProdutos = new HashSet<tb_ListaDeProdutos>();
        }
    
        public int id_usuario { get; set; }
        public string nome { get; set; }
        public string email { get; set; }
        public string senha { get; set; }
        public string token { get; set; }
    
        public virtual ICollection<tb_ListaDeItens> tb_ListaDeItens { get; set; }
        public virtual ICollection<tb_ListaDeProdutos> tb_ListaDeProdutos { get; set; }
    }
}
