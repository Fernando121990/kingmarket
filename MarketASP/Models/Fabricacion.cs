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
    
    public partial class Fabricacion
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Fabricacion()
        {
            this.FabricacionDetalle = new HashSet<FabricacionDetalle>();
        }
    
        public string Fab_Tipo { get; set; }
        public string Fab_NroDoc { get; set; }
        public Nullable<System.DateTime> Fab_Fecha { get; set; }
        public Nullable<decimal> Fab_TipoCambio { get; set; }
        public string Fab_Glosa { get; set; }
        public string Fab_Lote { get; set; }
        public string Fab_Estado { get; set; }
        public string Fab_TipoProd { get; set; }
        public Nullable<System.DateTime> Fab_Fvenc { get; set; }
        public Nullable<decimal> Fab_Cantidad { get; set; }
        public long Fab_Codigo { get; set; }
        public Nullable<long> Rec_Codigo { get; set; }
        public Nullable<decimal> Fab_CostoUnit { get; set; }
        public Nullable<decimal> Fab_CostoTotalMN { get; set; }
        public Nullable<decimal> Fab_CostoTotalUS { get; set; }
        public Nullable<decimal> Fab_CostoOperativo { get; set; }
        public Nullable<long> Fab_almacen { get; set; }
        public Nullable<int> ncode_alma { get; set; }
    
        public virtual ALMACEN ALMACEN { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FabricacionDetalle> FabricacionDetalle { get; set; }
    }
}
