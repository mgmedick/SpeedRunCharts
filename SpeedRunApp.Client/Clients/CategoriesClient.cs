using System;
using System.Collections;
using System.Collections.Generic;
using SpeedRunApp.Model;
using SpeedRunApp.Model.Data;
using System.Linq;

namespace SpeedRunApp.Client
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

        public Category GetCategory(string categoryId, CategoryEmbeds embeds = null)
        {
            var uri = GetCategoriesUri(string.Format("/{0}{1}", Uri.EscapeDataString(categoryId), embeds?.ToString().ToParameters()));
            var result = DoRequest(uri);

            return Parse(result.data);
        }

        public IEnumerable<Variable> GetVariables(string categoryId,
            VariablesOrdering orderBy = default(VariablesOrdering))
        {
            var parameters = new List<string>(orderBy.ToParameters());

            var uri = GetCategoriesUri(string.Format("/{0}/variables{1}", 
                Uri.EscapeDataString(categoryId), 
                parameters.ToParameters()));

            return DoRequest<Variable>(uri, x => Client.Variables.Parse(x));
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

            return DoRequest<Leaderboard>(uri, x => Client.Leaderboards.Parse(x));
        }

        public Category Parse(dynamic categoryElement)
        {
            if (categoryElement is ArrayList)
                return null;

            var category = new Category();
            var properties = categoryElement.Properties as IDictionary<string, dynamic>;

            //Parse Attributes
            category.ID = categoryElement.id as string;
            category.Name = categoryElement.name as string;
            category.WebLink = new Uri(categoryElement.weblink as string);
            category.Type = categoryElement.type == "per-game" ? CategoryType.PerGame : CategoryType.PerLevel;
            category.Rules = categoryElement.rules as string;
            category.Players = ParsePlayers(categoryElement.players);
            category.IsMiscellaneous = categoryElement.miscellaneous;

            //Parse Links
            var links = properties["links"] as IEnumerable<dynamic>;
            var gameUri = links.First(x => x.rel == "game").uri as string;
            category.GameID = gameUri.Substring(gameUri.LastIndexOf('/') + 1);

            //Parse Embeds
            if (properties.ContainsKey("game"))
            {
                var gameElement = properties["game"].data;
                var game = Client.Games.Parse(gameElement);
                category.Game = game;
            }

            if (properties.ContainsKey("variables"))
            {
                Func<dynamic, Variable> parser = x => Client.Variables.Parse(x) as Variable;
                var variables = ParseCollection(properties["variables"].data, parser);
                category.Variables = variables;
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
