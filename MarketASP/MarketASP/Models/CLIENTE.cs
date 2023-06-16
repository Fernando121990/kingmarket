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
    
    public partial class CLIENTE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CLIENTE()
        {
            this.CLI_DIRE = new HashSet<CLI_DIRE>();
            this.CLI_FOPAGO = new HashSet<CLI_FOPAGO>();
            this.CTA_COBRAR = new HashSet<CTA_COBRAR>();
            this.PROFORMAS = new HashSet<PROFORMAS>();
            this.VENTAS = new HashSet<VENTAS>();
            this.ORDEN_PEDIDOS = new HashSet<ORDEN_PEDIDOS>();
        }
    
        public int ncode_cliente { get; set; }
        public string srazon_cliente { get; set; }
        public string sruc_cliente { get; set; }
        public string sdnice_cliente { get; set; }
        public string sfono1_cliente { get; set; }
        public string sfax_cliente { get; set; }
        public string slineacred_cliente { get; set; }
        public string srepre_cliente { get; set; }
        public string smail_cliente { get; set; }
        public string sweb_cliente { get; set; }
        public string sobse_cliente { get; set; }
        public string sfono2_cliente { get; set; }
        public string sfono3_cliente { get; set; }
        public string sappa_cliente { get; set; }
        public string sapma_cliente { get; set; }
        public string snomb_cliente { get; set; }
        public Nullable<bool> bprocedencia_cliente { get; set; }
        public string suser_cliente { get; set; }
        public Nullable<System.DateTime> dfech_cliente { get; set; }
        public string susmo_cliente { get; set; }
        public Nullable<System.DateTime> dfemo_cliente { get; set; }
        public string stipo_cliente { get; set; }
        public Nullable<bool> bacti_cliente { get; set; }
        public Nullable<int> ncode_fopago { get; set; }
        public Nullable<int> ncode_afepercepcion { get; set; }
        public Nullable<int> nidtipodoc_cliente { get; set; }
        public Nullable<bool> bretencion_cliente { get; set; }
        public string smail2_cliente { get; set; }
        public Nullable<long> ncode_vende { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CLI_DIRE> CLI_DIRE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CLI_FOPAGO> CLI_FOPAGO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CTA_COBRAR> CTA_COBRAR { get; set; }
        public virtual CONFIGURACION CONFIGURACION { get; set; }
        public virtual CONFIGURACION CONFIGURACION1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PROFORMAS> PROFORMAS { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VENTAS> VENTAS { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ORDEN_PEDIDOS> ORDEN_PEDIDOS { get; set; }
        public virtual CONFIGURACION CONFIGURACION2 { get; set; }
    }
}
