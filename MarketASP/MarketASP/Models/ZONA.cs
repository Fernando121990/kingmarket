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
    
    public partial class ZONA
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ZONA()
        {
            this.VENDEDOR_ZONA = new HashSet<VENDEDOR_ZONA>();
        }
    
        public long ncode_zona { get; set; }
        public string sdesc_zona { get; set; }
        public Nullable<bool> nesta_zona { get; set; }
        public string suser_zona { get; set; }
        public Nullable<System.DateTime> dfech_zona { get; set; }
        public string susmo_zona { get; set; }
        public Nullable<System.DateTime> dfemo_zona { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VENDEDOR_ZONA> VENDEDOR_ZONA { get; set; }
    }
}
