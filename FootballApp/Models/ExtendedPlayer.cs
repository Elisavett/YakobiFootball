using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FootballApp.Models;

namespace FootballApp.Models
{
    public class ExtendedPlayer
    {
        public int PlayerId { get; set; }
        public string FullName { get; set; }
        public int PlayersInTeamId { get; set; }
        public int PositionId { get; set; }
        public string PositionName { get; set; }
        public int TeamId { get; set; }
        public string Name { get; set; }


    }
}