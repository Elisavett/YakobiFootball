using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FootballApp.Models
{
    public class ContraventionsInMatch
    {
        public int ContraventionsInMatchId { get; set; }
        public int Time { get; set; }
        public int ContraventionId { get; set; }
        public int MatchId { get; set; }
        public int PlayerId { get; set; }
        public string Period { get; set; }

    }
}