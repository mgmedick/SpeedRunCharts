using System;
using SpeedrunComSharp.Model;
using SpeedRunCommon;

namespace SpeedRunApp.Model
{
    public class SpeedRunStatusDTO
    {
        public SpeedRunStatusDTO(RunStatus runStatus)
        {
            Type = runStatus.Type;
            Reason = runStatus.Reason;
            VerifyDate = runStatus.VerifyDate;

            if(runStatus.Examiner != null)
            {
                Examiner = new UserDTO(runStatus.Examiner);
            }
        }

        public RunStatusType Type { get; set; }
        //public string ExaminerUserID { get; set; }
        public string Reason { get; set; }
        public DateTime? VerifyDate { get; set; }
        public UserDTO Examiner { get; set; }
    }
}

