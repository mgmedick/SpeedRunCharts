using SpeedRunApp.Model.Data;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SpeedRunApp.Model.ViewModels
{
    public class UserViewModel
    {
        public UserViewModel(UserView user)
        {
            ID = user.ID;
            Name = user.Name;
            SignUpDate = user.SignUpDate;
            Location = user.Location;
            SpeedRunComLink = user.SpeedRunComUrl;
            TwitchProfile = user.TwitchProfileUrl;
            HitboxProfile = user.HitboxProfileUrl;
            YoutubeProfile = user.YoutubeProfileUrl;
            TwitterProfile = user.TwitterProfileUrl;
            SpeedRunsLiveProfile = user.SpeedRunsLiveProfileUrl;
            ProfileImage = Task.Run<string>(async () => await ParseProfileImageLink(user.ProfileImageUrl)).Result;
            TotalSpeedRuns = user.TotalSpeedRuns;
            TotalWorldRecords = user.TotalWorldRecords;
            TotalPersonalBests = user.TotalPersonalBests;
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime? SignUpDate { get; set; }
        public string Location { get; set; }
        public string SpeedRunComLink { get; set; }
        public string ProfileImage { get; set; }
        public string TwitchProfile { get; set; }
        public string HitboxProfile { get; set; }
        public string YoutubeProfile { get; set; }
        public string TwitterProfile { get; set; }
        public string SpeedRunsLiveProfile { get; set; }
        public int TotalSpeedRuns { get; set; }
        public int TotalWorldRecords { get; set; }
        public int TotalPersonalBests { get; set; }
        private async Task<string> ParseProfileImageLink(string profileImageUrl)
        {
            string profileImageLink = profileImageUrl;

            using (HttpClient client = new HttpClient())
            {
                using (var response = await client.GetAsync(profileImageUrl, HttpCompletionOption.ResponseHeadersRead))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        profileImageLink = null;
                    }
                }
            }

            return profileImageLink;
        }
    }
}


