using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FootballApp.Models
{
    public class ExtendedContravention
    {
        public int ContraventionId { get; set; }
        public string Name { get; set; }
        public int Time { get; set; }
        public string Period { get; set; }
        public string PlayerName { get; set; }
    }
}