using System;
using System.Collections.Generic;
using System.Linq;

namespace SpeedRunApp.Model.Entity
{
    public class SpeedRunPlayerEntity
    {
        public string SpeedRunID { get; set; }
        public bool IsUser { get; set; }
        public string UserID { get; set; }
        public string GuestName { get; set; }
    }
} 
