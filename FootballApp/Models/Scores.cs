using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FootballApp.Models
{
    public class Scores
    {
        public string goals { get; set; }
        public string match { get; set; }
        public int matchId { get; set; }
        public DateTime date { get; set; }
        public int firstTeamId { get; internal set; }
        public int secondTeamId { get; internal set; }
        public List<ExtendedGoal> goalsMadeBy1 { get; set; }
        public List<ExtendedGoal> goalsMadeBy2 { get; set; }
    }
}