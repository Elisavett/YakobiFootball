using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FootballApp.Models
{
    public class ExtendedGoal
    {
        public string Team { get; set; }
        public int TeamId { get; set; }
        public int? Time { get; set; }
        public int? Period { get; set; }
        public string PlayerName { get; set; }
    }
}