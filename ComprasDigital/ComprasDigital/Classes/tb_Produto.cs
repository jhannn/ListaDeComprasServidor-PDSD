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
    
    public partial class tb_Produto
    {
        public tb_Produto()
        {
            this.tb_Item = new HashSet<tb_Item>();
            this.tb_ProdutoDaLista = new HashSet<tb_ProdutoDaLista>();
        }
    
        public int id_produto { get; set; }
        public string nome { get; set; }
        public string codigoDeBarras { get; set; }
        public string tipoCodigoDeBarras { get; set; }
    
        public virtual ICollection<tb_Item> tb_Item { get; set; }
        public virtual ICollection<tb_ProdutoDaLista> tb_ProdutoDaLista { get; set; }
    }
}
