using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace FootballApp.Models
{
    public class Bet
    {
        public int Id { get; set; }
        public bool? satus { get; set; }
        public Match Match { get; set; }
        public Team Team { get; set; }
        public User User { get; set; }
        [Column(TypeName = "money")]
        public decimal Amount { get; set; }
        public double Coefficient { get; set; }
    }
}