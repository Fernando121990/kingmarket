namespace MarketASP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "OrganizationId", c => c.Long());
            AddColumn("dbo.AspNetUsers", "Nombre", c => c.String());
            AddColumn("dbo.AspNetUsers", "Apellido", c => c.String());
            AddColumn("dbo.AspNetUsers", "Local", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Local");
            DropColumn("dbo.AspNetUsers", "Apellido");
            DropColumn("dbo.AspNetUsers", "Nombre");
            DropColumn("dbo.AspNetUsers", "OrganizationId");
        }
    }
}
