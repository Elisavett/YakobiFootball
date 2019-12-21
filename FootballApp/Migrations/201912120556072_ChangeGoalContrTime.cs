namespace FootballApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeGoalContrTime : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ContraventionsInMatches", "Time", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ContraventionsInMatches", "Time", c => c.DateTime(nullable: false));
        }
    }
}
