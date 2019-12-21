using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FootballApp.Models
{
    public class Match
    {
        public int MatchId { get; set; }
        public string MatchName { get; set; }
        public DateTime Date { get; set; }
        public int TournamentId { get; set; }
        public int TournamentStageId { get; set; }
        public int FirstTeamId { get; set; }
        public int SecondTeamId { get; set; }

        public string Status{ get; set; }
    }
}