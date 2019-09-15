using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SpeedrunComSharp.Model;

namespace SpeedRunApp.Model
{
    public static class Common
    {
        public static IEnumerable<SpeedRunDTO> GetRunsFromLazy(Lazy<IEnumerable<Run>> runs)
        {
            return runs.Value.Select(i => new SpeedRunDTO(i));
        }
    }
}
