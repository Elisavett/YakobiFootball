using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FootballApp.Models
{
    public class MatchScores
    {
        public int goals{ get; set; }
        public string matchName { get; set; }
        public string teamName { get; set; }
        public int teamId { get; set; }
        public DateTime Date { get; set; }
        public int matchId { get; set; }
        public List<ExtendedGoal> extendedGoals { get; set; }
    }
}