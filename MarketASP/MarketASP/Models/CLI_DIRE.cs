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
    
    public partial class CLI_DIRE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CLI_DIRE()
        {
            this.CLIDI_CONTAC = new HashSet<CLIDI_CONTAC>();
            this.VENTAS = new HashSet<VENTAS>();
        }
    
        public int ncode_clidire { get; set; }
        public string sdesc_clidire { get; set; }
        public int ncode_cliente { get; set; }
        public string scode_ubigeo { get; set; }
    
        public virtual UBIGEO UBIGEO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CLIDI_CONTAC> CLIDI_CONTAC { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VENTAS> VENTAS { get; set; }
        public virtual CLIENTE CLIENTE { get; set; }
    }
}
