namespace DevCodeGroupCapstone.Migrations
{
    using DevCodeGroupCapstone.Models;
    using Microsoft.AspNet.Identity;
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;

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
