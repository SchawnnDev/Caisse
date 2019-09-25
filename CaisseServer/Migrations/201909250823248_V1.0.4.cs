namespace CaisseServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V104 : DbMigration
    {
        public override void Up()
        {
            AddColumn("public.articles", "ArticleType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("public.articles", "ArticleType");
        }
    }
}
