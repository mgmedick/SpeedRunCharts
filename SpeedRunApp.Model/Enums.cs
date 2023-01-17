using System.Runtime.Serialization;

namespace SpeedRunApp.Model
{
    public enum CategoryType
    {
        [EnumMember(Value = "FullGame")]
        FullGame,
        [EnumMember(Value = "Level")]
        Level
    }

    public enum RunStatusType
    {
        New, Verified, Rejected
    }

    //Variables
    public enum VariableScopeType
    {
        Global, FullGame, AllLevels, SingleLevel
    }

    public enum Template
    {
        ActivateEmail = 1,
        ResetPasswordEmail = 2,
        ConfirmRegistration = 3
    }  

    public enum ExportType
    {
        csv = 0,
        json = 1
    }      
}
