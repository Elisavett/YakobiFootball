using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FootballApp.Models
{
    public class Replacement
    {
        public int ReplacementId { get; set; }
        public DateTime Time { get; set; }
        public string Period { get; set; }
        public int MatchId { get; set; }
        public int ReplacingPlayerId { get; set; }
        public int ReplaceablePlayerId { get; set; }
    }
}