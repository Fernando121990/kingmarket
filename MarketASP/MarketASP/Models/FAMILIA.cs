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
    
    public partial class FAMILIA
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FAMILIA()
        {
            this.ARTICULO = new HashSet<ARTICULO>();
            this.CLASE = new HashSet<CLASE>();
        }
    
        public int ncode_fami { get; set; }
        public string sdesc_fami { get; set; }
        public Nullable<int> nesta_fami { get; set; }
        public string suser_fami { get; set; }
        public Nullable<System.DateTime> dfech_fami { get; set; }
        public string susmo_fami { get; set; }
        public Nullable<System.DateTime> dfemo_fami { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ARTICULO> ARTICULO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CLASE> CLASE { get; set; }
    }
}
