using SpeedRunApp.Model.Data;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SpeedRunApp.Model.JSON
{
    public class VideoResult
    {
        public string VideoLinkUrl { get; set; }
        public string EmbeddedVideoLinkUrl { get; set; }
        public string ThumbnailLinkUrl { get; set; }
        public long? ViewCount { get; set; }
    }
}


