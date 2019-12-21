using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FootballApp.Models
{
    public class Goal
    {
        public int GoalId { get; set; }
        public int Time { get; set; }
        public int Period { get; set; }
        public int MatchId { get; set; }
        public int PlayerId { get; set; }
        public int TeamInMatchId { get; set; }
    }
}