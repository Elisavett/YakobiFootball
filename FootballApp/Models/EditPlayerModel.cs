using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FootballApp.Models
{
    public class EditPlayerModel
    {
        public PlayerCharacteristics PlayerCharacteristics { get; set;}
        public List<Team> Teams { get; set; }
        public List<PlayerPosition> PlayerPositions { get; set; }
    }
}