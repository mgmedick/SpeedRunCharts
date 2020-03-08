using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SpeedrunComSharp.Model
{
    public class Level : IElementWithID
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public Uri WebLink { get; set; }
        public string Rules { get; set; }

        #region Links


        public string GameID { get; set; }
        public Lazy<Game> game { get; set; }
        public Game Game { get { return game.Value; } }
        public Lazy<ReadOnlyCollection<Category>> categories { get; set; }
        public ReadOnlyCollection<Category> Categories { get { return categories.Value; } }
        public Lazy<ReadOnlyCollection<Variable>> variables { get; set; }
        public ReadOnlyCollection<Variable> Variables { get { return variables.Value; } }
        public Lazy<IEnumerable<Run>> runs { get; set; }
        public IEnumerable<Run> Runs { get { return runs.Value; } }
        #endregion

        public Level() { }

        /*
        public static Level Parse(SpeedrunComClient client, dynamic levelElement)
        {
            if (levelElement is ArrayList)
                return null;

            var level = new Level();

            //Parse Attributes

            level.ID = levelElement.id as string;
            level.Name = levelElement.name as string;
            level.WebLink = new Uri(levelElement.weblink as string);
            level.Rules = levelElement.rules;

            //Parse Links

            var properties = levelElement.Properties as IDictionary<string, dynamic>;
            var links = properties["links"] as IEnumerable<dynamic>;

            var gameUri = links.First(x => x.rel == "game").uri as string;
            level.GameID = gameUri.Substring(gameUri.LastIndexOf('/') + 1);
            level.game = new Lazy<Game>(() => client.Games.GetGame(level.GameID));

            if (properties.ContainsKey("categories"))
            {
                Func<dynamic, Category> categoryParser = x => Category.Parse(client, x) as Category;
                ReadOnlyCollection<Category> categories = client.ParseCollection(levelElement.categories.data, categoryParser);
                level.categories = new Lazy<ReadOnlyCollection<Category>>(() => categories);
            }
            else
            {
                level.categories = new Lazy<ReadOnlyCollection<Category>>(() => client.Levels.GetCategories(level.ID));
            }

            if (properties.ContainsKey("variables"))
            {
                Func<dynamic, Variable> variableParser = x => Variable.Parse(client, x) as Variable;
                ReadOnlyCollection<Variable> variables = client.ParseCollection(levelElement.variables.data, variableParser);
                level.variables = new Lazy<ReadOnlyCollection<Variable>>(() => variables);
            }
            else
            {
                level.variables = new Lazy<ReadOnlyCollection<Variable>>(() => client.Levels.GetVariables(level.ID));
            }

            //level.Runs = client.Runs.GetRuns(levelId: level.ID);

            return level;
        }
        */
        public override int GetHashCode()
        {
            return (ID ?? string.Empty).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = obj as Level;

            if (other == null)
                return false;

            return ID == other.ID;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
