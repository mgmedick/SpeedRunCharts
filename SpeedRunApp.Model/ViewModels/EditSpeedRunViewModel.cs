using SpeedRunApp.Model.Data;
using System.Collections.Generic;
using System;
using System.Linq;

namespace SpeedRunApp.Model.ViewModels
{
    public class EditSpeedRunViewModel
    {
        public EditSpeedRunViewModel(IEnumerable<IDNamePair> statusTypes, IEnumerable<IDNamePair> categoryTypes, IEnumerable<Category> categories, IEnumerable<IDNamePair> levels, IEnumerable<IDNamePair> platforms, IEnumerable<Variable> variables, IEnumerable<Variable> subCategoryVariables, SpeedRunViewModel speedRunVM)
        {
            StatusTypes = statusTypes;
            CategoryTypes = categoryTypes;
            Categories = categories;
            Levels = levels;
            Platforms = platforms;
            Variables = variables;
            SubCategoryVariables = subCategoryVariables;
            SpeedRunVM = speedRunVM;
        }

        public IEnumerable<IDNamePair> StatusTypes { get; set; }
        public IEnumerable<IDNamePair> CategoryTypes { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<IDNamePair> Levels { get; set; }
        public IEnumerable<IDNamePair> Platforms { get; set; }
        public IEnumerable<Variable> Variables { get; set; }
        public IEnumerable<Variable> SubCategoryVariables { get; set; } 
        public SpeedRunViewModel SpeedRunVM { get; set; }
        public IEnumerable<IDNamePair> Players { get { return SpeedRunVM?.Players; } }
        public IEnumerable<IDNamePair> Guests { get { return SpeedRunVM?.Guests; } }        
    }
}


