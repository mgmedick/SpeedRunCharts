using System;
using SpeedRunApp.Model;
using SpeedRunCommon;

namespace SpeedRunApp.Model.Data
{
    public class VariableScope
    {
        public VariableScopeType Type { get; set; }
        public string LevelID { get; set; }


        //public VariableScope() { }

        /*
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

        public static VariableScope Parse(SpeedrunComClient client, dynamic scopeElement)
        {
            var scope = new VariableScope();

            scope.Type = parseType(scopeElement.type as string);

            if (scope.Type == VariableScopeType.SingleLevel)
            {
                scope.LevelID = scopeElement.level as string;
                scope.level = new Lazy<Level>(() => client.Levels.GetLevel(scope.LevelID));
            }
            else
            {
                scope.level = new Lazy<Level>(() => null);
            }

            return scope;
        }
        */

        //public override string ToString()
        //{
        //    if (Type == VariableScopeType.SingleLevel)
        //        return "Single Level: " + (Level.Name ?? "");
        //    else
        //        return Type.ToString();
        //}
    }
}
