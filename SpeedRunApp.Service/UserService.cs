using System.Collections.Generic;
using System.Linq;
using System;
using SpeedRunApp.Model;
using Microsoft.Extensions.Caching.Memory;
using SpeedRunApp.Client;
using SpeedRunApp.Model.Data;
using SpeedRunApp.Model.ViewModels;
using SpeedRunApp.Interfaces.Services;
using SpeedRunCommon;

namespace SpeedRunApp.Service
{
    public class UserService : IUserService
    {
        private readonly IMemoryCache _cache = null;

        public UserService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public UserDetailsViewModel GetUserDetails(string userID)
        {
            var user = GetUser(userID);
            var userVM = new UserDetailsViewModel(user);
            var runs = GetUserSpeedRuns(userID);
            userVM.SpeedRunGridVM = GetUserSpeedRunGrid(runs);

            return userVM;
        }

        public User GetUser(string userID)
        {
            ClientContainer clientContainer = new ClientContainer();
            var user = clientContainer.Users.GetUser(userID);
            user.ProfileImage = clientContainer.Users.GetUserProfileImageUri(user.Name);

            return user;
        }


        public SpeedRunGridViewModel SearchUserSpeedRunGrid(string userID, List<string> drpCategoryTypes, List<string> drpGames, List<string> drpCategories, List<string> drpLevels)
        {
            var runs = GetUserSpeedRuns(userID);
            runs = runs.Where(i => (!drpCategoryTypes.Any() || drpCategoryTypes.Contains(((int)i.Category.Type).ToString()))
                                                        && (!drpGames.Any() || drpGames.Contains(i.GameID))
                                                        && (!drpCategories.Any() || drpCategories.Contains(i.CategoryID))
                                                        && (!drpLevels.Any() || drpLevels.Contains(i.LevelID)))
                                                        .ToList();

            var userSpeedRunGridVM = GetUserSpeedRunGrid(runs);

            return userSpeedRunGridVM;
        }

        public SpeedRunGridViewModel GetUserSpeedRunGrid(IEnumerable<SpeedRun> speedRuns)
        {
            var categoryTypes = speedRuns?.Select(i => i.Category.Type)
                        .GroupBy(g => new { g })
                        .Select(i => new IDNamePair
                        {
                            ID = ((int)i.First()).ToString(),
                            Name = i.First().ToString()
                        })
                        .OrderBy(i => Convert.ToInt32(i.ID));

            var categories = speedRuns?.Select(i => i.Category)
                                .GroupBy(g => new { g.ID })
                                .Select(i => new CategoryDisplay
                                {
                                    ID = i.First().ID,
                                    Name = i.First().Name,
                                    GameID = i.First().GameID,
                                    CategoryTypeID = ((int)i.First().Type).ToString()
                                })
                                .OrderBy(i => i.Name);

            var games = speedRuns?.Select(i => i.Game)
                            .GroupBy(g => new { g.ID })
                            .Select(i => new GameDisplay
                            {
                                ID = i.First().ID,
                                Name = i.First().Name,
                                CategoryTypeIDs = categories.Where(g => g.GameID == i.First().ID).Select(g => g.CategoryTypeID).Distinct()
                            })
                            .OrderBy(i => i.Name);

            var levels = speedRuns?.Where(i => i.Level != null)
                            .Select(i => i.Level)
                            .GroupBy(g => new { g.ID })
                            .Select(i => new LevelDisplay
                            {
                                ID = i.First().ID,
                                Name = i.First().Name,
                                GameID = i.First().GameID
                            })
                            .OrderBy(i => i.Name);

            return new SpeedRunGridViewModel("User", categoryTypes, games, categories, levels, speedRuns.Select(i => new SpeedRunViewModel(i)));
        }

        public IEnumerable<SpeedRunViewModel> GetUserSpeedRuns(string userID, string gameID, CategoryType categoryType, string categoryID, string levelID, DateTime? startDate, DateTime? endDate)
        {
            List<SpeedRunViewModel> runVMs = new List<SpeedRunViewModel>();
            IEnumerable<SpeedRun> runs = null;
            ClientContainer clientContainer = new ClientContainer();
            var runEmbeds = new SpeedRunEmbeds { EmbedGame = true, EmbedPlayers = true, EmbedCategory = true, EmbedLevel = true, EmbedPlatform = true };

            if (categoryType == CategoryType.PerGame)
            {
                runs = clientContainer.Runs.GetRuns(userId: userID, gameId: gameID, categoryId: categoryID, embeds: runEmbeds);
            }
            else
            {
                runs = clientContainer.Runs.GetRuns(userId: userID, gameId: gameID, categoryId: categoryID, levelId: levelID, embeds: runEmbeds);
            }

            if (startDate.HasValue)
            {
                runs = runs.Where(i => i.DateSubmitted.HasValue && i.DateSubmitted.Value >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                runs = runs.Where(i => i.DateSubmitted.HasValue && i.DateSubmitted.Value <= endDate.Value);
            }

            var gameService = new GamesService();
            var game = gameService.GetGame(gameID);

            foreach (var run in runs)
            {
                var runVM = new SpeedRunViewModel(run);
                if (!string.IsNullOrWhiteSpace(run.Status.ExaminerUserID))
                {
                    var user = game.ModeratorUsers?.FirstOrDefault(i => i.ID == run.Status.ExaminerUserID);
                    if (user != null)
                    {
                        runVM.ExaminerName = user.Name;
                    }
                    else
                    {
                        var examiner = clientContainer.Users.GetUser(run.Status.ExaminerUserID);
                        if (examiner != null)
                        {
                            runVM.ExaminerName = examiner.Name;
                        }
                    }
                }

                runVMs.Add(runVM);
            }

            return runVMs;
        }

        public IEnumerable<SpeedRun> GetUserSpeedRuns(string userID)
        {
            ClientContainer clientContainer = new ClientContainer();
            var runEmbeds = new SpeedRunEmbeds { EmbedGame = true, EmbedPlayers = true, EmbedCategory = true, EmbedLevel = true, EmbedPlatform = true };
            var runs = clientContainer.Runs.GetRuns(userId: userID, embeds: runEmbeds).ToList();
            runs.AddRange(clientContainer.Runs.GetRuns(userId: userID, status: RunStatusType.New, embeds: runEmbeds));

            return runs;
        }
    }
}
