//using SportsStore.Domain.Entities;
//using System.Web.Mvc;

namespace SportsStore.WebUI.Infrastructure.Binders
{
    /*
    public class GameModelBinder:IModelBinder
    {
        private const string sessionKey = "Game";

        public Task BindModelAsync(ModelBindingContext modelBindingContext)
        {
            GameDTO game = null;
            bool sessionExists = modelBindingContext.HttpContext.Session != null;

            if (sessionExists)
            {
                game = (GameDTO)modelBindingContext.HttpContext.Session.GetObjectFromJson<GameDTO>(sessionKey);
            }

            if (game == null)
            {
                game = new GameDTO();

                if (sessionExists)
                {
                    modelBindingContext.HttpContext.Session.SetObjectAsJson(sessionKey, game);
                }
            }

            modelBindingContext.Result = ModelBindingResult.Success(game);

            return Task.CompletedTask;
        }
    }
    */
}