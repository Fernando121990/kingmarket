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
    
    public partial class KARDEX
    {
        public long ncode_kardex { get; set; }
        public System.DateTime dfekard_kardex { get; set; }
        public string stipomovi_kardex { get; set; }
        public decimal ncant_kardex { get; set; }
        public decimal npuco_kardex { get; set; }
        public decimal ntc_kardex { get; set; }
        public string smone_kardex { get; set; }
        public decimal npucoMN_kardex { get; set; }
        public decimal npucoUS_kardex { get; set; }
        public long ncodeDoc_kardex { get; set; }
        public string sserie_kardex { get; set; }
        public string snume_kardex { get; set; }
        public Nullable<System.DateTime> dfvence_kardex { get; set; }
        public string suser_kardex { get; set; }
        public Nullable<System.DateTime> dfech_kardex { get; set; }
        public string susmo_kardex { get; set; }
        public Nullable<System.DateTime> dfemo_kardex { get; set; }
        public int ncode_alma { get; set; }
        public long ncode_arti { get; set; }
        public string smodulo_confi { get; set; }
        public int ncode_confi { get; set; }
        public long ncode_umed { get; set; }
        public Nullable<bool> borpe_kardex { get; set; }
    
        public virtual ALMACEN ALMACEN { get; set; }
        public virtual UMEDIDA UMEDIDA { get; set; }
        public virtual CONFIGURACION CONFIGURACION { get; set; }
        public virtual ARTICULO ARTICULO { get; set; }
    }
}
