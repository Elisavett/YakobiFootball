namespace FootballApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGoalTeam : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Goals", "TeamInMatchId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Goals", "TeamInMatchId");
        }
    }
}
