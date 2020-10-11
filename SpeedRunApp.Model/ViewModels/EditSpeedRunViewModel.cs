using SpeedRunApp.Model.Data;
using System.Collections.Generic;
using System;

namespace SpeedRunApp.Model.ViewModels
{
    public class EditSpeedRunViewModel
    {
        public EditSpeedRunViewModel(IEnumerable<IDNamePair> statusTypes, IEnumerable<IDNamePair> categoryTypes, IEnumerable<CategoryDisplay> categories, IEnumerable<LevelDisplay> levels, IEnumerable<IDNamePair> platforms, IEnumerable<VariableDisplay> variables, SpeedRunViewModel speedRunVM, bool isReadOnly)
        {
            StatusTypes = statusTypes;
            CategoryTypes = categoryTypes;
            Categories = categories;
            Levels = levels;
            Platforms = platforms;
            Variables = variables;
            SpeedRunVM = speedRunVM;
            IsReadOnly = isReadOnly;
        }

        public IEnumerable<IDNamePair> StatusTypes { get; set; }
        public IEnumerable<IDNamePair> CategoryTypes { get; set; }
        public IEnumerable<CategoryDisplay> Categories { get; set; }
        public IEnumerable<LevelDisplay> Levels { get; set; }
        public IEnumerable<IDNamePair> Platforms { get; set; }
        public IEnumerable<VariableDisplay> Variables { get; set; }
        public SpeedRunViewModel SpeedRunVM { get; set; }
        public bool IsReadOnly { get; set; }
    }
}


