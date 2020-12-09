using SpeedRunApp.Model.Data;
using System.Collections.Generic;
using System;

namespace SpeedRunApp.Model.ViewModels
{
    public class EditSpeedRunViewModel
    {
        public EditSpeedRunViewModel(IEnumerable<IDNamePair> statusTypes, IEnumerable<IDNamePair> categoryTypes, IEnumerable<CategoryDisplay1> categories, IEnumerable<IDNamePair> levels, IEnumerable<IDNamePair> platforms, IEnumerable<VariableDisplay1> variables, SpeedRunViewModel speedRunVM, bool isReadOnly)
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
        public IEnumerable<CategoryDisplay1> Categories { get; set; }
        public IEnumerable<IDNamePair> Levels { get; set; }
        public IEnumerable<IDNamePair> Platforms { get; set; }
        public IEnumerable<VariableDisplay1> Variables { get; set; }
        public SpeedRunViewModel SpeedRunVM { get; set; }
        public bool IsReadOnly { get; set; }
    }
}


