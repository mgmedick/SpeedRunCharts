﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace SpeedRunApp.Model.Entity
{
    public class VariableValueEntity
    {
        public int OrderValue { get; set; }
        public string ID { get; set; }
        public string GameID { get; set; }
        public string VariableID { get; set; }
        public string Value { get; set; }
        public bool IsCustomValue { get; set; }
    }
} 
