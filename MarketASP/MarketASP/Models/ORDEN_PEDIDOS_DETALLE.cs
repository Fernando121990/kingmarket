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
    
    public partial class ORDEN_PEDIDOS_DETALLE
    {
        public long ncode_orpedeta { get; set; }
        public long ncode_arti { get; set; }
        public Nullable<decimal> ncant_orpedeta { get; set; }
        public Nullable<decimal> npu_orpedeta { get; set; }
        public Nullable<decimal> ndscto_orpedeta { get; set; }
        public Nullable<decimal> ndscto2_orpedeta { get; set; }
        public Nullable<decimal> nexon_orpedeta { get; set; }
        public Nullable<decimal> nafecto_orpedeta { get; set; }
        public bool besafecto_orpedeta { get; set; }
        public Nullable<int> ncode_alma { get; set; }
        public Nullable<decimal> ndsctomax_orpedeta { get; set; }
        public Nullable<decimal> ndsctomin_orpedeta { get; set; }
        public Nullable<decimal> ndsctoporc_orpedeta { get; set; }
        public Nullable<int> nback_orpedeta { get; set; }
        public Nullable<long> ncode_orpe { get; set; }
        public Nullable<decimal> nsubt_orpedeta { get; set; }
    
        public virtual ARTICULO ARTICULO { get; set; }
        public virtual ORDEN_PEDIDOS ORDEN_PEDIDOS { get; set; }
    }
}
