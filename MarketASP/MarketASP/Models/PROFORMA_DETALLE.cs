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
    
    public partial class PROFORMA_DETALLE
    {
        public long ncode_profdeta { get; set; }
        public long ncode_arti { get; set; }
        public Nullable<decimal> ncant_profdeta { get; set; }
        public Nullable<decimal> npu_profdeta { get; set; }
        public Nullable<decimal> ndscto_profdeta { get; set; }
        public Nullable<decimal> ndscto2_profdeta { get; set; }
        public Nullable<decimal> nexon_profdeta { get; set; }
        public Nullable<decimal> nafecto_profdeta { get; set; }
        public bool besafecto_profdeta { get; set; }
        public Nullable<int> ncode_alma { get; set; }
        public Nullable<decimal> ndsctomax_profdeta { get; set; }
        public Nullable<decimal> ndsctomin_profdeta { get; set; }
        public Nullable<decimal> ndsctoporc_profdeta { get; set; }
        public Nullable<int> nback_profdeta { get; set; }
        public Nullable<long> ncode_prof { get; set; }
    
        public virtual ARTICULO ARTICULO { get; set; }
        public virtual PROFORMAS PROFORMAS { get; set; }
    }
}
