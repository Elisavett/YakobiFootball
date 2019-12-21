using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FootballApp.Models
{
    public class Schedule
    {
        public string goals { get; set; }
        public string matchName { get; set; }
        public int matchId { get; set; }
        public string FirstTeamName { get; set; }
        public string SecondTeamName { get; set; }
        public int FirstTeamId { get; set; }
        public int SecondTeamId { get; set; }
        public string tournament { get; set; }
        public string tournamentStage { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public double FirstTeamCoef { get; set; }
        public double SecondTeamCoef { get; set; }
        public double DrawCoef { get; set; }
    }
}