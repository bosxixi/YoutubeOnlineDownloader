using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoutubeOnlineDownloader.Models
{
    public class Playlist
    {
        public string Title { get; set; }
        public IEnumerable<string> Urls { get; set; }
    }
}