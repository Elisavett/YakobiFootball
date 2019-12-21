namespace FootballApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSmth : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TeamInMatches", "Coefficient", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TeamInMatches", "Coefficient", c => c.Double(nullable: false));
        }
    }
}
