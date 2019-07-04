using System.Data.Entity.Migrations;

namespace CaisseServer.Migrations
{
	internal sealed class Configuration : DbMigrationsConfiguration<CaisseServerContext>
	{
		public Configuration()
		{
			AutomaticMigrationsEnabled = false;
		}

		protected override void Seed(CaisseServerContext context)
		{
			//  This method will be called after migrating to the latest version.

			//  You can use the DbSet<T>.AddOrUpdate() helper extension method 
			//  to avoid creating duplicate seed data.
		}
	}
}