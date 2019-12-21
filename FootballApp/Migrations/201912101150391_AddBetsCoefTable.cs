namespace FootballApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBetsCoefTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Coefficients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        matchId = c.Int(nullable: false),
                        firstTeamCoef = c.Double(nullable: false),
                        secondTeamCoef = c.Double(nullable: false),
                        drawCoef = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Coefficients");
        }
    }
}
