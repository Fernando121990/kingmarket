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
    
    public partial class ALMACEN
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ALMACEN()
        {
            this.MOVIMIENTO = new HashSet<MOVIMIENTO>();
            this.MOVIMIENTO1 = new HashSet<MOVIMIENTO>();
            this.KARDEX = new HashSet<KARDEX>();
            this.VENTA_DETALLE = new HashSet<VENTA_DETALLE>();
            this.COMPRAS = new HashSet<COMPRAS>();
            this.COMPRA_DETALLE = new HashSet<COMPRA_DETALLE>();
            this.PROFORMAS = new HashSet<PROFORMAS>();
            this.ORDEN_PEDIDOS = new HashSet<ORDEN_PEDIDOS>();
            this.ORDEN_COMPRAS = new HashSet<ORDEN_COMPRAS>();
            this.GUIA = new HashSet<GUIA>();
            this.GUIA1 = new HashSet<GUIA>();
        }
    
        public int ncode_alma { get; set; }
        public string sdesc_alma { get; set; }
        public bool besta_alma { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MOVIMIENTO> MOVIMIENTO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MOVIMIENTO> MOVIMIENTO1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<KARDEX> KARDEX { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VENTA_DETALLE> VENTA_DETALLE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<COMPRAS> COMPRAS { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<COMPRA_DETALLE> COMPRA_DETALLE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PROFORMAS> PROFORMAS { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ORDEN_PEDIDOS> ORDEN_PEDIDOS { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ORDEN_COMPRAS> ORDEN_COMPRAS { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GUIA> GUIA { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GUIA> GUIA1 { get; set; }
    }
}
