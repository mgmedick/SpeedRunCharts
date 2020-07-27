using System;
using SpeedRunCommon;

namespace SpeedRunApp.Model.Data
{
    public class SpeedRunStatus
    {
        //public SpeedRunStatusDTO()
        //{
        //}

        //public SpeedRunStatusDTO(RunStatus runStatus)
        //{
        //    Type = runStatus.Type;
        //    ExaminerUserID = runStatus.ExaminerUserID;
        //    Reason = runStatus.Reason;
        //    VerifyDate = runStatus.VerifyDate;
        //}

        public RunStatusType Type { get; set; }
        public string ExaminerUserID { get; set; }
        public string Reason { get; set; }
        public DateTime? VerifyDate { get; set; }
    }
}

