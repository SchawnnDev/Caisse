using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaisseLibrary.Exceptions;
using CaisseServer.Events;
using CaisseServer.Items;

namespace CaisseLibrary.IO
{
    public class BitmapManager
    {
        private readonly SaveableEvent SaveableEvent;
        private readonly string BaseDirectory;
        //private readonly string[] ForbiddenStrings = new string[] {"<", ">", ":", "\"", "/", "\"", "|", "?", "*"};

        public BitmapManager(SaveableEvent e)
        {
            SaveableEvent = e;
            BaseDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Bitmaps", e.Id.ToString());
        }

        public void Init(List<SaveableArticle> articles)
        {
            try
            {
                if (!Directory.Exists(BaseDirectory))
                    Directory.CreateDirectory(BaseDirectory);
            }
            catch (Exception e)
            {
                // ignored
                throw new TicketPrinterException(e.Message);
            }

            ConvertEventLogo();

            foreach (var article in articles)
            {
                if (!File.Exists(article.ImageSrc)) continue;

                var path = Path.Combine(BaseDirectory, article.Id + ".bmp");

                if (File.Exists(path)) continue;

                var bitmap = new Bitmap(article.ImageSrc);

                try
                {
                    bitmap.Save(path);
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        public void ConvertEventLogo()
        {
            var fileName = $"{SaveableEvent.Id}.bmp";
            var path = Path.Combine(BaseDirectory, fileName);

            if (!File.Exists(SaveableEvent.ImageSrc) || File.Exists(path)) return;

            var bitmap = new Bitmap(SaveableEvent.ImageSrc);

            try
            {
                bitmap.Save(path);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public string GetBitmapPath(SaveableArticle article) =>
            Path.Combine(BaseDirectory, article.Id + ".bmp");

        public string GetLogoPath =>
            Path.Combine(BaseDirectory, $"{SaveableEvent.Id}.bmp");
    }
}