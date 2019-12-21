namespace FootballApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBetMatch : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bets", "Match_MatchId", c => c.Int());
            CreateIndex("dbo.Bets", "Match_MatchId");
            AddForeignKey("dbo.Bets", "Match_MatchId", "dbo.Matches", "MatchId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bets", "Match_MatchId", "dbo.Matches");
            DropIndex("dbo.Bets", new[] { "Match_MatchId" });
            DropColumn("dbo.Bets", "Match_MatchId");
        }
    }
}
