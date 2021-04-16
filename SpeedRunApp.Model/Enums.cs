using System.Runtime.Serialization;

namespace SpeedRunApp.Model
{
    //Base
    public enum ElementType
    {
        Category,
        Game,
        Guest,
        Level,
        Notification,
        Platform,
        Region,
        Run,
        Series,
        User,
        Variable

    }

    //Categories
    public enum CategoriesOrdering : int
    {
        Position = 0,
        PositionDescending,
        Name,
        NameDescending,
        Miscellaneous,
        MiscellaneousDescending
    }

    public enum CategoryType
    {
        [EnumMember(Value = "PerGame")]
        PerGame,
        [EnumMember(Value = "PerLevel")]
        PerLevel
    }

    //Common
    public enum ModeratorType
    {
        Moderator,
        SuperModerator
    }

    public enum PlayersType
    {
        Exactly, UpTo
    }

    public enum TimingMethod
    {
        GameTime, RealTime, RealTimeWithoutLoads
    }

    //Games
    public enum GamesOrdering : int
    {
        Similarity = 0,
        SimilarityDescending,
        Name,
        NameDescending,
        JapaneseName,
        JapaneseNameDescending,
        Abbreviation,
        AbbreviationDescending,
        YearOfRelease,
        YearOfReleaseDescending,
        CreationDate,
        CreationDateDescending
    }

    //Leaderboards
    public enum EmulatorsFilter
    {
        NotSet,
        OnlyEmulators,
        NoEmulators
    }

    public enum LeaderboardScope
    {
        All, FullGame, Levels
    }

    //Levels
    public enum LevelsOrdering : int
    {
        Position = 0,
        PositionDescending,
        Name,
        NameDescending
    }

    //Notifications
    public enum NotificationsOrdering : int
    {
        NewestToOldest = 0,
        OldestToNewest
    }

    public enum NotificationStatus
    {
        Unread, Read
    }

    public enum NotificationType
    {
        Post, Run, Game, Guide
    }

    //Platforms
    public enum PlatformsOrdering : int
    {
        Name = 0,
        NameDescending,
        YearOfRelease,
        YearOfReleaseDescending
    }

    //Regions
    public enum RegionsOrdering : int
    {
        Name = 0,
        NameDescending,
    }

    //Runs
    public enum RunsOrdering : int
    {
        Game = 0,
        GameDescending,
        Category,
        CategoryDescending,
        Level,
        LevelDescending,
        Platform,
        PlatformDescending,
        Region,
        RegionDescending,
        Emulated,
        EmulatedDescending,
        Date,
        DateDescending,
        DateSubmitted,
        DateSubmittedDescending,
        Status,
        StatusDescending,
        VerifyDate,
        VerifyDateDescending
    }

    public enum RunStatusType
    {
        New, Verified, Rejected
    }

    //Series
    public enum SeriesOrdering : int
    {
        Name = 0,
        NameDescending,
        JapaneseName,
        JapaneseNameDescending,
        Abbreviation,
        AbbreviationDescending,
        CreationDate,
        CreationDateDescending
    }

    //Users
    public enum UserRole
    {
        Banned, User, Trusted, Moderator, Admin, Programmer, ContentModerator
    }

    public enum UsersOrdering : int
    {
        Name = 0,
        NameDescending,
        JapaneseName,
        JapaneseNameDescending,
        SignUpDate,
        SignUpDateDescending,
        Role,
        RoleDescending
    }

    //Variables
    public enum VariableScopeType
    {
        Global, FullGame, AllLevels, SingleLevel
    }

    public enum VariablesOrdering : int
    {
        Position = 0,
        PositionDescending,
        Name,
        NameDescending,
        Mandatory,
        MandatoryDescending,
        UserDefined,
        UserDefinedDescending
    }

    //Web
    public enum SpeedRunListCategory
    {
        New = 0,
        Top5Perc = 1,
        First = 2,
        Top3 = 3,
        PersonalBest = 4
    }

    public enum SearchCategory
    {
        Game = 0,
        User = 1
    }
}
