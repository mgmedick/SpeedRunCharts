using System;
using System.Collections.Generic;
using System.Linq;

namespace SpeedRunApp.Model.Data
{
    public class Game
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Abbr { get; set; }
        public bool ShowMilliseconds { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public DateTime? SrcCreatedDate { get; set; }
        public bool Deleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
} 
