using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FootballApp.Models
{
    public class AddMatchModel
    {
        public string tournament { get; set; }
        public string tournamentStage { get; set; }
        public Match match { get; set; }
        public int? FirstTeamTrainerId { get; set; }
        public int? SecondTeamTrainerId { get; set; }
        public int FirstTeamId { get; set; }
        public int SecondTeamId { get; set; }
        public List<Tournament> tournaments { get; set; }
        public List<TournamentStage> tournamentStages { get; set; }
        public List<Player> players { get; set; }
        public List<Team> teams { get; set; }
        public List<Trainer> trainers { get; set; }
        public List<Judge> judges { get; set; }
        public List<ExtendedPlayerInTeam> PlayersInFirstTeam { get; set; }
        public List<ExtendedPlayerInTeam> PlayersInSecondTeam { get; set; }
        public List<PlayerGoal> FirstTeamPlayersGoals { get; set; }
        public List<PlayerGoal> SecondTeamPlayersGoals { get; set; }
        public List<PlayerFall> FirstTeamPlayersFalls { get; set; }
        public List<PlayerFall> SecondTeamPlayersFalls { get; set; }
        public List<PlayerPosition> positions { get; set; }
        public string[] statuses { get; set; }
    }
}