namespace FootballApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeGoalContrTimeToInt : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ContraventionsInMatches", "Time", c => c.Int(nullable: false));
            AlterColumn("dbo.Goals", "Time", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ContraventionsInMatches", "Time", c => c.String());
            AlterColumn("dbo.Goals", "Time", c => c.String());
        }
    }
}
