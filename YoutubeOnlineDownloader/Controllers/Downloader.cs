using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using YoutubeExtractor;
using YoutubeOnlineDownloader.Models;
using static bosxixi.Extensions.Extension;

namespace YoutubeOnlineDownloader.Controllers
{
    public class Downloader
    {
        public Downloader(string link)
        {
            this.link = link;
        }

        public void Start()
        {
            var pl = GetPlaylist(link);
            string path = Path.Combine("c:/", "bosxixi.com", "youtube", pl.Title);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            int counter = 1;
            foreach (var video in pl.Urls)
            {
                DownloadVideo(video, path, counter++);
            }
        }

        private readonly string link;
        private void DownloadVideo(string link, string path, int index)
        {
            IEnumerable<VideoInfo> videoInfos = DownloadUrlResolver.GetDownloadUrls(link);
            int maxResolution = videoInfos.Max(c => c.Resolution);

            VideoInfo video = videoInfos
                .First(info => info.VideoType == VideoType.Mp4 && info.Resolution == maxResolution);

            if (video.RequiresDecryption)
            {
                DownloadUrlResolver.DecryptDownloadUrl(video);
            }
            var videoFileName = $"{index}_" + GetValidFileName(video.Title + video.VideoExtension);
            var videoDownloader = new VideoDownloader(video, Path.Combine(path, videoFileName));

            videoDownloader.Execute();
        }
        private Playlist GetPlaylist(string playlistlink)
        {
            HttpClient client = new HttpClient();
            var html = client.GetStringAsync(playlistlink).GetAwaiter().GetResult();

            byte[] bytes = Encoding.Default.GetBytes(html);
            html = Encoding.UTF8.GetString(bytes);

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            var nodes = doc.QuerySelectorAll("tr.pl-video a.pl-video-title-link").Select(c => "https://www.youtube.com" + c.Attributes["href"].Value);
            var title = doc.QuerySelector("h1.pl-header-title").InnerText.Trim();

            return new Playlist() { Title = title, Urls = nodes };
        }
    }
}