﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FootballApp.Models
{
    public class Player
    {
        public int PlayerId { get; set; }
        public string FullName { get; set; }
    }
}