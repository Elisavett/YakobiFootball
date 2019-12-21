using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FootballApp.Models
{
    public class PlayersInTeam
    {
        public int PlayersInTeamId { get; set; }
        public int PositionId { get; set; }
        public int TeamId { get; set; }
        public int PlayerId { get; set; }
    }
}