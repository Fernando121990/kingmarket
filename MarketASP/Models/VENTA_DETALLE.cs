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
    
    public partial class VENTA_DETALLE
    {
        public long ncode_vedeta { get; set; }
        public long ncode_venta { get; set; }
        public long ncode_arti { get; set; }
        public Nullable<decimal> ncant_vedeta { get; set; }
        public Nullable<decimal> npu_vedeta { get; set; }
        public Nullable<decimal> ndscto_vedeta { get; set; }
        public Nullable<decimal> ndscto2_vedeta { get; set; }
        public Nullable<decimal> nexon_vedeta { get; set; }
        public Nullable<decimal> nafecto_vedeta { get; set; }
        public bool besafecto_vedeta { get; set; }
        public Nullable<int> ncode_alma { get; set; }
        public Nullable<decimal> ndsctomax_vedeta { get; set; }
        public Nullable<decimal> ndsctomin_vedeta { get; set; }
        public Nullable<decimal> ndsctoporc_vedeta { get; set; }
        public Nullable<int> nback_vedeta { get; set; }
        public Nullable<decimal> nsubt_vedeta { get; set; }
        public Nullable<decimal> ncantLote_vedeta { get; set; }
        public Nullable<long> ncode_orpe { get; set; }
        public Nullable<long> ncode_guia { get; set; }
        public Nullable<int> norden_vedeta { get; set; }
    
        public virtual ALMACEN ALMACEN { get; set; }
        public virtual VENTAS VENTAS { get; set; }
        public virtual ARTICULO ARTICULO { get; set; }
    }
}
