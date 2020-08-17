using SpeedRunApp.Model;
using SpeedRunApp.Model.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SpeedRunApp.Client
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

            //Parse Attributes
            var properties = variableElement.Properties as IDictionary<string, dynamic>;
            variable.ID = variableElement.id as string;
            variable.Name = variableElement.name as string;
            variable.Scope = ParseVariableScope(variableElement.scope) as VariableScope;
            variable.IsMandatory = (bool)(variableElement.mandatory ?? false);
            variable.IsUserDefined = (bool)(properties["user-defined"] ?? false);
            variable.IsUsedForObsoletingRuns = (bool)variableElement.obsoletes;

            if (!(variableElement.values.choices is ArrayList))
            {
                var choiceElements = variableElement.values.choices.Properties as IDictionary<string, dynamic>;
                variable.Values = choiceElements.Select(x => ParseVariableValue(variable, x) as VariableValue).ToList();
            }
            else
            {
                variable.Values = new VariableValue[0];
            }

            var valuesProperties = variableElement.values.Properties as IDictionary<string, dynamic>;
            var defaultValue = valuesProperties["default"] as string;
            if (!string.IsNullOrEmpty(defaultValue))
                variable.DefaultValue = variable.Values.FirstOrDefault(x => x.ID == defaultValue);

            variable.CategoryID = variableElement.category as string;

            //Parse Links
            var links = properties["links"] as IEnumerable<dynamic>;
            var gameLink = links.FirstOrDefault(x => x.rel == "game");
            if (gameLink != null)
            {
                var gameUri = gameLink.uri as string;
                variable.GameID = gameUri.Substring(gameUri.LastIndexOf("/") + 1);
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

        //public VariableValue CreateCustomValue(string variableId, string customValue)
        //{
        //    var value = new VariableValue();

        //    value.VariableID = variableId;
        //    value.Value = customValue;

        //    return value;
        //}

        public VariableValueMapping ParseVariableValueMapping(KeyValuePair<string, dynamic> valueElement)
        {
            var valueMapping = new VariableValueMapping();

            valueMapping.VariableID = valueElement.Key;
            valueMapping.VariableValueID = valueElement.Value as string;

            return valueMapping;
        }

        private static VariableValue ParseVariableValue(Variable variable, KeyValuePair<string, dynamic> valueElement)
        {
            var value = new VariableValue();

            value.VariableID = variable.ID;
            value.ID = valueElement.Key as string;
            value.Value = valueElement.Value as string;
            value.Variable = variable;

            return value;
        }
    }
}
