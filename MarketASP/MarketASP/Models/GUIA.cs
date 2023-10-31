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
    
    public partial class GUIA
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GUIA()
        {
            this.GUIA_DETALLE = new HashSet<GUIA_DETALLE>();
            this.GUIA_LOTE = new HashSet<GUIA_LOTE>();
            this.GUIA_CUOTAS = new HashSet<GUIA_CUOTAS>();
        }
    
        public long ncode_guia { get; set; }
        public System.DateTime dfemov_guia { get; set; }
        public string smone_guia { get; set; }
        public decimal ntc_guia { get; set; }
        public string sobse_guia { get; set; }
        public bool besta_guia { get; set; }
        public string sserie_guia { get; set; }
        public string snume_guia { get; set; }
        public string suser_guia { get; set; }
        public Nullable<System.DateTime> dfech_guia { get; set; }
        public string susmo_guia { get; set; }
        public Nullable<System.DateTime> dfemo_guia { get; set; }
        public int ncode_tiguia { get; set; }
        public int ncode_alma { get; set; }
        public Nullable<int> ndestino_alma { get; set; }
        public string stipo_guia { get; set; }
        public Nullable<int> ncode_cliente { get; set; }
        public Nullable<int> ncode_docu { get; set; }
        public Nullable<int> ncode_clidire { get; set; }
        public Nullable<int> ncode_mone { get; set; }
        public Nullable<long> ncode_orpe { get; set; }
        public Nullable<long> ncode_tran { get; set; }
        public string sserienume_orpe { get; set; }
        public Nullable<bool> bdespacho_guia { get; set; }
        public Nullable<bool> bventaasociada_guia { get; set; }
        public Nullable<long> ncode_venta { get; set; }
        public string sserienume_venta { get; set; }
        public Nullable<long> ncode_dose { get; set; }
        public string sglosadespacho_guia { get; set; }
        public Nullable<bool> bflete_guia { get; set; }
        public Nullable<decimal> nbrutoex_guia { get; set; }
        public Nullable<decimal> nbrutoaf_guia { get; set; }
        public Nullable<decimal> ndsctoex_guia { get; set; }
        public Nullable<decimal> ndsctoaf_guia { get; set; }
        public Nullable<decimal> nsubex_guia { get; set; }
        public Nullable<decimal> nsubaf_guia { get; set; }
        public Nullable<decimal> nigvex_guia { get; set; }
        public Nullable<decimal> nigvaf_guia { get; set; }
        public Nullable<decimal> ntotaex_guia { get; set; }
        public Nullable<decimal> ntotaaf_guia { get; set; }
        public Nullable<decimal> ntotal_guia { get; set; }
        public Nullable<decimal> ntotalMN_guia { get; set; }
        public Nullable<decimal> ntotalUS_guia { get; set; }
        public Nullable<long> ncuotas_guia { get; set; }
        public Nullable<decimal> ncuotavalor_guia { get; set; }
        public Nullable<long> ncuotadias_guia { get; set; }
        public Nullable<bool> bclienteagretencion { get; set; }
        public Nullable<decimal> nvalIGV_guia { get; set; }
    
        public virtual ALMACEN ALMACEN { get; set; }
        public virtual ALMACEN ALMACEN1 { get; set; }
        public virtual TIPO_GUIA TIPO_GUIA { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GUIA_DETALLE> GUIA_DETALLE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GUIA_LOTE> GUIA_LOTE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GUIA_CUOTAS> GUIA_CUOTAS { get; set; }
    }
}
