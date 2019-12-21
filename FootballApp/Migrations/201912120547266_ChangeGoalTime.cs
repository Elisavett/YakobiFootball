namespace FootballApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeGoalTime : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Goals", "Time", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Goals", "Time", c => c.DateTime(nullable: false));
        }
    }
}
