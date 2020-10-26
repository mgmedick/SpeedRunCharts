using System;
using System.Collections.Generic;
using SpeedRunApp.Model.Entity;

namespace SpeedRunApp.Model.Data
{
    public class Variable : ICloneable
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public VariableScope Scope { get; set; }
        public bool IsMandatory { get; set; }
        public bool IsSubCategory { get; set; }
        public bool IsUserDefined { get; set; }
        public bool IsUsedForObsoletingRuns { get; set; }
        public IEnumerable<VariableValue> Values { get; set; }
        public VariableValue DefaultValue { get; set; }

        #region Links
        public string GameID { get; set; }
        public string CategoryID { get; set; }
        #endregion

        public object Clone()
        {
            Variable variable = (Variable)this.MemberwiseClone();
            variable.Scope = (VariableScope)this.Scope.Clone();
            variable.Values = new List<VariableValue>(this.Values);

            return variable;
        }

        //private SpeedrunComClient client;

        //public Variable() { }

        /*
        public VariableValue CreateCustomValue(string customValue)
        {
            if (!IsUserDefined)
                throw new NotSupportedException("This variable doesn't support custom values.");

            return VariableValue.CreateCustomValue(client, ID, customValue);
        }

        public static Variable Parse(SpeedrunComClient client, dynamic variableElement)
        {
            var variable = new Variable();

            variable.client = client;

            var properties = variableElement.Properties as IDictionary<string, dynamic>;
            var links = properties["links"] as IEnumerable<dynamic>;

            //Parse Attributes

            variable.ID = variableElement.id as string;
            variable.Name = variableElement.name as string;
            variable.Scope = VariableScope.Parse(client, variableElement.scope) as VariableScope;
            variable.IsMandatory = (bool)(variableElement.mandatory ?? false);
            variable.IsUserDefined = (bool)(properties["user-defined"] ?? false);
            variable.IsUsedForObsoletingRuns = (bool)variableElement.obsoletes;

            if (!(variableElement.values.choices is ArrayList))
            {
                var choiceElements = variableElement.values.choices.Properties as IDictionary<string, dynamic>;
                variable.Values = choiceElements.Select(x => VariableValue.ParseIDPair(client, variable, x) as VariableValue).ToList().AsReadOnly();
            }
            else
            {
                variable.Values = new ReadOnlyCollection<VariableValue>(new VariableValue[0]);
            }

            var valuesProperties = variableElement.values.Properties as IDictionary<string, dynamic>;
            var defaultValue = valuesProperties["default"] as string;
            if (!string.IsNullOrEmpty(defaultValue))
                variable.DefaultValue = variable.Values.FirstOrDefault(x => x.ID == defaultValue);

            //Parse Links

            var gameLink = links.FirstOrDefault(x => x.rel == "game");
            if (gameLink != null)
            {
                var gameUri = gameLink.uri as string;
                variable.GameID = gameUri.Substring(gameUri.LastIndexOf("/") + 1);
                variable.game = new Lazy<Game>(() => client.Games.GetGame(variable.GameID));
            }
            else
            {
                variable.game = new Lazy<Game>(() => null);
            }

            variable.CategoryID = variableElement.category as string;
            if (!string.IsNullOrEmpty(variable.CategoryID))
            {
                variable.category = new Lazy<Category>(() => client.Categories.GetCategory(variable.CategoryID));
            }
            else
            {
                variable.category = new Lazy<Category>(() => null);
            }

            if (!string.IsNullOrEmpty(variable.Scope.LevelID))
            {
                variable.level = new Lazy<Level>(() => client.Levels.GetLevel(variable.Scope.LevelID));
            }
            else
            {
                variable.level = new Lazy<Level>(() => null);
            }

            return variable;
        }
        */

        public override int GetHashCode()
        {
            return (ID ?? string.Empty).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = obj as Variable;

            if (other == null)
                return false;

            return ID == other.ID;
        }

        public override string ToString()
        {
            return Name;
        }

        public VariableEntity ConvertToEntity()
        {
            return new VariableEntity
            {
                ID = this.ID,
                Name = this.Name,
                GameID = this.GameID,
                VariableScopeTypeID = (int)this.Scope.Type,
                CategoryID = this.CategoryID,
                LevelID = this.Scope.LevelID
            };
        }
    }
}
