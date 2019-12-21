using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FootballApp.Models
{
    public class TournamentStage
    {
        public int TournamentStageId { get; set; }
        public string Name { get; set; }
    }
    public class TournamentStageDTO
    {
        public int TournamentStageId { get; set; }
        public string Name { get; set; }
    }
}