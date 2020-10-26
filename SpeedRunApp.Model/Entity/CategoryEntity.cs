using System;
using System.Collections.Generic;
using System.Linq;

namespace SpeedRunApp.Model.Entity
{
    public class CategoryEntity
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Rules { get; set; }
        public string GameID { get; set; }
        public int CategoryTypeID { get; set; }
    }
}

