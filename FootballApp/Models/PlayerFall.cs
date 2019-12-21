using System;
namespace FootballApp.Models
{
    public class PlayerFall
    {
        public int ContraventionsInMatchId { get; set; }
        public int Time { get; set; }
        public int ContraventionId { get; set; }
        public string ContraventionName { get; set; }
        public int MatchId { get; set; }
        public string MatchName { get; set; }
        public int PlayerId { get; set; }
        public string PlayerName { get; set; }
        public string Period { get; set; }
        public int TeamId { get; set; }
    }
}