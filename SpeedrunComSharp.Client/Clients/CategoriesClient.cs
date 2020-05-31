using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SpeedrunComSharp.Model;
using System.Linq;
using SpeedRunCommon;

namespace SpeedrunComSharp.Client
{
    public class CategoriesClient : BaseClient
    {

        public const string Name = "categories";

        public CategoriesClient(ClientContainer client) : base(client)
        {
        }

        public static Uri GetCategoriesUri(string subUri)
        {
            return GetAPIUri(string.Format("{0}{1}", Name, subUri));
        }

        public Category GetCategoryFromSiteUri(string siteUri, CategoryEmbeds embeds = default(CategoryEmbeds))
        {
            var id = GetCategoryIDFromSiteUri(siteUri);

            if (string.IsNullOrEmpty(id))
                return null;

            return GetCategory(id, embeds);
        }

        public string GetCategoryIDFromSiteUri(string siteUri)
        {
            var elementDescription = GetElementDescriptionFromSiteUri(siteUri);

            if (elementDescription == null
                || elementDescription.Type != ElementType.Category)
                return null;

            return elementDescription.ID;
        }

        public Category GetCategory(string categoryId, CategoryEmbeds embeds = null)
        {
            var uri = GetCategoriesUri(string.Format("/{0}{1}", Uri.EscapeDataString(categoryId), embeds?.ToString().ToParameters()));
            var result = DoRequest(uri);

            return Parse(result.data);
        }

        public ReadOnlyCollection<Variable> GetVariables(string categoryId,
            VariablesOrdering orderBy = default(VariablesOrdering))
        {
            var parameters = new List<string>(orderBy.ToParameters());

            var uri = GetCategoriesUri(string.Format("/{0}/variables{1}", 
                Uri.EscapeDataString(categoryId), 
                parameters.ToParameters()));

            return DoDataCollectionRequest<Variable>(uri,
                x => Client.Variables.Parse(x));
        }

        public IEnumerable<Leaderboard> GetRecords(string categoryId,
            int? top = null, bool skipEmptyLeaderboards = false,
            int? elementsPerPage = null,
            LeaderboardEmbeds embeds = null)
        {
            var parameters = new List<string>() { embeds?.ToString() };

            if (top.HasValue)
                parameters.Add(string.Format("top={0}", top.Value));
            if (skipEmptyLeaderboards)
                parameters.Add("skip-empty=true");
            if (elementsPerPage.HasValue)
                parameters.Add(string.Format("max={0}", elementsPerPage.Value));

            var uri = GetCategoriesUri(string.Format("/{0}/records{1}",
                Uri.EscapeDataString(categoryId),
                parameters.ToParameters()));

            return DoPaginatedRequest<Leaderboard>(uri,
                x => Client.Leaderboards.Parse(x));
        }

        public Category Parse(dynamic categoryElement)
        {
            if (categoryElement is ArrayList)
                return null;

            var category = new Category();

            //Parse Attributes
            category.ID = categoryElement.id as string;
            category.Name = categoryElement.name as string;
            category.WebLink = new Uri(categoryElement.weblink as string);
            category.Type = categoryElement.type == "per-game" ? CategoryType.PerGame : CategoryType.PerLevel;
            category.Rules = categoryElement.rules as string;
            category.Players = ParsePlayers(categoryElement.players);
            category.IsMiscellaneous = categoryElement.miscellaneous;

            //Parse Links
            var properties = categoryElement.Properties as IDictionary<string, dynamic>;
            var links = properties["links"] as IEnumerable<dynamic>;

            var gameUri = links.First(x => x.rel == "game").uri as string;
            category.GameID = gameUri.Substring(gameUri.LastIndexOf('/') + 1);

            if (properties.ContainsKey("game"))
            {
                var gameElement = properties["game"].data;
                var game = Client.Games.Parse(gameElement);
                category.game = new Lazy<Game>(() => game);
            }
            else
            {
                category.game = new Lazy<Game>(() => Client.Games.GetGame(category.GameID));
            }

            if (properties.ContainsKey("variables"))
            {
                Func<dynamic, Variable> parser = x => Client.Variables.Parse(x) as Variable;
                var variables = ParseCollection(properties["variables"].data, parser);
                category.variables = new Lazy<ReadOnlyCollection<Variable>>(() => variables);
            }
            else
            {
                category.variables = new Lazy<ReadOnlyCollection<Variable>>(() => Client.Categories.GetVariables(category.ID));
            }

            category.runs = new Lazy<IEnumerable<Run>>(() => Client.Runs.GetRuns(categoryId: category.ID));

            if (category.Type == CategoryType.PerGame)
            {
                category.leaderboard = new Lazy<Leaderboard>(() =>
                {
                    var leaderboard = Client.Leaderboards.GetLeaderboardForFullGameCategory(category.GameID, category.ID);

                    leaderboard.game = new Lazy<Game>(() => category.Game);
                    leaderboard.category = new Lazy<Category>(() => category);

                    foreach (var record in leaderboard.Records)
                    {
                        record.game = leaderboard.game;
                        record.category = leaderboard.category;
                    }

                    return leaderboard;
                });
                category.worldRecord = new Lazy<Record>(() =>
                {
                    if (category.leaderboard.IsValueCreated)
                        return category.Leaderboard.Records.FirstOrDefault();

                    var leaderboard = Client.Leaderboards.GetLeaderboardForFullGameCategory(category.GameID, category.ID, top: 1);

                    leaderboard.game = new Lazy<Game>(() => category.Game);
                    leaderboard.category = new Lazy<Category>(() => category);

                    foreach (var record in leaderboard.Records)
                    {
                        record.game = leaderboard.game;
                        record.category = leaderboard.category;
                    }

                    return leaderboard.Records.FirstOrDefault();
                });
            }
            else
            {
                category.leaderboard = new Lazy<Leaderboard>(() => null);
                category.worldRecord = new Lazy<Record>(() => null);
            }

            return category;
        }

        private Players ParsePlayers(dynamic playersElement)
        {
            var players = new Players();

            players.Value = (int)playersElement.value;
            players.Type = playersElement.type == "exactly" ? PlayersType.Exactly : PlayersType.UpTo;

            return players;
        }
    }
}
