using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using DevExpress.Mvvm;

namespace PicEditor.ViewModel
{
    class ImageItem : ViewModelBase
    {
        private const int imgWidgh = 150;
        private const int imgHeigh = 150;

        public double Width { get; set; } = imgWidgh;
        public double Height { get; set; } = imgHeigh;
        private string path = "";
        public string FullPath
        {
            get => path;
            set
            {
                path = value;
                Name = Path.GetFileNameWithoutExtension(value);
                Extension = Path.GetExtension(value);
                CreationDate = File.GetCreationTime(value);
                ModificationDate = File.GetLastWriteTime(value);
            }
        }
        public string Name { get; private set; }
        public string Extension { get; private set; }
        public BitmapImage Icon { get; set; }
        private BitmapImage _fullImage = null;
        public BitmapImage FullImage => _fullImage == null ? GetFullImage() : _fullImage;

        

        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }

        public static Action<BitmapImage> SetVisibility;

        #region Конструкторы
        public ImageItem(BitmapImage source, string fullPath)
        {
            Icon = source;
            FullPath = fullPath;
        }

        public ImageItem(BitmapImage source)
        {
            Icon = source;
        }

        public ImageItem(Image image)
        {
            Icon = (BitmapImage)image.Source;
            Width = image.Width;
            Height = image.Height;
        }

        public ImageItem(BitmapImage source, string fullPath, double width = imgWidgh, double height = imgHeigh)
        {
            Width = width;
            Height = height;
            Icon = source;
            FullPath = fullPath;
        }
        #endregion

        private static ImageItem DraggableImage { get; set; }

        public Image GetImage()
        {
            Image image = new Image()
            {
                Source = Icon,
                Height = Height,
                Width = Width,
            };
            return image;
        }

        public void Fill(ImageItem it)
        {
            Icon = it.Icon;
            //FullImage = it.FullImage;
            FullPath = it.FullPath;
        }        

        public ICommand ImageClick
        {
            get => new DCommand((obj) =>
            {
                DraggableImage = this;
                System.Windows.DataObject dataObj = new System.Windows.DataObject(this);
                DragDrop.DoDragDrop(this.GetImage(), dataObj, System.Windows.DragDropEffects.Move);
            });
        }

        public ICommand ImageDrop
        {
            get => new DCommand((obj) =>
            {
                int i = MainWindowVM.ImageItems.IndexOf(this);
                MainWindowVM.ImageItems.Remove(DraggableImage);
                MainWindowVM.ImageItems.Insert(i, DraggableImage);
                DraggableImage = null;
            });
        }

        public ICommand ShowFullImage
        {
            get => new DCommand((obj) =>
            {
                SetVisibility(FullImage);
            });
        }

        private BitmapImage GetFullImage()
        {
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(FullPath);
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.EndInit();
            return image;
        }
    }
}
