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
    
    public partial class CONFIGURACION
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CONFIGURACION()
        {
            this.COMPRAS = new HashSet<COMPRAS>();
            this.COMPRAS1 = new HashSet<COMPRAS>();
            this.KARDEX = new HashSet<KARDEX>();
            this.CTAS_PAGAR = new HashSet<CTAS_PAGAR>();
            this.CTAS_PAGAR1 = new HashSet<CTAS_PAGAR>();
            this.CTA_COBRAR = new HashSet<CTA_COBRAR>();
            this.CTASCO_DETALLE = new HashSet<CTASCO_DETALLE>();
            this.CTASCO_DETALLE1 = new HashSet<CTASCO_DETALLE>();
            this.CTASPA_DETALLE = new HashSet<CTASPA_DETALLE>();
            this.CTASPA_DETALLE1 = new HashSet<CTASPA_DETALLE>();
            this.PROFORMAS = new HashSet<PROFORMAS>();
            this.PROFORMAS1 = new HashSet<PROFORMAS>();
            this.PROVEEDOR = new HashSet<PROVEEDOR>();
            this.CLI_FOPAGO = new HashSet<CLI_FOPAGO>();
            this.CLIENTE = new HashSet<CLIENTE>();
            this.CLIENTE1 = new HashSet<CLIENTE>();
            this.VENTAS = new HashSet<VENTAS>();
            this.VENTAS1 = new HashSet<VENTAS>();
            this.ORDEN_PEDIDOS = new HashSet<ORDEN_PEDIDOS>();
            this.ORDEN_PEDIDOS1 = new HashSet<ORDEN_PEDIDOS>();
            this.CLIENTE2 = new HashSet<CLIENTE>();
            this.ORDEN_COMPRAS = new HashSet<ORDEN_COMPRAS>();
            this.ORDEN_COMPRAS1 = new HashSet<ORDEN_COMPRAS>();
            this.DOCU_SERIE = new HashSet<DOCU_SERIE>();
            this.RecetaDetalle = new HashSet<RecetaDetalle>();
        }
    
        public int ncode_confi { get; set; }
        public string sdesc_confi { get; set; }
        public string svalor_confi { get; set; }
        public bool besta_confi { get; set; }
        public int ntipo_confi { get; set; }
        public string stipo_confi { get; set; }
        public string saux1_confi { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<COMPRAS> COMPRAS { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<COMPRAS> COMPRAS1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<KARDEX> KARDEX { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CTAS_PAGAR> CTAS_PAGAR { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CTAS_PAGAR> CTAS_PAGAR1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CTA_COBRAR> CTA_COBRAR { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CTASCO_DETALLE> CTASCO_DETALLE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CTASCO_DETALLE> CTASCO_DETALLE1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CTASPA_DETALLE> CTASPA_DETALLE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CTASPA_DETALLE> CTASPA_DETALLE1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PROFORMAS> PROFORMAS { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PROFORMAS> PROFORMAS1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PROVEEDOR> PROVEEDOR { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CLI_FOPAGO> CLI_FOPAGO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CLIENTE> CLIENTE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CLIENTE> CLIENTE1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VENTAS> VENTAS { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VENTAS> VENTAS1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ORDEN_PEDIDOS> ORDEN_PEDIDOS { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ORDEN_PEDIDOS> ORDEN_PEDIDOS1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CLIENTE> CLIENTE2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ORDEN_COMPRAS> ORDEN_COMPRAS { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ORDEN_COMPRAS> ORDEN_COMPRAS1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DOCU_SERIE> DOCU_SERIE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RecetaDetalle> RecetaDetalle { get; set; }
    }
}
