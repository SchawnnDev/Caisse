namespace CaisseServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V103 : DbMigration
    {
        public override void Up()
        {
            AddColumn("public.events", "AddressNumber", c => c.String());
            AddColumn("public.events", "PostalCode", c => c.String());
            AddColumn("public.events", "City", c => c.String());
            DropColumn("public.events", "PostalCodeCity");
        }
        
        public override void Down()
        {
            AddColumn("public.events", "PostalCodeCity", c => c.String());
            DropColumn("public.events", "City");
            DropColumn("public.events", "PostalCode");
            DropColumn("public.events", "AddressNumber");
        }
    }
}
