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
    
    public partial class GUIA_DETALLE
    {
        public long ncode_guiadet { get; set; }
        public long ncode_arti { get; set; }
        public Nullable<decimal> ncant_guiadet { get; set; }
        public Nullable<decimal> npu_guiadet { get; set; }
        public string suser_guiadet { get; set; }
        public Nullable<System.DateTime> dfech_guiadet { get; set; }
        public string susmo_guiadet { get; set; }
        public Nullable<System.DateTime> dfemo_guiadet { get; set; }
        public long ncode_guia { get; set; }
        public long ncode_umed { get; set; }
    
        public virtual ARTICULO ARTICULO { get; set; }
        public virtual GUIA GUIA { get; set; }
        public virtual UMEDIDA UMEDIDA { get; set; }
    }
}
