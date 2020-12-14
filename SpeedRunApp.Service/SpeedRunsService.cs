using Microsoft.Extensions.Configuration;
using SpeedRunApp.Interfaces.Repositories;
using SpeedRunApp.Interfaces.Services;
using SpeedRunApp.Model;
using SpeedRunApp.Model.ViewModels;
using SpeedRunApp.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpeedRunApp.Service
{
    public class SpeedRunsService : ISpeedRunsService
    {
        private readonly IConfiguration _config = null;
        private readonly IGamesService _gamesService = null;
        private readonly IUserService _userService = null;
        private readonly ISpeedRunRepository _speedRunRepo = null;

        public SpeedRunsService(IConfiguration config, IGamesService gamesService, IUserService userService, ISpeedRunRepository speedRunRepo)
        {
            _config = config;
            _gamesService = gamesService;
            _userService = userService;
            _speedRunRepo = speedRunRepo;
        }

        public SpeedRunListViewModel GetSpeedRunList()
        {
            var elementsPerPage = Convert.ToInt32(_config.GetSection("ApiSettings").GetSection("SpeedRunListElementsPerPage").Value);

            var runListVM = new SpeedRunListViewModel(elementsPerPage);

            return runListVM;
        }

        public IEnumerable<SpeedRunViewModel> GetLatestSpeedRuns(SpeedRunListCategory1 category, int topAmount, int? orderValueOffset)
        {
            var runs = _speedRunRepo.GetLatestSpeedRuns(category, topAmount, orderValueOffset);
            IEnumerable<SpeedRunViewModel> runVMs = runs.Select(i => new SpeedRunViewModel(i));

            return runVMs;
        }

        public EditSpeedRunViewModel GetEditSpeedRun(string runID, string gameID, bool isReadOnly)
        {
            var run = _speedRunRepo.GetSpeedRunView(runID);
            var runVM = new SpeedRunViewModel(run);

            var gameDetails = _gamesService.GetGameDetails(gameID);
            var statusTypes = _speedRunRepo.RunStatusTypes();

            var editSpeedRunVM = new EditSpeedRunViewModel(statusTypes, gameDetails.CategoryTypes, gameDetails.Categories, gameDetails.Levels, gameDetails.Platforms, gameDetails.Variables, runVM, isReadOnly);

            return editSpeedRunVM;
        }

        public IEnumerable<SpeedRunView> GetSpeedRunRecordsByGameID(string gameID)
        {
            return _speedRunRepo.GetSpeedRuns(i => i.GameID == gameID && i.StatusTypeID == (int)RunStatusType.Verified).ToList();
        }

        /*
        public IEnumerable<SpeedRunViewModel> GetLeaderboards(IEnumerable<SpeedRunGridItem> gridItems)
        {
            List<Tuple<string,string,string,string>> leaderboardParams = new List<Tuple<string, string, string, string>>();

            foreach (var gridItem in gridItems)
            {
                foreach (var category in gridItem.Categories)
                {
                    if (category.CategoryTypeID == (int)CategoryType.PerGame)
                    {
                        var varaibles = gridItem.SubCategoryVariables?
                                                .Where(i => i.CategoryID == category.ID && (i.ScopeTypeID == (int)VariableScopeType.Global || i.ScopeTypeID == (int)VariableScopeType.FullGame))
                                                .ToList();

                        if (varaibles != null && varaibles.Any())
                        {
                            var variableValues = GetVariableValueStrings(varaibles);
                            foreach (var variableValue in variableValues)
                            {
                                leaderboardParams.Add(new Tuple<string, string, string, string>(gridItem.GameID, category.ID, null, variableValue));
                            }
                        }
                        else
                        {
                            leaderboardParams.Add(new Tuple<string, string, string, string>(gridItem.GameID, category.ID, null, null));
                        }
                    }
                    else
                    {
                        foreach (var level in gridItem.Levels)
                        {
                            var varaibles = gridItem.SubCategoryVariables?
                                                    .Where(i => i.CategoryID == category.ID && i.LevelID == level.ID && (i.ScopeTypeID == (int)VariableScopeType.AllLevels || i.ScopeTypeID == (int)VariableScopeType.SingleLevel))
                                                    .ToList();

                            if (varaibles != null && varaibles.Any())
                            {
                                var variableValues = GetVariableValueStrings(varaibles);
                                foreach (var variableValue in variableValues)
                                {
                                    leaderboardParams.Add(new Tuple<string, string, string, string>(gridItem.GameID, category.ID, level.ID, variableValue));
                                }
                            }
                            else
                            {
                                leaderboardParams.Add(new Tuple<string, string, string, string>(gridItem.GameID, category.ID, level.ID, null));
                            }
                        }
                    }
                }
            }

            var leaderboardParamString = string.Join(",", leaderboardParams.Select(i => i.Item1 + "|" + i.Item2 + "|" + (i.Item3 ?? string.Empty) + "|" + (i.Item4 ?? string.Empty)));
            var runs = _speedRunRepo.GetLeaderboards(leaderboardParamString);
            var runVMs = runs.Select(i => new SpeedRunViewModel(i));

            return runVMs;
        }

        private IEnumerable<string> GetVariableValueStrings(IEnumerable<Variable> variables, List<string> variableValueStrings = null, string result = null)
        {
            if (variableValueStrings == null)
            {
                variableValueStrings = new List<string>();
            }

            if(string.IsNullOrWhiteSpace(result))
            {
                result = string.Empty;
            }

            foreach (var variable in variables)
            {
                foreach (var variableValue in variable.VariableValues)
                {
                    result += variable.ID + "||" + variableValue.Name + "^^";

                    if (variableValue.SubVariables != null)
                    {
                        GetVariableValueStrings(variableValue.SubVariables, variableValueStrings, result);
                    }

                    variableValueStrings.Add(result.Trim('^'));
                }
            }

            return variableValueStrings;
        }
        */
    }
}
