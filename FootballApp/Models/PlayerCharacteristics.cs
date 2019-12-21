using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FootballApp.Models
{
    public class PlayerCharacteristics
    {
        public int playerId { get; set; }
        public string teamName { get; set; }
        public string playerName { get; set; }
        public string position { get; set; }
        public int goals{ get; set; }
        public int redCardsNum { get; set; }
        public int yellowCardsNum { get; set; }
        public int ContrNum { get; set; }
    }
}