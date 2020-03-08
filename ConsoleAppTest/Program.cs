using System;
using SpeedrunComSharp;
using System.Collections.Generic;
using SpeedRunCommon;
using SpeedrunComSharp.Client;
using SpeedrunComSharp.Model;

namespace ConsoleAppTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var clientContainer = new ClientContainer();
            //Run run = serviceContainer.Runs.GetRun("y8198j5z");
            //Console.WriteLine(string.Format("RunID: {0}, GameName: {1}", run.ID, run.Game.Name));
            IEnumerable<Run> runs = clientContainer.Runs.GetRuns(status: RunStatusType.New, orderBy: RunsOrdering.DateSubmittedDescending, elementsPerPage: 10, embeds: new RunEmbeds { EmbedGame = true }); ;
            foreach (var run in runs)
            {
                Console.WriteLine(string.Format("RunID: {0}, GameName: {1}", run.ID, run.Game.Name));
            }
        }
    }
}
