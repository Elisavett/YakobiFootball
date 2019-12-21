using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FootballApp.Models
{
    public class TeamInMatch
    {
        public int TeamInMatchId { get; set; }
        public int? TrainerId { get; set; }
        public int TeamId { get; set; }
        public double? Coefficient { get; set; }
    }
}