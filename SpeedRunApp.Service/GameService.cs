using System;
using SpeedRunApp.Interfaces.Repositories;
using SpeedRunApp.Interfaces.Services;
using SpeedRunApp.Model;
using SpeedRunApp.Model.Data;
using SpeedRunApp.Model.ViewModels;
using System.Collections.Generic;
using System.Linq;
using SpeedRunCommon.Extensions;

namespace SpeedRunApp.Service
{
    public class GamesService : IGameService
    {
        private readonly IGameRepository _gameRepo = null;
        private readonly ISpeedRunRepository _speedRunRepo = null;
        private readonly ICacheService _cacheService = null;

        public GamesService(IGameRepository gameRepo, ISpeedRunRepository speedRunRepo, ICacheService cacheService)
        {
            _gameRepo = gameRepo;
            _speedRunRepo = speedRunRepo;
            _cacheService = cacheService;
        }

        public GameDetailsViewModel GetGameDetails(string gameAbbr, string speedRunCode) {
            var gameDetailsVM = new GameDetailsViewModel();
            var gameVW = _gameRepo.GetGameViews(i => i.Abbr == gameAbbr).FirstOrDefault();
            if (gameVW != null)
            {
                gameDetailsVM = new GameDetailsViewModel(gameVW, speedRunCode);
            }

            return gameDetailsVM;
        }

        public GameDetailsTabViewModel GetLeaderboardTabs(int gameID, string speedRunCode = null)
        {
            var gamevw = _gameRepo.GetGameViews(i => i.ID == gameID).FirstOrDefault();
            var runs = _speedRunRepo.GetSpeedRuns(i => i.GameID == gameID && i.Rank == 1 && !i.Deleted).ToList();
            var gameVM = new GameTabViewModel(gamevw, runs);
            var exportTypes = new List<IDNamePair>() { new IDNamePair() { ID = (int)ExportType.csv, Name = ExportType.csv.ToString() },
                                                       new IDNamePair() { ID = (int)ExportType.json, Name = ExportType.json.ToString() } };
            var gridTabVM = new GameDetailsTabViewModel(new List<GameTabViewModel>() { gameVM }, exportTypes);

            if (!string.IsNullOrWhiteSpace(speedRunCode)) {
                var runVW = _speedRunRepo.GetSpeedRunGridViews(i => i.Code == speedRunCode).FirstOrDefault();
                if(runVW != null) {
                    var subCategoryVariableValueIDs = !string.IsNullOrWhiteSpace(runVW.SubCategoryVariableValueIDs) ? runVW.SubCategoryVariableValueIDs.Split(",").Select(x => Convert.ToInt32(x)).ToList() : new List<int>();
                    runVW.SubCategoryVariableValues = GetSubCategoryVariableValueNames(subCategoryVariableValueIDs, gameVM.SubCategoryVariables);
                    gridTabVM.RunVW = runVW;
                }
            }

            return gridTabVM;
        }
        
        public GameDetailsTabViewModel GetWorldRecordTabs(int gameID)
        {
            var gamevw = _gameRepo.GetGameViews(i => i.ID == gameID).FirstOrDefault();
            var runs = _speedRunRepo.GetSpeedRuns(i => i.GameID == gameID && i.Rank == 1 && !i.Deleted).ToList();
            var gameVM = new GameTabViewModel(gamevw, runs, true);
            var exportTypes = new List<IDNamePair>() { new IDNamePair() { ID = (int)ExportType.csv, Name = ExportType.csv.ToString() },
                                                       new IDNamePair() { ID = (int)ExportType.json, Name = ExportType.json.ToString() } };                            
            var tabVM = new GameDetailsTabViewModel(new List<GameTabViewModel>() { gameVM }, exportTypes);

            return tabVM;
        }

        public GameDetailsTabViewModel GetGameChartTabs(int gameID)
        {
            var gamevw = _gameRepo.GetGameViews(i => i.ID == gameID).FirstOrDefault();
            var runs = _speedRunRepo.GetSpeedRuns(i => i.GameID == gameID && i.Rank == 1 && !i.Deleted).ToList();
            var gameVM = new GameTabViewModel(gamevw, runs, true);
            var tabVM = new GameDetailsTabViewModel(new List<GameTabViewModel>() { gameVM });

            return tabVM;
        }

        private Dictionary<string, string> GetSubCategoryVariableValueNames(List<int> runSubCategoryVariableValueIDs, List<Variable> gameSubCategoryVariables)
        {                
            var SubCategoryVariableValueNames = new Dictionary<string, string>();

            var variableCount = 0;
            if (runSubCategoryVariableValueIDs != null) {
                foreach (var runSubCategoryVariableValueID in runSubCategoryVariableValueIDs) {
                    var variable = gameSubCategoryVariables.FirstOrDefault(i => i.VariableValues.Any(g => g.ID == runSubCategoryVariableValueID));
                    var variableValue = variable?.VariableValues?.FirstOrDefault(i => i.ID == runSubCategoryVariableValueID);
                    
                    if (variable != null && variableValue != null) {
                        SubCategoryVariableValueNames.Add(variable.Name + variableCount, variableValue.Name);
                    }

                    variableCount++;
                }
            }

            return SubCategoryVariableValueNames;
        }

        public IEnumerable<SearchResult> SearchGames(string searchText)
        {
            var results = new List<SearchResult>();

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                searchText = searchText.SanatizeName();
                var searchItems = searchText.Split(' ');
                results = _cacheService.GetGameViews()
                               .Where(i => searchItems.Any(g => i.SantizedName.Contains(g, StringComparison.OrdinalIgnoreCase)))
                               .GroupBy(g => new { g.Name, g.ReleaseDate?.Year })
                               .Select(i => i.First())
                               .Select(i => new { i.ID, i.Name, i.Abbr, i.ReleaseDate, i.CoverImageUrl, 
                                    ContainsPriority = searchItems.Count() - searchItems.Count(g => i.SantizedName.Contains(g, StringComparison.OrdinalIgnoreCase)),
                                    MatchPriority = searchItems.Count() - searchItems.Intersect(i.SantizedName.Split(' '), StringComparer.OrdinalIgnoreCase).Count(),
                                    RemainderPriority = i.SantizedNameNoSpace.Replace(searchItems, string.Empty, StringComparison.OrdinalIgnoreCase).Length
                               })
                               .OrderBy(i => i.ContainsPriority)
                               .ThenBy(i => i.MatchPriority)
                               .ThenBy(i => i.RemainderPriority)
                               .ThenByDescending(i => i.ReleaseDate)
                               .Take(20)       
                               .Select(i => new SearchResult() { Value = i.Abbr, Label = i.Name, LabelSecondary = i.ReleaseDate?.Year.ToString(), ImagePath = i.CoverImageUrl })
                               .ToList();
            }
            
            return results;
        }                   
    }
}

