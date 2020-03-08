using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SpeedrunComSharp.Model;
using SpeedRunCommon;

namespace SpeedrunComSharp.Client
{
    public class VariablesClient : BaseClient
    {

        public const string Name = "variables";

        public VariablesClient(ClientContainer client) : base(client)
        {
        }

        public static Uri GetVariablesUri(string subUri)
        {
            return GetAPIUri(string.Format("{0}{1}", Name, subUri));
        }

        public Variable GetVariableFromSiteUri(string siteUri)
        {
            var id = GetVariableIDFromSiteUri(siteUri);

            if (string.IsNullOrEmpty(id))
                return null;

            return GetVariable(id);
        }

        public string GetVariableIDFromSiteUri(string siteUri)
        {
            var elementDescription = GetElementDescriptionFromSiteUri(siteUri);

            if (elementDescription == null
                || elementDescription.Type != ElementType.Variable)
                return null;

            return elementDescription.ID;
        }

        public Variable GetVariable(string variableId)
        {
            var uri = GetVariablesUri(string.Format("/{0}",
                Uri.EscapeDataString(variableId)));

            var result = DoRequest(uri);

            return Parse(result.data);
        }

        public Variable Parse(dynamic variableElement)
        {
            var variable = new Variable();

            //variable.client = client;

            var properties = variableElement.Properties as IDictionary<string, dynamic>;
            var links = properties["links"] as IEnumerable<dynamic>;

            //Parse Attributes

            variable.ID = variableElement.id as string;
            variable.Name = variableElement.name as string;
            variable.Scope = ParseVariableScope(variableElement.scope) as VariableScope;
            variable.IsMandatory = (bool)(variableElement.mandatory ?? false);
            variable.IsUserDefined = (bool)(properties["user-defined"] ?? false);
            variable.IsUsedForObsoletingRuns = (bool)variableElement.obsoletes;

            if (!(variableElement.values.choices is ArrayList))
            {
                var choiceElements = variableElement.values.choices.Properties as IDictionary<string, dynamic>;
                variable.Values = choiceElements.Select(x => ParseIDPair(variable, x) as VariableValue).ToList().AsReadOnly();
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
                variable.game = new Lazy<Game>(() => Client.Games.GetGame(variable.GameID));
            }
            else
            {
                variable.game = new Lazy<Game>(() => null);
            }

            variable.CategoryID = variableElement.category as string;
            if (!string.IsNullOrEmpty(variable.CategoryID))
            {
                variable.category = new Lazy<Category>(() => Client.Categories.GetCategory(variable.CategoryID));
            }
            else
            {
                variable.category = new Lazy<Category>(() => null);
            }

            if (!string.IsNullOrEmpty(variable.Scope.LevelID))
            {
                variable.level = new Lazy<Level>(() => Client.Levels.GetLevel(variable.Scope.LevelID));
            }
            else
            {
                variable.level = new Lazy<Level>(() => null);
            }

            return variable;
        }

        public VariableScope ParseVariableScope(dynamic scopeElement)
        {
            var scope = new VariableScope();

            scope.Type = parseType(scopeElement.type as string);

            if (scope.Type == VariableScopeType.SingleLevel)
            {
                scope.LevelID = scopeElement.level as string;
                scope.level = new Lazy<Level>(() => Client.Levels.GetLevel(scope.LevelID));
            }
            else
            {
                scope.level = new Lazy<Level>(() => null);
            }

            return scope;
        }

        private static VariableScopeType parseType(string type)
        {
            switch (type)
            {
                case "global":
                    return VariableScopeType.Global;
                case "full-game":
                    return VariableScopeType.FullGame;
                case "all-levels":
                    return VariableScopeType.AllLevels;
                case "single-level":
                    return VariableScopeType.SingleLevel;
            }

            throw new ArgumentException("type");
        }

        //public VariableValue CreateCustomValue(string customValue)
        //{
        //    if (!IsUserDefined)
        //        throw new NotSupportedException("This variable doesn't support custom values.");

        //    return VariableValue.CreateCustomValue(baseClient, ID, customValue);
        //}

        public VariableValue CreateCustomValue(string variableId, string customValue)
        {
            var value = new VariableValue();

            value.VariableID = variableId;

            value.variable = new Lazy<Variable>(() => Client.Variables.GetVariable(value.VariableID));
            value.value = new Lazy<string>(() => customValue);

            return value;
        }

        public VariableValue ParseValueDescriptor(KeyValuePair<string, dynamic> valueElement)
        {
            var value = new VariableValue();

            value.VariableID = valueElement.Key;
            value.ID = valueElement.Value as string;

            //Parse Links

            value.variable = new Lazy<Variable>(() => Client.Variables.GetVariable(value.VariableID));
            value.value = new Lazy<string>(() => value.Variable.Values.FirstOrDefault(x => x.ID == value.ID).Value);

            return value;
        }

        private static VariableValue ParseIDPair(Variable variable, KeyValuePair<string, dynamic> valueElement)
        {
            var value = new VariableValue();

            value.VariableID = variable.ID;
            value.ID = valueElement.Key as string;

            //Parse Links

            value.variable = new Lazy<Variable>(() => variable);

            var valueName = valueElement.Value as string;
            value.value = new Lazy<string>(() => valueName);

            return value;
        }
    }
}
