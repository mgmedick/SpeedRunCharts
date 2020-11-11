using System;
using SpeedRunApp.Model.Entity;

namespace SpeedRunApp.Model.Data
{
    public class User
    {
        //public UserDTO(User user)
        //{
        //    ID = user.ID;
        //    Name = user.Name;
        //    JapaneseName = user.JapaneseName;
        //    Role = user.Role;
        //    SignUpDate = user.SignUpDate;
        //    Location = user.Location.ToString();
        //    TwitchProfile = user.TwitchProfile;
        //    HitboxProfile = user.HitboxProfile;
        //    YoutubeProfile = user.YoutubeProfile;
        //    TwitterProfile = user.TwitterProfile;
        //    SpeedRunsLiveProfile = user.SpeedRunsLiveProfile;

        //    //links
        //    _runs = user.runs;
        //    _profileImageUri = user.profileImageUri;
        //}

        public string ID { get; set; }
        public string Name { get; set; }
        public string JapaneseName { get; set; }
        public Uri WebLink { get; set; }
        public UserNameStyle NameStyle { get; set; }
        public UserRole Role { get; set; }
        public DateTime? SignUpDate { get; set; }
        public Location Location { get; set; }
        public Uri ProfileImage { get; set; }
        public Uri TwitchProfile { get; set; }
        public Uri HitboxProfile { get; set; }
        public Uri YoutubeProfile { get; set; }
        public Uri TwitterProfile { get; set; }
        public Uri SpeedRunsLiveProfile { get; set; }


        //links
        //public IEnumerable<Run> Runs { get; set; }
        //public IEnumerable<Game> ModeratedGames { get; set; }
        //public IEnumerable<Record> PersonalBests { get; set; }
        //public Lazy<Uri> profileImageUri { get; set; }
        //public Uri ProfileImageUri { get; set; }

        //private Lazy<Uri> _profileImageUri { get; set; }
        //public Uri ProfileImageUri { get { return _profileImageUri.Value; } }

        public UserEntity ConvertToEntity()
        {
            return new UserEntity
            {
                ID = this.ID,
                Name = this.Name,
                JapaneseName = this.JapaneseName,
                UserRoleID = (int)this.Role,
                Location = this.Location?.ToString(),
                SpeedRunComUrl = this.WebLink.ToString(),
                ProfileImageUrl = this.ProfileImage?.ToString(),
                TwitchProfileUrl = this.TwitchProfile?.ToString(),
                HitboxProfileUrl = this.HitboxProfile?.ToString(),
                YoutubeProfileUrl = this.YoutubeProfile?.ToString(),
                TwitterProfileUrl = this.TwitterProfile?.ToString(),
                SpeedRunsLiveProfileUrl = this.SpeedRunsLiveProfile?.ToString(),
                SignUpDate = this.SignUpDate
            };
        }
    }
}
