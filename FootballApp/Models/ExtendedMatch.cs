using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FootballApp.Models
{
    public class ExtendedMatch
    {
        public List<Scores> Scores { get; set; }
        public string tournament { get; set; }
        public string tournamentStage { get; set; }
        public int tournamentId { get; set; }
        public int tournamentStageId { get; set; }
    }
}