using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace SpeedRunApp.Model.Data
{
    public class GameView
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }        
        public string Abbr { get; set; }
        public bool ShowMilliseconds { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int GameLinkID { get; set; }    
        public string CoverImageUrl { get; set; }
        public string SrcUrl { get; set; }
        public string GameCategoryTypesJson { get; set; }
        public string CategoriesJson { get; set; }
        public string LevelsJson { get; set; }
        public string VariablesJson { get; set; }
        public string VariableValuesJson { get; set; }
        public string GamePlatformsJson { get; set; }

        private List<GameCategoryType> _gameCategoryTypes = null;
        public List<GameCategoryType> GameCategoryTypes
        { 
            get
            {
                if (!string.IsNullOrWhiteSpace(GameCategoryTypesJson))
                {
                    _gameCategoryTypes = JsonSerializer.Deserialize<List<GameCategoryType>>(GameCategoryTypesJson);
                }

                return _gameCategoryTypes;
            }
        }  

        private List<Category> _categories = null;
        public List<Category> Categories
        { 
            get
            {
                if (!string.IsNullOrWhiteSpace(CategoriesJson))
                {
                    _categories = JsonSerializer.Deserialize<List<Category>>(CategoriesJson);
                }

                return _categories;
            }
        }        

        private List<Level> _levels = null;
        public List<Level> Levels
        { 
            get
            {
                if (!string.IsNullOrWhiteSpace(LevelsJson))
                {
                    _levels = JsonSerializer.Deserialize<List<Level>>(LevelsJson);
                }

                return _levels;
            }
        }         

        private List<Variable> _variables = null;
        public List<Variable> Variables
        { 
            get
            {
                if (!string.IsNullOrWhiteSpace(VariablesJson))
                {
                    _variables = JsonSerializer.Deserialize<List<Variable>>(VariablesJson);
                }

                return _variables;
            }
        }      

        private List<VariableValue> _variableValues = null;
        public List<VariableValue> VariableValues
        { 
            get
            {
                if (!string.IsNullOrWhiteSpace(VariableValuesJson))
                {
                    _variableValues = JsonSerializer.Deserialize<List<VariableValue>>(VariableValuesJson);
                }

                return _variableValues;
            }
        }

        private List<GamePlatform> _gamePlatforms = null;
        public List<GamePlatform> GamePlatforms
        { 
            get
            {
                if (!string.IsNullOrWhiteSpace(GamePlatformsJson))
                {
                    _gamePlatforms = JsonSerializer.Deserialize<List<GamePlatform>>(GamePlatformsJson);
                }

                return _gamePlatforms;
            }
        }                               
    }
} 
