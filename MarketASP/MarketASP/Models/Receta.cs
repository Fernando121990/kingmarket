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
    
    public partial class Receta
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Receta()
        {
            this.RecetaDetalle = new HashSet<RecetaDetalle>();
        }
    
        public long Rec_codigo { get; set; }
        public string Rec_descripcion { get; set; }
        public string Rec_codclase { get; set; }
        public string Rec_codProd { get; set; }
        public Nullable<decimal> Rec_cantidad { get; set; }
        public string Rec_almacen { get; set; }
        public string Rec_tipo { get; set; }
        public Nullable<decimal> Rec_costoOperativo { get; set; }
        public Nullable<int> ncode_alma { get; set; }
    
        public virtual ALMACEN ALMACEN { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RecetaDetalle> RecetaDetalle { get; set; }
    }
}
