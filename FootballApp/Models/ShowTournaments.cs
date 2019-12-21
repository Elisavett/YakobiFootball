using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FootballApp.Models;

namespace FootballApp.Models
{
    public class ShowTournaments
    {
        public int TournamentId { get; set; }
        public string Name { get; set; }
        public List<TournamentStageDTO> Stages { get; set; }
    }
}