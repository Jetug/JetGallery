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
using NaturalSort.Extension;
using DevExpress.Mvvm.Native;

namespace PicEditor.Model
{
    enum SortingType
    {
        None,
        Name,
        CreationDate,
        ModificationDate
    }

    class MainModel
    {
        public Action<ImageItem, int> ShowPicture;
        public Action<int> GiveCount = (i) => { };

        #region public methods
        public void GetPictures(string directory, SortingType sorting = SortingType.None, Action end = null)
        {
            Thread thread = new Thread(() =>
            {
                List<string> temp = Directory.GetFiles(directory).ToList();
                temp.RemoveAll( (s) => !IsImage(s));
                IOrderedEnumerable<string> pics = null;

                switch (sorting)
                {
                    case SortingType.Name:
                        pics = temp.OrderBy(p => p, StringComparison.OrdinalIgnoreCase.WithNaturalSort());
                        break;
                    case SortingType.CreationDate:
                        pics = temp.OrderBy(p => File.GetCreationTime(p));
                        break;
                    case SortingType.ModificationDate:
                        pics = temp.OrderBy(p => File.GetLastWriteTime(p));
                        break;
                }

                Application.Current.Dispatcher.Invoke(() => GiveCount(pics.Count()));
                for (int i = 0; i < pics.Count(); i++)
                {
                    string path = pics.ElementAt(i);
                    BitmapImage icon = GetPreview(path, 150);
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        ShowPicture(new ImageItem(icon, path), i);
                    });
                }
                Application.Current.Dispatcher.Invoke(() => end?.Invoke());
            });
            thread.Start();
        }

        //public void GetPictures(string directory, SortingType sorting, Action end = null)
        //{
        //    Thread thread = new Thread(() =>
        //    {
        //        string[] pics = Directory.GetFiles(directory);

        //        List<ImageItem> sortingList = new List<ImageItem>();

        //        Application.Current.Dispatcher.Invoke(() => GiveCount(pics.Length));
        //        for (int i = 0; i < pics.Length; i++)
        //        {
        //            string path = pics[i];
        //            BitmapImage icon = GetPreview(path, 150);
        //            sortingList.Add(new ImageItem(icon, path));                    
        //        }

        //        IOrderedEnumerable<ImageItem> temp = sortingList.OrderBy(p => p.Name, StringComparison.OrdinalIgnoreCase.WithNaturalSort());

        //        for (int i = 0; i < temp.Count(); i++)
        //        {
        //            Application.Current.Dispatcher.Invoke(() =>
        //            {
        //                ShowPicture(temp.ElementAt(i), i);
        //            });
        //        }

        //        Application.Current.Dispatcher.Invoke(() => end?.Invoke());
        //    });
        //    thread.Start();
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
                    BitmapImage icon = GetPreview(path, 150);
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

        public async void GetPreviewAsync(string path, int size, Action<BitmapImage> ShowIcon)
        {
            await Task.Run(() => ShowIcon(GetPreview(path, size)));
        }

        public BitmapImage GetPreview(string path, int size)
        {
            Bitmap image = new Bitmap(path);

            int x = 0;
            int y = 0;
            int width = image.Width;
            int height = image.Height;

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
            image = image.Clone(new Rectangle(x, y, width, height), PixelFormat.Format16bppRgb555);

            //BitmapImage bmi = new BitmapImage(new Uri(path));
            //BitmapSource bms = new CroppedBitmap(bmi, new Int32Rect(x, y, width, height));

            //return Stoi(bms, size);

            Bitmap preview = new Bitmap(image, new System.Drawing.Size(size, size));
            return BitmapToImage(preview);
        }

        private BitmapImage Stoi(BitmapSource bitmapSource, int size)
        {
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            MemoryStream memoryStream = new MemoryStream();
            using (MemoryStream memory = new MemoryStream())
            {
                BitmapImage bmi = new BitmapImage();

                encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                encoder.Save(memoryStream);

                memoryStream.Position = 0;
                bmi.BeginInit();
                bmi.StreamSource = memoryStream;
                bmi.DecodePixelWidth = size;
                bmi.DecodePixelHeight = size;
                bmi.EndInit();

                memoryStream.Close();

                return bmi;
            }
        }

        private void CutImage(BitmapImage bmi)
        {
            BitmapSource topHalf = new CroppedBitmap(bmi, new Int32Rect());
        }

        public BitmapImage GetFullImage(string path)
        {
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(path);
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.EndInit();
            return image;
        }

        #region private methods

        private readonly string[] imageExtensions = new string[] { ".jpg",".jpeg",".bmp",".png",".gif",};

        private bool IsImage(string name)
        {
            string ext = Path.GetExtension(name);
            bool b = imageExtensions.Contains(ext);
            return b;
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