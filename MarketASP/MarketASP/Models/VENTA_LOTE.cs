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
    
    public partial class VENTA_LOTE
    {
        public long ncode_velote { get; set; }
        public long ncode_venta { get; set; }
        public long ncode_arti { get; set; }
        public Nullable<decimal> ncant_velote { get; set; }
        public Nullable<int> ncode_alma { get; set; }
        public Nullable<int> nback_velote { get; set; }
        public string sdesc_lote { get; set; }
        public Nullable<System.DateTime> dfvenci_lote { get; set; }
        public Nullable<long> ncode_lote { get; set; }
    
        public virtual ALMACEN ALMACEN { get; set; }
        public virtual ARTICULO ARTICULO { get; set; }
        public virtual VENTAS VENTAS { get; set; }
    }
}
