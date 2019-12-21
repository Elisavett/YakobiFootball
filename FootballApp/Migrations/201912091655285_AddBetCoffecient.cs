namespace FootballApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBetCoffecient : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bets", "Coefficient", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bets", "Coefficient");
        }
    }
}
