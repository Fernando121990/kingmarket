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
    
    public partial class COMPRAS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public COMPRAS()
        {
            this.COMPRA_DETALLE = new HashSet<COMPRA_DETALLE>();
        }
    
        public long ncode_compra { get; set; }
        public string sseri_compra { get; set; }
        public string snume_compra { get; set; }
        public Nullable<System.DateTime> dfecompra_compra { get; set; }
        public Nullable<System.DateTime> dfevenci_compra { get; set; }
        public string smone_compra { get; set; }
        public Nullable<decimal> ntc_compra { get; set; }
        public string sobs_compra { get; set; }
        public string sguia_compra { get; set; }
        public string sproforma_compra { get; set; }
        public Nullable<decimal> nbrutoex_compra { get; set; }
        public Nullable<decimal> nbrutoaf_compra { get; set; }
        public Nullable<decimal> ndsctoex_compra { get; set; }
        public Nullable<decimal> ndsctoaf_compra { get; set; }
        public Nullable<decimal> nsubex_compra { get; set; }
        public Nullable<decimal> nsubaf_compra { get; set; }
        public Nullable<decimal> nigvex_compra { get; set; }
        public Nullable<decimal> nigvaf_compra { get; set; }
        public Nullable<decimal> ntotaex_compra { get; set; }
        public Nullable<decimal> ntotaaf_compra { get; set; }
        public Nullable<decimal> ntotal_compra { get; set; }
        public Nullable<decimal> ntotalMN_compra { get; set; }
        public Nullable<decimal> ntotalUS_compra { get; set; }
        public bool besta_compra { get; set; }
        public Nullable<decimal> nvalIGV_compra { get; set; }
        public string suser_compra { get; set; }
        public Nullable<System.DateTime> dfech_compra { get; set; }
        public string susmo_compra { get; set; }
        public Nullable<System.DateTime> dfemo_compra { get; set; }
        public int ncode_alma { get; set; }
        public long ncode_provee { get; set; }
        public int ncode_docu { get; set; }
        public int ncode_fopago { get; set; }
        public Nullable<int> ncode_local { get; set; }
        public Nullable<bool> btitgratuito_compra { get; set; }
        public Nullable<long> ncode_orco { get; set; }
    
        public virtual ALMACEN ALMACEN { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<COMPRA_DETALLE> COMPRA_DETALLE { get; set; }
        public virtual LOCAL LOCAL { get; set; }
        public virtual CONFIGURACION CONFIGURACION { get; set; }
        public virtual CONFIGURACION CONFIGURACION1 { get; set; }
        public virtual PROVEEDOR PROVEEDOR { get; set; }
    }
}
