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
    
    public partial class Pr_KardexMatTabla_Result
    {
        public long Cod { get; set; }
        public string Cod2 { get; set; }
        public string DescArt { get; set; }
        public string Medida { get; set; }
        public decimal Precio { get; set; }
        public decimal Costo { get; set; }
        public decimal Stock { get; set; }
        public decimal StockReservado { get; set; }
        public decimal StockTransito { get; set; }
        public Nullable<long> ncode_umed { get; set; }
        public bool bafecto_arti { get; set; }
        public bool bisc_arti { get; set; }
        public bool bdscto_arti { get; set; }
        public bool bicbper_arti { get; set; }
        public bool bpercepcion_arti { get; set; }
    }
}
