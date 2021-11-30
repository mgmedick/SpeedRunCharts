using SpeedRunApp.Model.Data;
using System.Collections.Generic;
using System;
using System.Linq;

namespace SpeedRunApp.Model.ViewModels
{
    public class EditSpeedRunViewModel
    {
        public EditSpeedRunViewModel(IEnumerable<IDNamePair> statusTypes, IEnumerable<IDNamePair> players, IEnumerable<IDNamePair> guests, IEnumerable<IDNamePair> categoryTypes, IEnumerable<Category> categories, IEnumerable<IDNamePair> levels, IEnumerable<IDNamePair> platforms, IEnumerable<Variable> variables, SpeedRunViewModel speedRunVM)
        {
            StatusTypes = statusTypes;
            Players = players;
            Guests = guests;
            CategoryTypes = categoryTypes;
            Categories = categories;
            Levels = levels;
            Platforms = platforms;
            Variables = variables;
            SpeedRunVM = speedRunVM;
        }

        public IEnumerable<IDNamePair> StatusTypes { get; set; }
        public IEnumerable<IDNamePair> Players { get; set; }
        public IEnumerable<IDNamePair> Guests { get; set; }
        public IEnumerable<IDNamePair> CategoryTypes { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<IDNamePair> Levels { get; set; }
        public IEnumerable<IDNamePair> Platforms { get; set; }
        public IEnumerable<Variable> Variables { get; set; }
        public SpeedRunViewModel SpeedRunVM { get; set; }
    }
}


