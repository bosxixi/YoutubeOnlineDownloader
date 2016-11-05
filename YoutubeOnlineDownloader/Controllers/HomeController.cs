using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace YoutubeOnlineDownloader.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            string path = Path.Combine("c:/", "bosxixi.com", "youtube");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var ds = Directory.GetDirectories(path).Select(c => new DirectoryInfo(c));
            return View(ds);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string url)
        {
            Downloader downloader = new Downloader(url);

            Task task = new Task(downloader.Start);
            task.Start();

            return RedirectToAction(nameof(Index));
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


    }
}