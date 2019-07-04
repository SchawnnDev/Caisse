namespace CaisseServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V102 : DbMigration
    {
        public override void Up()
        {
            AddColumn("public.checkout_types", "Type", c => c.Int(nullable: false));
            DropColumn("public.articles", "ItemType");
            DropColumn("public.owners", "Permissions");
        }
        
        public override void Down()
        {
            AddColumn("public.owners", "Permissions", c => c.String());
            AddColumn("public.articles", "ItemType", c => c.Int(nullable: false));
            DropColumn("public.checkout_types", "Type");
        }
    }
}
