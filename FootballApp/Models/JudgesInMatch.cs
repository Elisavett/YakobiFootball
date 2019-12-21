using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FootballApp.Models
{
    public class JudgesInMatch
    {
        public int JudgesInMatchId { get; set; }
        public string Status { get; set; }
        public int MatchId { get; set; }
        public int JudgeId { get; set; }
    }
}