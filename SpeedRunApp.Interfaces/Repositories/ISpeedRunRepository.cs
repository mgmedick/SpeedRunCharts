using System;
using System.Collections.Generic;
using SpeedRunApp.Model;
using SpeedRunApp.Model.Entity;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace SpeedRunApp.Interfaces.Repositories
{
    public interface ISpeedRunRepository
    {
        //void UpdateSpeedRunStatus(IEnumerable<SpeedRunEntity> speedRuns, RunStatusType statusType);
        //void UpdateSpeedRunStatusAndRejectReason(IEnumerable<SpeedRunEntity> speedRuns);
        void CopySpeedRunTables();
        void RenameAndDropSpeedRunTables();
        void InsertSpeedRuns(IEnumerable<SpeedRunEntity> speedRuns, IEnumerable<SpeedRunVariableValueEntity> variableValues, IEnumerable<SpeedRunPlayerEntity> players, IEnumerable<SpeedRunVideoEntity> videos);
        void SaveSpeedRuns(IEnumerable<SpeedRunEntity> speedRuns, IEnumerable<SpeedRunVariableValueEntity> variableValues, IEnumerable<SpeedRunPlayerEntity> players, IEnumerable<SpeedRunVideoEntity> videos);
        IEnumerable<SpeedRunEntity> GetSpeedRuns(Expression<Func<SpeedRunEntity, bool>> predicate);
    }
}






