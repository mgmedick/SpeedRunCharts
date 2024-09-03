using SpeedRunApp.Model.Data;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SpeedRunApp.Model.JSON
{
    public class VariableValueResult
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int VariableID { get; set; }
        public string VariableName { get; set; }
    }
}


