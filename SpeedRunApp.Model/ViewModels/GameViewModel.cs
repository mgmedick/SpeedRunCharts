using System;
using SpeedRunApp.Model.Data;
using System.Collections.Generic;
using System.Linq;

namespace SpeedRunApp.Model.ViewModels
{
    public class GameViewModel
    {
        public GameViewModel(GameView game)
        {
            ID = game.ID;
            Name = game.Name;
            Abbr = game.Abbr;
            YearOfRelease = game.YearOfRelease;
            IsChanged = game.IsChanged;
            CoverImageUri = game.CoverImageUrl;
            SpeedRunComLink = game.SpeedRunComUrl;

            if (!string.IsNullOrWhiteSpace(game.CategoryTypes))
            {
                CategoryTypes = new List<IDNamePair>();
                foreach (var categoryType in game.CategoryTypes.Split("^^"))
                {
                    var values = categoryType.Split("|", 2);
                    var categoryTypeTab = new TabItem() { ID = Convert.ToInt32(values[0]), Name = values[1] };
                    CategoryTypes.Add(categoryTypeTab);
                }
            }

            if (!string.IsNullOrWhiteSpace(game.Categories))
            {
                Categories = new List<Category>();
                foreach (var categoryString in game.Categories.Split("^^"))
                {
                    var values = categoryString.Split("|", 5);
                    var category = new Category
                    {
                        ID = Convert.ToInt32(values[0]),
                        CategoryTypeID = Convert.ToInt32((string)values[1]),
                        IsTimerAsc = Convert.ToBoolean((string)values[2]),
                        IsMisc = Convert.ToBoolean((string)values[3]),
                        Name = values[4]
                    };
                    Categories.Add(category);
                }             
            }

            if (!string.IsNullOrWhiteSpace(game.Levels))
            {
                Levels = new List<IDNamePair>();
                foreach (var levelString in game.Levels.Split("^^"))
                {
                    var values = levelString.Split("|", 2);
                    var level = new IDNamePair
                    {
                        ID = Convert.ToInt32(values[0]),
                        Name = values[1]
                    };
                    Levels.Add(level);
                }
            }
            
            if (!string.IsNullOrWhiteSpace(game.Variables))
            {
                Variables = new List<Variable>();
                foreach (var variableString in game.Variables.Split("^^"))
                {
                    var values = variableString.Split("|", 6);
                    var variable = new Variable
                    {
                        ID = Convert.ToInt32(values[0]),
                        IsSubCategory = Convert.ToBoolean(values[1]),
                        ScopeTypeID = Convert.ToInt32((string)values[2]),
                        CategoryID = !String.IsNullOrWhiteSpace((string)values[3]) ? Convert.ToInt32((string)values[3]) : (int?)null,
                        LevelID = !String.IsNullOrWhiteSpace((string)values[4]) ? Convert.ToInt32((string)values[4]) : (int?)null,
                        Name = values[5],
                    };

                    variable.VariableValues = game.VariableValues?.Split("^^")
                                                  .Where(i => i.Split("|", 3)[1] == variable.ID.ToString())
                                                  .Select(i => new VariableValue
                                                  {
                                                      ID = Convert.ToInt32(i.Split("|", 3)[0]),
                                                      Name = i.Split("|", 3)[2]
                                                  }).ToList();

                    Variables.Add(variable);
                }

                Variables.RemoveAll(i => i.VariableValues == null || !i.VariableValues.Any());
            }

            if (!string.IsNullOrWhiteSpace(game.Platforms))
            {
                Platforms = new List<IDNamePair>();
                foreach (var platform in game.Platforms.Split("^^"))
                {
                    var values = platform.Split("|", 2);
                    Platforms.Add(new IDNamePair { ID = Convert.ToInt32(values[0]), Name = values[1] });
                }
            }

            if (!string.IsNullOrWhiteSpace(game.Moderators))
            {
                Moderators = new List<UserNameViewModel>();
                foreach (var moderator in game.Moderators.Split("^^"))
                {
                    var moderatorValue = moderator.Split("¦", 7);
                    int moderatorID;
                    int.TryParse(moderatorValue[0], out moderatorID);                      
                    Moderators.Add(new UserNameViewModel { ID = moderatorID, Name = moderatorValue[1], Abbr = moderatorValue[2], ColorLight = moderatorValue[3], ColorToLight = moderatorValue[4], ColorDark = moderatorValue[5], ColorToDark = moderatorValue[6] });
                }
            }
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public string Abbr { get; set; }
        public string CoverImageUri { get; set; }
        public string SpeedRunComLink { get; set; }
        public int? YearOfRelease { get; set; }
        public bool IsChanged { get; set; }        
        public List<IDNamePair> CategoryTypes { get; set; }
        public List<Category> Categories { get; set; }
        public List<IDNamePair> Levels { get; set; }
        public List<Variable> Variables { get; set; }
        public List<IDNamePair> Platforms { get; set; }
        public List<UserNameViewModel> Moderators { get; set; }
         public List<Variable> SubCategoryVariables
         { 
            get
            {
                return Variables?.Where(i => i.IsSubCategory).ToList();
            }
         }       
        public string PlatformsString
        {
            get
            {
                return Platforms != null ? string.Join(", ", Platforms.Select(i => i.Name)) : null;
            }
        }
    }
}

