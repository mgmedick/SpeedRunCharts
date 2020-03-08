using System;
using System.Collections.Generic;
using System.Globalization;
using SpeedRunCommon;

namespace SpeedrunComSharp.Model
{
    public class RunStatus
    {
        public RunStatusType Type { get; set; }
        public string ExaminerUserID { get; set; }
        public string Reason { get; set; }
        public DateTime? VerifyDate { get; set; }

        #region Links
        public Lazy<User> examiner { get; set; }
        public User Examiner { get { return examiner.Value; } }
        #endregion

        public RunStatus() { }

        /*
        private static RunStatusType ParseType(string type)
        {
            switch (type)
            {
                case "new":
                    return RunStatusType.New;
                case "verified":
                    return RunStatusType.Verified;
                case "rejected":
                    return RunStatusType.Rejected;
            }

            throw new ArgumentException("type");
        }

        public static RunStatus Parse(SpeedrunComClient client, dynamic statusElement)
        {
            var status = new RunStatus();

            var properties = statusElement.Properties as IDictionary<string, dynamic>;

            status.Type = ParseType(statusElement.status as string);

            if (status.Type == RunStatusType.Rejected 
                || status.Type == RunStatusType.Verified)
            {
                status.ExaminerUserID = statusElement.examiner as string;
                status.examiner = new Lazy<User>(() => client.Users.GetUser(status.ExaminerUserID));

                if (status.Type == RunStatusType.Verified)
                {
                    var date = properties["verify-date"] as string;
                    if (!string.IsNullOrEmpty(date))
                        status.VerifyDate = DateTime.Parse(date, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
                }
            }
            else
            {
                status.examiner = new Lazy<User>(() => null);
            }

            if (status.Type == RunStatusType.Rejected)
            {
                status.Reason = statusElement.reason as string;
            }

            return status;
        }
        */
        public override string ToString()
        {
            if (Type == RunStatusType.Rejected)
                return "Rejected:" + Reason;
            else
                return Type.ToString();
        }
    }
}
