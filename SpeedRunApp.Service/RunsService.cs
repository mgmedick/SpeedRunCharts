using System;
using System.Collections.Generic;
using System.Linq;
using SpeedRunApp.Model;
using SpeedrunComSharp.Client;
using SpeedrunComSharp.Common;
using SpeedrunComSharp.Model;

namespace SpeedRunApp.Service
{
    public class RunsService
    {
        public IEnumerable<RunDTO> GetLatestRuns(int? elementsOffset = null)
        {
            List<RunDTO> runs = new List<RunDTO>();
            ClientContainer clientContainer = new ClientContainer();
            IEnumerable<Run> clientRuns = clientContainer.Runs.GetRuns(status: RunStatusType.New, orderBy: RunsOrdering.DateSubmittedDescending, elementsPerPage: 10, embeds: new RunEmbeds { EmbedGame = true, EmbedPlayers = true, EmbedCategory = true, EmbedLevel = true }, elementsOffset: elementsOffset); ;

            foreach (var run in clientRuns)
            {
                runs.Add(new RunDTO(run));
            }

            return runs.AsEnumerable();
        }
    }
}
