using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace FootballApp.Models
{
    public class Context : DbContext
    {
        public DbSet<Contravention> Contraventions { get; set; }
        public DbSet<ContraventionsInMatch> ContraventionsInMatchs { get; set; }
        public DbSet<Goal> Goals { get; set; }
        public DbSet<Judge> Judges { get; set; }
        public DbSet<JudgesInMatch> JudgesInMatchs { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<PlayerPosition> PlayerPositions { get; set; }
        public DbSet<PlayersInTeam> PlayersInTeams { get; set; }
        public DbSet<Replacement> Replacements { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamInMatch> TeamInMatches { get; set; }
        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<TournamentStage> TournamentStages { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Bet> Bets { get; set; }
        public DbSet<Coefficients> Coefficients { get; set; }
        //public DbSet<TeamsGoalsTournamentStage> TeamsGoalsTournamentStages { get; set; }
    }
    public class UserDbInitializer : DropCreateDatabaseAlways<Context>
    {
        protected override void Seed(Context db)
        {
            Role admin = new Role { Id = 1, Name = "admin" };
            Role user = new Role { Id = 2, Name = "user" };
            db.Roles.Add(admin);
            db.Roles.Add(user);
            db.Users.Add(new User
            {
                Id = 1,
                Email = "somemail@gmail.com",
                Password = "123456",
                RoleId = 1
            });
            base.Seed(db);
        }
    }
}