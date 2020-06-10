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
            ExaminerUserID = runStatus.ExaminerUserID;
            Reason = runStatus.Reason;
            VerifyDate = runStatus.VerifyDate;

            //links
            _examiner = runStatus.examiner;
        }

        public RunStatusType Type { get; set; }
        public string ExaminerUserID { get; set; }
        public string Reason { get; set; }
        public DateTime? VerifyDate { get; set; }

        //links
        private Lazy<User> _examiner { get; set; }
        public UserDTO Examiner { get { return (_examiner != null && _examiner.Value != null) ? new UserDTO(_examiner.Value) : null; } }
    }
}

