namespace FootballApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeBetStatusNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Bets", "satus", c => c.Boolean());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Bets", "satus", c => c.Boolean(nullable: false));
        }
    }
}
