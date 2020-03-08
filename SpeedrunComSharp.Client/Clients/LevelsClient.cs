using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SpeedrunComSharp.Model;
using SpeedRunCommon;

namespace SpeedrunComSharp.Client
{

    public class LevelsClient : BaseClient
    {
        public const string Name = "levels";

        public LevelsClient(ClientContainer client) : base(client)
        {
        }

        public static Uri GetLevelsUri(string subUri)
        {
            return GetAPIUri(string.Format("{0}{1}", Name, subUri));
        }

        public Level GetLevelFromSiteUri(string siteUri, LevelEmbeds embeds = default(LevelEmbeds))
        {
            var id = GetLevelIDFromSiteUri(siteUri);

            if (string.IsNullOrEmpty(id))
                return null;

            return GetLevel(id, embeds);
        }

        public string GetLevelIDFromSiteUri(string siteUri)
        {
            var elementDescription = GetElementDescriptionFromSiteUri(siteUri);

            if (elementDescription == null
                || elementDescription.Type != ElementType.Level)
                return null;

            return elementDescription.ID;
        }

        public Level GetLevel(string levelId, 
            LevelEmbeds embeds = default(LevelEmbeds))
        {
            var parameters = new List<string>() { embeds.ToString() };

            var uri = GetLevelsUri(string.Format("/{0}{1}",
                Uri.EscapeDataString(levelId), 
                parameters.ToParameters()));

            var result = DoRequest(uri);

            return Parse(result.data);
        }

        public ReadOnlyCollection<Category> GetCategories(
            string levelId, bool miscellaneous = true,
            CategoryEmbeds embeds = default(CategoryEmbeds),
            CategoriesOrdering orderBy = default(CategoriesOrdering))
        {
            var parameters = new List<string>() { embeds.ToString() };

            parameters.AddRange(orderBy.ToParameters());

            if (!miscellaneous)
                parameters.Add("miscellaneous=no");

            var uri = GetLevelsUri(string.Format("/{0}/categories{1}", 
                Uri.EscapeDataString(levelId), 
                parameters.ToParameters()));

            return DoDataCollectionRequest<Category>(uri,
                x => Client.Categories.Parse(x));
        }

        public ReadOnlyCollection<Variable> GetVariables(string levelId,
            VariablesOrdering orderBy = default(VariablesOrdering))
        {
            var parameters = new List<string>(orderBy.ToParameters());

            var uri = GetLevelsUri(string.Format("/{0}/variables{1}", 
                Uri.EscapeDataString(levelId),
                parameters.ToParameters()));

            return DoDataCollectionRequest<Variable>(uri,
                x => Client.Variables.Parse(x));
        }

        public IEnumerable<Leaderboard> GetRecords(string levelId,
           int? top = null, bool skipEmptyLeaderboards = false,
           int? elementsPerPage = null,
           LeaderboardEmbeds embeds = default(LeaderboardEmbeds))
        {
            var parameters = new List<string>() { embeds.ToString() };

            if (top.HasValue)
                parameters.Add(string.Format("top={0}", top.Value));
            if (skipEmptyLeaderboards)
                parameters.Add("skip-empty=true");
            if (elementsPerPage.HasValue)
                parameters.Add(string.Format("max={0}", elementsPerPage.Value));

            var uri = GetLevelsUri(string.Format("/{0}/records{1}",
                Uri.EscapeDataString(levelId),
                parameters.ToParameters()));

            return DoPaginatedRequest<Leaderboard>(uri,
                x => Client.Leaderboards.Parse(x));
        }

        public Level Parse(dynamic levelElement)
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
            level.game = new Lazy<Game>(() => Client.Games.GetGame(level.GameID));

            if (properties.ContainsKey("categories"))
            {
                Func<dynamic, Category> categoryParser = x => Client.Categories.Parse(x) as Category;
                ReadOnlyCollection<Category> categories = ParseCollection(levelElement.categories.data, categoryParser);
                level.categories = new Lazy<ReadOnlyCollection<Category>>(() => categories);
            }
            else
            {
                level.categories = new Lazy<ReadOnlyCollection<Category>>(() => Client.Levels.GetCategories(level.ID));
            }

            if (properties.ContainsKey("variables"))
            {
                Func<dynamic, Variable> variableParser = x => Client.Variables.Parse(x) as Variable;
                ReadOnlyCollection<Variable> variables = ParseCollection(levelElement.variables.data, variableParser);
                level.variables = new Lazy<ReadOnlyCollection<Variable>>(() => variables);
            }
            else
            {
                level.variables = new Lazy<ReadOnlyCollection<Variable>>(() => Client.Levels.GetVariables(level.ID));
            }

            level.runs = new Lazy<IEnumerable<Run>>(() => Client.Runs.GetRuns(levelId: level.ID));

            return level;
        }
    }
}
