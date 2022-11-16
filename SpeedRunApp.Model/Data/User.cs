using System;
using System.Collections.Generic;
using System.Linq;

namespace SpeedRunApp.Model.Data
{
    public class User
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int UserRoleID { get; set; }
        public DateTime? SignUpDate { get; set; }
        public DateTime ImportedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? IsChanged { get; set; }
    }
} 
