using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FootballApp.Models
{
    public class TeamTrainer
    {
        public string teamName { get; set; }
        public string teamGoalsNum { get; set; }
        public string trainerName { get; set; }
        public string bestPlayer { get; set; }
        public int PlayerGoalsNum { get; set; }
        public int PlayerContrNum { get; set; }
    }
}