using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FootballApp.Models
{
    public class PlayerGoal
    {
        public int GoalId { get; set; }
        public DateTime Time { get; set; }
        public int Period { get; set; }
        public int MatchId { get; set; }
        public string MatchName { get; set; }
        public string PlayerName { get; set; }
        public int PlayerId { get; set; }
        public int TeamId { get; set; }
    }
}