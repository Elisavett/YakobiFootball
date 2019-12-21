using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FootballApp.Models
{
    public class ExtendedPlayerInTeam
    {
        public int PlayersInTeamId { get; set; }
        public int PositionId { get; set; }
        public string PositionName { get; set; }
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public int PlayerId { get; set; }
        public string PlayerName { get; set; }
    }
}