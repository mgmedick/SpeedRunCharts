using System;
using System.Collections.Generic;
using System.Linq;

namespace SpeedRunApp.Model.ViewModels
{
    public class SpeedRunListViewModel1
    {
        public SpeedRunListViewModel1(int elementsPerPage, string loadDateString)
        {
            ElementsPerPage = elementsPerPage;
            LoadDateString = loadDateString;
        }

        public int ElementsPerPage { get; set; }

        public string LoadDateString { get; set; }
    }
}
