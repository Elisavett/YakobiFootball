using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FootballApp.Models
{
    public class Coefficients
    {
        public int Id { get; set; }
        public int matchId { get; set; }
        public double firstTeamCoef { get; set; }
        public double secondTeamCoef { get; set; }
        public double drawCoef { get; set; }
    }
}