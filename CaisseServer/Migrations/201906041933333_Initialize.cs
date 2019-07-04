using System.Data.Entity.Migrations;

namespace CaisseServer.Migrations
{
	public partial class Initialize : DbMigration
	{
		public override void Up()
		{
			CreateTable(
					"public.article_max_sell_numbers",
					c => new
					{
						Id = c.Int(false, true),
						Amount = c.Int(false),
						Article_Id = c.Int(),
						Day_Id = c.Int()
					})
				.PrimaryKey(t => t.Id)
				.ForeignKey("public.articles", t => t.Article_Id)
				.ForeignKey("public.days", t => t.Day_Id)
				.Index(t => t.Article_Id)
				.Index(t => t.Day_Id);

			CreateTable(
					"public.articles",
					c => new
					{
						Id = c.Int(false, true),
						Name = c.String(),
						ImageSrc = c.String(),
						Price = c.Decimal(false, 18, 2),
						Position = c.Int(false),
						Color = c.String(),
						ItemType = c.Int(false),
						NeedsCup = c.Boolean(false),
						Active = c.Boolean(false),
						NumberingTracking = c.Boolean(false),
						MaxSellNumberPerDay = c.Int(false),
						Type_Id = c.Int()
					})
				.PrimaryKey(t => t.Id)
				.ForeignKey("public.checkout_types", t => t.Type_Id)
				.Index(t => t.Type_Id);

			CreateTable(
					"public.checkout_types",
					c => new
					{
						Id = c.Int(false, true),
						Name = c.String(),
						Event_Id = c.Int()
					})
				.PrimaryKey(t => t.Id)
				.ForeignKey("public.events", t => t.Event_Id)
				.Index(t => t.Event_Id);

			CreateTable(
					"public.events",
					c => new
					{
						Id = c.Int(false, true),
						Name = c.String(),
						Start = c.DateTime(false),
						End = c.DateTime(false),
						AddressName = c.String(),
						Address = c.String(),
						PostalCodeCity = c.String(),
						Description = c.String(),
						ImageSrc = c.String(),
						Telephone = c.String(),
						Siret = c.String()
					})
				.PrimaryKey(t => t.Id);

			CreateTable(
					"public.days",
					c => new
					{
						Id = c.Int(false, true),
						Start = c.DateTime(false),
						End = c.DateTime(false),
						Color = c.String(),
						Event_Id = c.Int()
					})
				.PrimaryKey(t => t.Id)
				.ForeignKey("public.events", t => t.Event_Id)
				.Index(t => t.Event_Id);

			CreateTable(
					"public.cashiers",
					c => new
					{
						Id = c.Int(false, true),
						Login = c.String(),
						FirstName = c.String(),
						Name = c.String(),
						WasHere = c.Boolean(false),
						Substitute = c.Boolean(false),
						LastActivity = c.DateTime(false),
						Checkout_Id = c.Int()
					})
				.PrimaryKey(t => t.Id)
				.ForeignKey("public.checkouts", t => t.Checkout_Id)
				.Index(t => t.Checkout_Id);

			CreateTable(
					"public.checkouts",
					c => new
					{
						Id = c.Int(false, true),
						Name = c.String(),
						Details = c.String(),
						CheckoutType_Id = c.Int(),
						Owner_Id = c.Int()
					})
				.PrimaryKey(t => t.Id)
				.ForeignKey("public.checkout_types", t => t.CheckoutType_Id)
				.ForeignKey("public.owners", t => t.Owner_Id)
				.Index(t => t.CheckoutType_Id)
				.Index(t => t.Owner_Id);

			CreateTable(
					"public.owners",
					c => new
					{
						Id = c.Int(false, true),
						Login = c.String(),
						FirstName = c.String(),
						Name = c.String(),
						Permissions = c.String(),
						LastLogin = c.DateTime(false),
						LastLogout = c.DateTime(false),
						SuperAdmin = c.Boolean(false),
						Event_Id = c.Int()
					})
				.PrimaryKey(t => t.Id)
				.ForeignKey("public.events", t => t.Event_Id)
				.Index(t => t.Event_Id);

			CreateTable(
					"public.consigns",
					c => new
					{
						Id = c.Int(false, true),
						Amount = c.Int(false),
						Invoice_Id = c.Int()
					})
				.PrimaryKey(t => t.Id)
				.ForeignKey("public.invoices", t => t.Invoice_Id)
				.Index(t => t.Invoice_Id);

			CreateTable(
					"public.invoices",
					c => new
					{
						Id = c.Int(false, true),
						Date = c.DateTime(false),
						GivenMoney = c.Decimal(false, 18, 2),
						Cashier_Id = c.Int(),
						PaymentMethod_Id = c.Int()
					})
				.PrimaryKey(t => t.Id)
				.ForeignKey("public.cashiers", t => t.Cashier_Id)
				.ForeignKey("public.payment_methods", t => t.PaymentMethod_Id)
				.Index(t => t.Cashier_Id)
				.Index(t => t.PaymentMethod_Id);

			CreateTable(
					"public.payment_methods",
					c => new
					{
						Id = c.Int(false, true),
						Name = c.String(),
						Type = c.String(),
						MinFee = c.Decimal(false, 18, 2),
						Event_Id = c.Int()
					})
				.PrimaryKey(t => t.Id)
				.ForeignKey("public.events", t => t.Event_Id)
				.Index(t => t.Event_Id);

			CreateTable(
					"public.operations",
					c => new
					{
						Id = c.Int(false, true),
						Amount = c.Int(false),
						Invoice_Id = c.Int(),
						Item_Id = c.Int()
					})
				.PrimaryKey(t => t.Id)
				.ForeignKey("public.invoices", t => t.Invoice_Id)
				.ForeignKey("public.articles", t => t.Item_Id)
				.Index(t => t.Invoice_Id)
				.Index(t => t.Item_Id);

			CreateTable(
					"public.time_slots",
					c => new
					{
						Id = c.Int(false, true),
						Start = c.DateTime(false),
						End = c.DateTime(false),
						SubstituteActive = c.Boolean(false),
						Pause = c.Boolean(false),
						Cashier_Id = c.Int(),
						Checkout_Id = c.Int(),
						Day_Id = c.Int(),
						Substitute_Id = c.Int()
					})
				.PrimaryKey(t => t.Id)
				.ForeignKey("public.cashiers", t => t.Cashier_Id)
				.ForeignKey("public.checkouts", t => t.Checkout_Id)
				.ForeignKey("public.days", t => t.Day_Id)
				.ForeignKey("public.cashiers", t => t.Substitute_Id)
				.Index(t => t.Cashier_Id)
				.Index(t => t.Checkout_Id)
				.Index(t => t.Day_Id)
				.Index(t => t.Substitute_Id);
		}

		public override void Down()
		{
			DropForeignKey("public.time_slots", "Substitute_Id", "public.cashiers");
			DropForeignKey("public.time_slots", "Day_Id", "public.days");
			DropForeignKey("public.time_slots", "Checkout_Id", "public.checkouts");
			DropForeignKey("public.time_slots", "Cashier_Id", "public.cashiers");
			DropForeignKey("public.operations", "Item_Id", "public.articles");
			DropForeignKey("public.operations", "Invoice_Id", "public.invoices");
			DropForeignKey("public.consigns", "Invoice_Id", "public.invoices");
			DropForeignKey("public.invoices", "PaymentMethod_Id", "public.payment_methods");
			DropForeignKey("public.payment_methods", "Event_Id", "public.events");
			DropForeignKey("public.invoices", "Cashier_Id", "public.cashiers");
			DropForeignKey("public.cashiers", "Checkout_Id", "public.checkouts");
			DropForeignKey("public.checkouts", "Owner_Id", "public.owners");
			DropForeignKey("public.owners", "Event_Id", "public.events");
			DropForeignKey("public.checkouts", "CheckoutType_Id", "public.checkout_types");
			DropForeignKey("public.article_max_sell_numbers", "Day_Id", "public.days");
			DropForeignKey("public.days", "Event_Id", "public.events");
			DropForeignKey("public.article_max_sell_numbers", "Article_Id", "public.articles");
			DropForeignKey("public.articles", "Type_Id", "public.checkout_types");
			DropForeignKey("public.checkout_types", "Event_Id", "public.events");
			DropIndex("public.time_slots", new[] {"Substitute_Id"});
			DropIndex("public.time_slots", new[] {"Day_Id"});
			DropIndex("public.time_slots", new[] {"Checkout_Id"});
			DropIndex("public.time_slots", new[] {"Cashier_Id"});
			DropIndex("public.operations", new[] {"Item_Id"});
			DropIndex("public.operations", new[] {"Invoice_Id"});
			DropIndex("public.payment_methods", new[] {"Event_Id"});
			DropIndex("public.invoices", new[] {"PaymentMethod_Id"});
			DropIndex("public.invoices", new[] {"Cashier_Id"});
			DropIndex("public.consigns", new[] {"Invoice_Id"});
			DropIndex("public.owners", new[] {"Event_Id"});
			DropIndex("public.checkouts", new[] {"Owner_Id"});
			DropIndex("public.checkouts", new[] {"CheckoutType_Id"});
			DropIndex("public.cashiers", new[] {"Checkout_Id"});
			DropIndex("public.days", new[] {"Event_Id"});
			DropIndex("public.checkout_types", new[] {"Event_Id"});
			DropIndex("public.articles", new[] {"Type_Id"});
			DropIndex("public.article_max_sell_numbers", new[] {"Day_Id"});
			DropIndex("public.article_max_sell_numbers", new[] {"Article_Id"});
			DropTable("public.time_slots");
			DropTable("public.operations");
			DropTable("public.payment_methods");
			DropTable("public.invoices");
			DropTable("public.consigns");
			DropTable("public.owners");
			DropTable("public.checkouts");
			DropTable("public.cashiers");
			DropTable("public.days");
			DropTable("public.events");
			DropTable("public.checkout_types");
			DropTable("public.articles");
			DropTable("public.article_max_sell_numbers");
		}
	}
}