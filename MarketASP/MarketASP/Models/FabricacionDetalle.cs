//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MarketASP.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class FabricacionDetalle
    {
        public string FabD_tipo { get; set; }
        public string FabD_NroDoc { get; set; }
        public string FabD_CodClase { get; set; }
        public string FabD_CodProd { get; set; }
        public Nullable<decimal> FabD_Cantidad { get; set; }
        public Nullable<decimal> FabD_Costo_D { get; set; }
        public Nullable<decimal> FabD_Costo_S { get; set; }
        public string FabD_Almacen { get; set; }
        public string FabD_Mov { get; set; }
        public string FabD_TipoDetalle { get; set; }
        public string FabD_CodClase_Ref { get; set; }
        public string FabD_CodProd_Ref { get; set; }
        public string FabD_CodProd_Ini { get; set; }
        public string FabD_CodClase_Ini { get; set; }
        public Nullable<decimal> FabD_CantUtil { get; set; }
        public Nullable<decimal> FabD_Adicional { get; set; }
        public Nullable<long> Fab_Codigo { get; set; }
        public long FabD_Codigo { get; set; }
        public Nullable<decimal> RecD_Cantidad { get; set; }
        public Nullable<long> ncode_arti { get; set; }
        public Nullable<int> ncode_alma { get; set; }
    
        public virtual ALMACEN ALMACEN { get; set; }
        public virtual Fabricacion Fabricacion { get; set; }
        public virtual ARTICULO ARTICULO { get; set; }
    }
}
