using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.Drawing.Imaging;
using PicEditor.ViewModel;

namespace PicEditor.Model
{
    class MainModel
    {
        public Action<ImageItem, int> ShowPicture;
        public Action<int> GiveCount = (i) => { };

        //public string WorkingPath { get; set; }

        #region public methods
        //public async void GetPictures(string directory)
        //{
        //    await Task.Run(() => 
        //    {
        //        string[] pics = Directory.GetFiles(directory);

        //        Application.Current.Dispatcher.Invoke(() => GiveCount(pics.Length));
        //        for (int i = 0; i < pics.Length; i++)
        //        {
        //            string path = pics[i];
        //            BitmapImage image = new BitmapImage();
        //            image.BeginInit();
        //            image.UriSource = new Uri(path);
        //            image.CacheOption = BitmapCacheOption.OnLoad;
        //            image.EndInit();
        //            image.Freeze();

        //            Bitmap icon = GetIcon(path);

        //            Application.Current.Dispatcher.Invoke(() =>
        //            {
        //                ShowPicture(new ImageItem(BitmapToImage(icon), image, path), i);
        //            });
        //            if (i % 10 == 0)
        //                Thread.Sleep(1);

        //        }
        //    });
        //}

        public void GetPictures(string directory, Action end = null )
        {
            Thread thread = new Thread(() =>
            {
                string[] pics = Directory.GetFiles(directory);

                Application.Current.Dispatcher.Invoke(() => GiveCount(pics.Length));
                for (int i = 0; i < pics.Length; i++)
                {
                    string path = pics[i];
                    BitmapImage image = new BitmapImage();
                    image.BeginInit();
                    image.UriSource = new Uri(path);
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.EndInit();

                    BitmapImage icon = GetIcon(path, 150);
                    //icon.Freeze();

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        ShowPicture(new ImageItem(icon, path), i);
                    });
                    //if (i % 10 == 0)
                        //Thread.Sleep(1);
                }
                Application.Current.Dispatcher.Invoke(() => end?.Invoke());
            });
            thread.Start();
        }

        public void RenameAll(List<ImageItem> images, string renaming)
        {
            for (int i = 0; i < images.Count; i++)
            {
                string newName = renaming + $"-{i+1}";
                string dir = Path.GetDirectoryName(images[i].FullPath);
                string newPath = Path.Combine(dir, newName + images[i].Extension);
                if(!File.Exists(newPath))
                {
                    File.Move(images[i].FullPath, newPath);
                    images[i].FullPath = newPath;
                }
            }
        }
        #endregion

        #region private methods
        private BitmapImage GetIcon(string path, int size)
        {
            Bitmap icon = new Bitmap(path);

            int x = 0;
            int y = 0;
            int width = icon.Width;
            int height = icon.Height;

            if (width > height)
            {
                x = (width - height) / 2;
                width = height;
            }
            else if (height > width)
            {
                y = (height - width) / 2;
                height = width;
            }
            icon = icon.Clone(new Rectangle(x, y, width, height), PixelFormat.Format16bppRgb555);
            Bitmap bm = new Bitmap(icon, new System.Drawing.Size(size, size));
            return BitmapToImage(bm);
        }

        private BitmapImage BitmapToImage(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();
                bitmapimage.Freeze();

                return bitmapimage;
            }
        }
        #endregion
    }
}