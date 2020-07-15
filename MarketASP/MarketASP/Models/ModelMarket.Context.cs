﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class MarketWebEntities : DbContext
    {
        public MarketWebEntities()
            : base("name=MarketWebEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ALMACEN> ALMACEN { get; set; }
        public virtual DbSet<ART_BARRA> ART_BARRA { get; set; }
        public virtual DbSet<ART_PRECIO> ART_PRECIO { get; set; }
        public virtual DbSet<ART_PROVE> ART_PROVE { get; set; }
        public virtual DbSet<ARTICULO> ARTICULO { get; set; }
        public virtual DbSet<CLASE> CLASE { get; set; }
        public virtual DbSet<CLI_DIRE> CLI_DIRE { get; set; }
        public virtual DbSet<CLIDI_CONTAC> CLIDI_CONTAC { get; set; }
        public virtual DbSet<CLIENTE> CLIENTE { get; set; }
        public virtual DbSet<CONFIGURACION> CONFIGURACION { get; set; }
        public virtual DbSet<CONTACTO> CONTACTO { get; set; }
        public virtual DbSet<FAMILIA> FAMILIA { get; set; }
        public virtual DbSet<KARDEX> KARDEX { get; set; }
        public virtual DbSet<LISTA_PRECIO> LISTA_PRECIO { get; set; }
        public virtual DbSet<MARCA> MARCA { get; set; }
        public virtual DbSet<MOVI_DETALLE> MOVI_DETALLE { get; set; }
        public virtual DbSet<MOVIMIENTO> MOVIMIENTO { get; set; }
        public virtual DbSet<PROV_CONTACTO> PROV_CONTACTO { get; set; }
        public virtual DbSet<PROVEEDOR> PROVEEDOR { get; set; }
        public virtual DbSet<TIPO_CAMBIO> TIPO_CAMBIO { get; set; }
        public virtual DbSet<TIPO_MOVIMIENTO> TIPO_MOVIMIENTO { get; set; }
        public virtual DbSet<UBIGEO> UBIGEO { get; set; }
        public virtual DbSet<UMEDIDA> UMEDIDA { get; set; }
    
        public virtual int Pr_tipoCambioExiste(string dfecha_tc, ObjectParameter valor)
        {
            var dfecha_tcParameter = dfecha_tc != null ?
                new ObjectParameter("dfecha_tc", dfecha_tc) :
                new ObjectParameter("dfecha_tc", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Pr_tipoCambioExiste", dfecha_tcParameter, valor);
        }
    }
}
