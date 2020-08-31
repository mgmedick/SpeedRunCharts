using SpeedRunApp.Client;
using SpeedRunApp.Interfaces.Services;
using SpeedRunApp.Model.Data;
using SpeedRunApp.Model.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace SpeedRunApp.Service
{
    public class UserService : IUserService
    {
        private readonly ICacheHelper _cacheHelper = null;

        public UserService(ICacheHelper cacheHelper)
        {
            _cacheHelper = cacheHelper;
        }

        public UserDetailsViewModel GetUserDetails(string userID)
        {
            ClientContainer clientContainer = new ClientContainer();
            var user = clientContainer.Users.GetUser(userID);
            user.ProfileImage = clientContainer.Users.GetUserProfileImageUri(user.Name);
            var userVM = new UserDetailsViewModel(user);

            return userVM;
        }

        public IEnumerable<SpeedRunViewModel> GetUserSpeedRuns(string userID, int elementsPerPage, int elementsOffset)
        {
            ClientContainer clientContainer = new ClientContainer();
            var runEmbeds = new SpeedRunEmbeds { EmbedGame = true, EmbedPlayers = true, EmbedCategory = true, EmbedLevel = true, EmbedPlatform = false };
            var runs = clientContainer.Runs.GetRuns(userId: userID, elementsPerPage: elementsPerPage, elementsOffset: elementsOffset, embeds: runEmbeds).ToList();

            var platforms = _cacheHelper.GetPlatforms();
            Dictionary<string, IEnumerable<Variable>> gameVariables = new Dictionary<string, IEnumerable<Variable>>();
            foreach (var run in runs)
            {
                if (!string.IsNullOrWhiteSpace(run.System?.PlatformID))
                {
                    run.System.Platform = platforms.FirstOrDefault(i => i.ID == run.System.PlatformID);
                }

                if (run.VariableValueMappings != null && run.VariableValueMappings.Any())
                {
                    var variables = new List<Variable>();
                    var variableMappings = run.VariableValueMappings.GroupBy(k => k.VariableID).Select(g => new { g.Key, VariableValueIDs = g.Select(i => i.VariableValueID) });
                    foreach (var variableMapping in variableMappings)
                    {
                        var variable = GetVariable(run.GameID, variableMapping.Key, gameVariables); //clientContainer.Variables.GetVariable(variableMapping.Key.ToString());
                        variable.Values = variable.Values.Where(i => variableMapping.VariableValueIDs.Contains(i.ID));
                        variables.Add(variable);
                    }
                    run.Variables = variables;
                }
            }

            var runVMs = runs.Select(i => new SpeedRunViewModel(i));

            return runVMs;
        }

        private Variable GetVariable(string gameID, string variableID, Dictionary<string, IEnumerable<Variable>> gameVariables)
        {
            Variable variable = null;
            ClientContainer clientContainer = new ClientContainer();

            if (gameVariables.ContainsKey(gameID))
            {
                variable = gameVariables[gameID].FirstOrDefault(i => i.ID == variableID);
            }
            else
            {
                var variables = clientContainer.Games.GetVariables(gameID);
                if (variables != null && variables.Any())
                {
                    gameVariables.Add(gameID, variables);
                    variable = variables.FirstOrDefault(i => i.ID == variableID);
                }
            }

            return variable;
        }

        //private string GetExaminerName(string examinerUserID, Dictionary<string, string> examiners)
        //{
        //    string examinerName = null;
        //    ClientContainer clientContainer = new ClientContainer();

        //    if (examiners.ContainsKey(examinerUserID))
        //    {
        //        examinerName = examiners[examinerUserID];
        //    }
        //    else
        //    {
        //        var examiner = clientContainer.Users.GetUser(examinerUserID);
        //        if (examiner != null)
        //        {
        //            examiners.Add(examiner.ID, examiner.Name);
        //            examinerName = examiner.Name;
        //        }
        //    }

        //    return examinerName;
        //}

        //public SpeedRunGridViewModel SearchUserSpeedRunGrid(string userID, List<string> drpCategoryTypes, List<string> drpGames, List<string> drpCategories, List<string> drpLevels)
        //{
        //    var runs = GetUserSpeedRuns(userID);
        //    runs = runs.Where(i => (!drpCategoryTypes.Any() || drpCategoryTypes.Contains(((int)i.Category.Type).ToString()))
        //                                                && (!drpGames.Any() || drpGames.Contains(i.GameID))
        //                                                && (!drpCategories.Any() || drpCategories.Contains(i.CategoryID))
        //                                                && (!drpLevels.Any() || drpLevels.Contains(i.LevelID)))
        //                                                .ToList();

        //    var userSpeedRunGridVM = GetUserSpeedRunGrid(runs);

        //    return userSpeedRunGridVM;
        //}

        //public SpeedRunGridViewModel GetUserSpeedRunGrid(IEnumerable<SpeedRun> speedRuns)
        //{
        //    var categoryTypes = speedRuns?.Select(i => i.Category.Type)
        //                .GroupBy(g => new { g })
        //                .Select(i => new IDNamePair
        //                {
        //                    ID = ((int)i.First()).ToString(),
        //                    Name = i.First().ToString()
        //                })
        //                .OrderBy(i => Convert.ToInt32(i.ID));

        //    var categories = speedRuns?.Select(i => i.Category)
        //                        .GroupBy(g => new { g.ID })
        //                        .Select(i => new CategoryDisplay
        //                        {
        //                            ID = i.First().ID,
        //                            Name = i.First().Name,
        //                            GameID = i.First().GameID,
        //                            CategoryTypeID = ((int)i.First().Type).ToString()
        //                        })
        //                        .OrderBy(i => i.Name);

        //    var games = speedRuns?.Select(i => i.Game)
        //                    .GroupBy(g => new { g.ID })
        //                    .Select(i => new GameDisplay
        //                    {
        //                        ID = i.First().ID,
        //                        Name = i.First().Name,
        //                        CategoryTypeIDs = categories.Where(g => g.GameID == i.First().ID).Select(g => g.CategoryTypeID).Distinct()
        //                    })
        //                    .OrderBy(i => i.Name);

        //    var levels = speedRuns?.Where(i => i.Level != null)
        //                    .Select(i => i.Level)
        //                    .GroupBy(g => new { g.ID })
        //                    .Select(i => new LevelDisplay
        //                    {
        //                        ID = i.First().ID,
        //                        Name = i.First().Name,
        //                        GameID = i.First().GameID
        //                    })
        //                    .OrderBy(i => i.Name);

        //    return new SpeedRunGridViewModel("User", categoryTypes, games, categories, levels, speedRuns.Select(i => new SpeedRunViewModel(i)));
        //}

        //public IEnumerable<SpeedRunViewModel> GetUserSpeedRuns(string userID, string gameID, CategoryType categoryType, string categoryID, string levelID, DateTime? startDate, DateTime? endDate)
        //{
        //    List<SpeedRunViewModel> runVMs = new List<SpeedRunViewModel>();
        //    IEnumerable<SpeedRun> runs = null;
        //    ClientContainer clientContainer = new ClientContainer();
        //    var runEmbeds = new SpeedRunEmbeds { EmbedGame = true, EmbedPlayers = true, EmbedCategory = true, EmbedLevel = true, EmbedPlatform = true };

        //    if (categoryType == CategoryType.PerGame)
        //    {
        //        runs = clientContainer.Runs.GetRuns(userId: userID, gameId: gameID, categoryId: categoryID, embeds: runEmbeds);
        //    }
        //    else
        //    {
        //        runs = clientContainer.Runs.GetRuns(userId: userID, gameId: gameID, categoryId: categoryID, levelId: levelID, embeds: runEmbeds);
        //    }

        //    if (startDate.HasValue)
        //    {
        //        runs = runs.Where(i => i.DateSubmitted.HasValue && i.DateSubmitted.Value >= startDate.Value);
        //    }

        //    if (endDate.HasValue)
        //    {
        //        runs = runs.Where(i => i.DateSubmitted.HasValue && i.DateSubmitted.Value <= endDate.Value);
        //    }

        //    //var gameService = new GamesService();
        //    //var game = gameService.GetGame(gameID);

        //    foreach (var run in runs)
        //    {
        //        var runVM = new SpeedRunViewModel(run);
        //        //if (!string.IsNullOrWhiteSpace(run.Status.ExaminerUserID))
        //        //{
        //        //    var user = game.ModeratorUsers?.FirstOrDefault(i => i.ID == run.Status.ExaminerUserID);
        //        //    if (user != null)
        //        //    {
        //        //        runVM.ExaminerName = user.Name;
        //        //    }
        //        //    else
        //        //    {
        //        //        var examiner = clientContainer.Users.GetUser(run.Status.ExaminerUserID);
        //        //        if (examiner != null)
        //        //        {
        //        //            runVM.ExaminerName = examiner.Name;
        //        //        }
        //        //    }
        //        //}

        //        runVMs.Add(runVM);
        //    }

        //    return runVMs;
        //}
    }
}
