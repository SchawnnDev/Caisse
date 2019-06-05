using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaisseLibrary.Exceptions;
using CaisseLibrary.Utils;
using CaisseServer.Events;
using CaisseServer.Items;

namespace CaisseLibrary.IO
{
    public class BitmapManager
    {
        private readonly SaveableEvent SaveableEvent;
        private readonly string BaseDirectory;
        public static readonly string[] ForbiddenStrings = {"<", ">", ":", "\"", "/", "\"", "|", "?", "*"};

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

                var bitmap = new Bitmap(article.ImageSrc);

                if (File.Exists(path))
                {

                    var existing = new Bitmap(path);

                    if (bitmap.CompareBitmapsFast(existing))
                        continue;
                    
                    // dispose before delete
                    existing.Dispose();

                    File.Delete(path);

                }
                
                try
                {
                    bitmap.Scale(900, 520); // test
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
            var fileName = $"E{SaveableEvent.Id}.bmp";
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
            Path.Combine(BaseDirectory, $"E{SaveableEvent.Id}.bmp");

        public static string NormalizeFileName(string fileName) => ForbiddenStrings.Aggregate(fileName, (current, forbiddenString) => current.Replace(forbiddenString, ""));
    }
}