﻿using SpeedRunApp.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using SpeedRunApp.Model;

namespace SpeedRunApp.Model.ViewModels
{
    public class GameDetailsViewModel
    {
        public GameDetailsViewModel(Game game)
        {
            ID = game.ID;
            Name = game.Name;
            JapaneseName = game.JapaneseName;
            Abbreviation = game.Abbreviation;
            YearOfRelease = game.YearOfRelease;
            CoverImageUri = game.Assets?.CoverLarge?.Uri;
            Moderators = game.ModeratorUsers?.Select(i => new IDNamePair { ID = i.ID, Name = i.Name }).OrderBy(i => i.Name);
            Platforms = game.Platforms.OrderBy(i => i.Name);
            Games = new List<GameDisplay>() { new GameDisplay { ID = game.ID, Name = game.Name, CategoryTypeIDs = game.Categories.Select(i => ((int)i.Type).ToString()).Distinct() } };
            Categories = game.Categories.Select(i => new CategoryDisplay { ID = i.ID, Name = i.Name, CategoryTypeID = ((int)i.Type).ToString(), GameID = i.GameID }).ToList();
            Levels = game.Levels.Select(i => new LevelDisplay { ID = i.ID, Name = i.Name, GameID = i.GameID }).ToList();
            CategoryTypes = game.CategoryTypes.Where(i => Levels.Any() || (int)i != (int)CategoryType.PerLevel).Select(i => new IDNamePair { ID = ((int)i).ToString(), Name = i.ToString() }).ToList();

            if (game.Variables != null && game.Variables.Any())
            {
                var variables = game.Variables.ToList();
                var gameVariables = variables.Where(i => string.IsNullOrWhiteSpace(i.CategoryID) && (i.Scope.Type == VariableScopeType.Global || i.Scope.Type == VariableScopeType.FullGame)).ToList();
                var gameCategories = game.Categories.Where(i => i.Type == CategoryType.PerGame).Reverse();
                foreach (var gameCategory in gameCategories)
                {
                    foreach (var gameVariable in gameVariables)
                    {
                        var variable = (Variable)gameVariable.Clone();
                        variable.CategoryID = gameCategory.ID;
                        variables.Insert(0, variable);
                    }
                }

                var levelVariables = variables.Where(i => string.IsNullOrWhiteSpace(i.CategoryID) && (i.Scope.Type == VariableScopeType.AllLevels || i.Scope.Type == VariableScopeType.SingleLevel)).ToList();
                var levelCategories = game.Categories.Where(i => i.Type == CategoryType.PerLevel).Reverse();
                foreach (var levelCategory in levelCategories)
                {
                    foreach (var levelVariable in levelVariables)
                    {
                        var variable = (Variable)levelVariable.Clone();
                        variable.CategoryID = levelCategory.ID;
                        variables.Insert(0, variable);
                    }
                }

                levelVariables = variables.Where(i => string.IsNullOrWhiteSpace(i.Scope.LevelID) && (i.Scope.Type == VariableScopeType.AllLevels)).ToList();
                foreach (var level in game.Levels)
                {
                    foreach (var levelVariable in levelVariables)
                    {
                        var variable = (Variable)levelVariable.Clone();
                        variable.Scope.LevelID = level.ID;
                        variables.Insert(0, variable);
                    }
                }

                variables.RemoveAll(i => string.IsNullOrWhiteSpace(i.CategoryID) || (i.Scope.Type == VariableScopeType.AllLevels && string.IsNullOrWhiteSpace(i.Scope.LevelID)));
                Variables = variables.Where(i => !i.IsSubCategory).Select(i => new VariableDisplay { ID = i.ID, Name = i.Name, GameID = i.GameID, CategoryID = i.CategoryID, LevelID = i.Scope.LevelID, ScopeTypeID = ((int)i.Scope.Type).ToString(), VariableValues = i.Values.Select(g => new VariableValueDisplay { ID = g.ID, Name = g.Value }) }).ToList();
                SubCategoryVariables = GetNestedVariables(variables.Where(i => i.IsSubCategory));
            }
        }

        public IEnumerable<VariableDisplay> GetNestedVariables(IEnumerable<Variable> variables, int count = 0)
        {
            var results = variables.Skip(count).Take(variables.Count() - count).Select((g, i) => new VariableDisplay
            {
                ID = g.ID,
                Name = g.Name,
                GameID = g.GameID,
                CategoryID = g.CategoryID,
                LevelID = g.Scope.LevelID,
                ScopeTypeID = ((int)g.Scope.Type).ToString(),
                VariableValues = g.Values.Select(h => new VariableValueDisplay
                {
                    ID = h.ID,
                    Name = h.Value,
                    SubVariables = GetNestedVariables(variables.Where(n => n.GameID == g.GameID
                                                                        && n.CategoryID == g.CategoryID
                                                                        && (g.Scope.Type == VariableScopeType.Global
                                                                            || n.Scope.Type == VariableScopeType.Global
                                                                            || (g.Scope.Type == VariableScopeType.FullGame && n.Scope.Type == VariableScopeType.FullGame)
                                                                            || ((g.Scope.Type == VariableScopeType.AllLevels || g.Scope.Type == VariableScopeType.SingleLevel) && (n.Scope.Type == VariableScopeType.AllLevels || n.Scope.Type == VariableScopeType.SingleLevel)))), count + 1)
                })
            });

            return results.GroupBy(i => new { i.GameID, i.CategoryID }).Select(i => i.FirstOrDefault());
        }

        public string ID { get; set; }
        public string Name { get; set; }
        public string JapaneseName { get; set; }
        public string Abbreviation { get; set; }
        public int? YearOfRelease { get; set; }
        public Uri CoverImageUri { get; set; }
        public IEnumerable<IDNamePair> Moderators { get; set; }
        public IEnumerable<Platform> Platforms { get; set; }
        public IEnumerable<IDNamePair> CategoryTypes { get; set; }
        public IEnumerable<GameDisplay> Games { get; set; }
        public IEnumerable<CategoryDisplay> Categories { get; set; }
        public IEnumerable<LevelDisplay> Levels { get; set; }
        public IEnumerable<VariableDisplay> SubCategoryVariables { get; set; }
        public IEnumerable<VariableDisplay> Variables { get; set; }
        public string PlatformsString
        {
            get
            {
                return string.Join(", ", Platforms.Select(i => i.Name));
            }
        }
    }
}
