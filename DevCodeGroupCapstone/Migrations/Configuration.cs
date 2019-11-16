namespace DevCodeGroupCapstone.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<DevCodeGroupCapstone.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DevCodeGroupCapstone.Models.ApplicationDbContext context)
        {


        }
    }
}
