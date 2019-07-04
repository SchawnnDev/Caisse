using System.Data.Entity.Migrations;

namespace CaisseServer.Migrations
{
	public partial class V101 : DbMigration
	{
		public override void Up()
		{
			
			CreateTable(
					"public.article_events",
					c => new
					{
						Id = c.Int(false, true),
						NeededAmount = c.Int(false),
						GivenAmount = c.Int(false),
						GivenItem_Id = c.Int(),
						Item_Id = c.Int(),
						Type_Id = c.Int()
					})
				.PrimaryKey(t => t.Id)
				.ForeignKey("public.articles", t => t.GivenItem_Id)
				.ForeignKey("public.articles", t => t.Item_Id)
				.ForeignKey("public.checkout_types", t => t.Type_Id)
				.Index(t => t.GivenItem_Id)
				.Index(t => t.Item_Id)
				.Index(t => t.Type_Id);
		}

		public override void Down()
		{
			DropForeignKey("public.article_events", "Type_Id", "public.checkout_types");
			DropForeignKey("public.article_events", "Item_Id", "public.articles");
			DropForeignKey("public.article_events", "GivenItem_Id", "public.articles");
			DropIndex("public.article_events", new[] {"Type_Id"});
			DropIndex("public.article_events", new[] {"Item_Id"});
			DropIndex("public.article_events", new[] {"GivenItem_Id"});
			DropTable("public.article_events");
		}
	}
}