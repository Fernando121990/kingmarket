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
    
    public partial class ORDEN_PEDIDOS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ORDEN_PEDIDOS()
        {
            this.ORDEN_PEDIDOS_DETALLE = new HashSet<ORDEN_PEDIDOS_DETALLE>();
        }
    
        public long ncode_orpe { get; set; }
        public int ncode_docu { get; set; }
        public string sseri_orpe { get; set; }
        public string snume_orpe { get; set; }
        public Nullable<System.DateTime> dfeorpeo_orpe { get; set; }
        public Nullable<System.DateTime> dfevenci_orpe { get; set; }
        public int ncode_cliente { get; set; }
        public Nullable<int> ncode_clidire { get; set; }
        public string smone_orpe { get; set; }
        public Nullable<decimal> ntc_orpe { get; set; }
        public int ncode_fopago { get; set; }
        public string sobse_orpe { get; set; }
        public Nullable<long> ncode_compra { get; set; }
        public Nullable<decimal> nbrutoex_orpe { get; set; }
        public Nullable<decimal> nbrutoaf_orpe { get; set; }
        public Nullable<decimal> ndctoex_orpe { get; set; }
        public Nullable<decimal> ndsctoaf_orpe { get; set; }
        public Nullable<decimal> nsubex_orpe { get; set; }
        public Nullable<decimal> nsubaf_orpe { get; set; }
        public Nullable<decimal> nigvex_orpe { get; set; }
        public Nullable<decimal> nigvaf_orpe { get; set; }
        public Nullable<decimal> ntotaex_orpe { get; set; }
        public Nullable<decimal> ntotaaf_orpe { get; set; }
        public Nullable<decimal> ntotal_orpe { get; set; }
        public Nullable<decimal> ntotalMN_orpe { get; set; }
        public Nullable<decimal> ntotalUs_orpe { get; set; }
        public Nullable<bool> besta_orpe { get; set; }
        public Nullable<decimal> nvalIGV_orpe { get; set; }
        public string suser_orpe { get; set; }
        public Nullable<System.DateTime> dfech_orpe { get; set; }
        public string susmo_orpe { get; set; }
        public Nullable<System.DateTime> dfemo_orpe { get; set; }
        public int ncode_alma { get; set; }
        public int ncode_local { get; set; }
        public Nullable<int> ncode_mone { get; set; }
    
        public virtual ALMACEN ALMACEN { get; set; }
        public virtual CLI_DIRE CLI_DIRE { get; set; }
        public virtual CLIENTE CLIENTE { get; set; }
        public virtual CONFIGURACION CONFIGURACION { get; set; }
        public virtual CONFIGURACION CONFIGURACION1 { get; set; }
        public virtual LOCAL LOCAL { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ORDEN_PEDIDOS_DETALLE> ORDEN_PEDIDOS_DETALLE { get; set; }
    }
}
