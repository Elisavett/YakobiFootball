using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FootballApp.Models
{
    public class MatchesPlayersTeams
    {
        public int teamId { get; set; }
        public int playerId { get; set; }
        public int matchId { get; set; }
        public string teamName { get; set; }
        public string matchName { get; set; }
        public string playerName { get; set; }

        public string position { get; set; }
        public DateTime date { get; set; }
        public int? goalId  { get; set; }
        public DateTime? goalTime { get; set; }
        public int? goalPeriod { get; set; }
        public int? contrType { get; set; }
    }
}