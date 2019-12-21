namespace FootballApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Amount = c.Decimal(nullable: false, storeType: "money"),
                        Team_TeamId = c.Int(),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Teams", t => t.Team_TeamId)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.Team_TeamId)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Teams",
                c => new
                    {
                        TeamId = c.Int(nullable: false, identity: true),
                        TeamName = c.String(),
                    })
                .PrimaryKey(t => t.TeamId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(),
                        Password = c.String(),
                        RoleId = c.Int(nullable: false),
                        ballance = c.Decimal(nullable: false, storeType: "money"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Contraventions",
                c => new
                    {
                        ContraventionId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ContraventionId);
            
            CreateTable(
                "dbo.ContraventionsInMatches",
                c => new
                    {
                        ContraventionsInMatchId = c.Int(nullable: false, identity: true),
                        Time = c.DateTime(nullable: false),
                        ContraventionId = c.Int(nullable: false),
                        MatchId = c.Int(nullable: false),
                        PlayerId = c.Int(nullable: false),
                        Period = c.String(),
                    })
                .PrimaryKey(t => t.ContraventionsInMatchId);
            
            CreateTable(
                "dbo.Goals",
                c => new
                    {
                        GoalId = c.Int(nullable: false, identity: true),
                        Time = c.DateTime(nullable: false),
                        Period = c.Int(nullable: false),
                        MatchId = c.Int(nullable: false),
                        PlayerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.GoalId);
            
            CreateTable(
                "dbo.Judges",
                c => new
                    {
                        JudgeId = c.Int(nullable: false, identity: true),
                        FullName = c.String(),
                    })
                .PrimaryKey(t => t.JudgeId);
            
            CreateTable(
                "dbo.JudgesInMatches",
                c => new
                    {
                        JudgesInMatchId = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        MatchId = c.Int(nullable: false),
                        JudgeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.JudgesInMatchId);
            
            CreateTable(
                "dbo.Matches",
                c => new
                    {
                        MatchId = c.Int(nullable: false, identity: true),
                        MatchName = c.String(),
                        Date = c.DateTime(nullable: false),
                        TournamentId = c.Int(nullable: false),
                        TournamentStageId = c.Int(nullable: false),
                        FirstTeamId = c.Int(nullable: false),
                        SecondTeamId = c.Int(nullable: false),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.MatchId);
            
            CreateTable(
                "dbo.PlayerPositions",
                c => new
                    {
                        PlayerPositionId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.PlayerPositionId);
            
            CreateTable(
                "dbo.Players",
                c => new
                    {
                        PlayerId = c.Int(nullable: false, identity: true),
                        FullName = c.String(),
                    })
                .PrimaryKey(t => t.PlayerId);
            
            CreateTable(
                "dbo.PlayersInTeams",
                c => new
                    {
                        PlayersInTeamId = c.Int(nullable: false, identity: true),
                        PositionId = c.Int(nullable: false),
                        TeamId = c.Int(nullable: false),
                        PlayerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PlayersInTeamId);
            
            CreateTable(
                "dbo.Replacements",
                c => new
                    {
                        ReplacementId = c.Int(nullable: false, identity: true),
                        Time = c.DateTime(nullable: false),
                        Period = c.String(),
                        MatchId = c.Int(nullable: false),
                        ReplacingPlayerId = c.Int(nullable: false),
                        ReplaceablePlayerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ReplacementId);
            
            CreateTable(
                "dbo.TeamInMatches",
                c => new
                    {
                        TeamInMatchId = c.Int(nullable: false, identity: true),
                        TrainerId = c.Int(),
                        TeamId = c.Int(nullable: false),
                        Coefficient = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.TeamInMatchId);
            
            CreateTable(
                "dbo.Tournaments",
                c => new
                    {
                        TournamentId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.TournamentId);
            
            CreateTable(
                "dbo.TournamentStages",
                c => new
                    {
                        TournamentStageId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.TournamentStageId);
            
            CreateTable(
                "dbo.Trainers",
                c => new
                    {
                        TrainerId = c.Int(nullable: false, identity: true),
                        FullName = c.String(),
                    })
                .PrimaryKey(t => t.TrainerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bets", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Users", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.Bets", "Team_TeamId", "dbo.Teams");
            DropIndex("dbo.Users", new[] { "RoleId" });
            DropIndex("dbo.Bets", new[] { "User_Id" });
            DropIndex("dbo.Bets", new[] { "Team_TeamId" });
            DropTable("dbo.Trainers");
            DropTable("dbo.TournamentStages");
            DropTable("dbo.Tournaments");
            DropTable("dbo.TeamInMatches");
            DropTable("dbo.Replacements");
            DropTable("dbo.PlayersInTeams");
            DropTable("dbo.Players");
            DropTable("dbo.PlayerPositions");
            DropTable("dbo.Matches");
            DropTable("dbo.JudgesInMatches");
            DropTable("dbo.Judges");
            DropTable("dbo.Goals");
            DropTable("dbo.ContraventionsInMatches");
            DropTable("dbo.Contraventions");
            DropTable("dbo.Roles");
            DropTable("dbo.Users");
            DropTable("dbo.Teams");
            DropTable("dbo.Bets");
        }
    }
}
