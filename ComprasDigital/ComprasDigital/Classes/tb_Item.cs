//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ComprasDigital.Classes
{
    using System;
    using System.Collections.Generic;
    
    public partial class tb_Item
    {
        public tb_Item()
        {
            this.tb_ItemDaLista = new HashSet<tb_ItemDaLista>();
        }
    
        public int id_item { get; set; }
        public double preco { get; set; }
        public int qualificacao { get; set; }
        public System.DateTime compraMaisRecente { get; set; }
        public int id_estabelecimento { get; set; }
        public int id_produto { get; set; }
    
        public virtual tb_Estabelecimento tb_Estabelecimento { get; set; }
        public virtual tb_Produto tb_Produto { get; set; }
        public virtual ICollection<tb_ItemDaLista> tb_ItemDaLista { get; set; }
    }
}