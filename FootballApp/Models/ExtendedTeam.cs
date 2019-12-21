using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FootballApp.Models
{
    public class ExtendedTeam
    {
        public Trainer trainer1 { get; set; }
        public Trainer trainer2 { get; set; }
        public string tournament { get; set; }
        public string Stage { get; set; }
        public string Judge { get; set; }
        public DateTime date { get; set; }
        public string Team1 { get; set; }
        public string Team2 { get; set; }
        public List<ExtendedPlayer> Players1 { get; set; }
        public List<ExtendedPlayer> Players2 { get; set; }

        public List<ExtendedGoal> Goals1 { get; set; }
        public List<ExtendedGoal> Goals2 { get; set; }
        public List<ExtendedContravention> Contraventions1 { get; set; }
        public List<ExtendedContravention> Contraventions2 { get; set; }
        public string scores { get; set; }
        public string status { get; set; }
    }
}