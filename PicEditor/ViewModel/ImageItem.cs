using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using DevExpress.Mvvm;
using PicEditor.Model;
using System.Security.Policy;
using System.Collections.Generic;

namespace PicEditor.ViewModel
{
    class ImageItem : ViewModelBase
    {
        private const int imgWidgh = 150;
        private const int imgHeigh = 150;

        private static string pth = @"C:\Users\kserg\OneDrive\Рабочий стол\Images\Flopp.png";

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
        //private BitmapImage _preview = bmi;
        //public BitmapImage Preview 
        //{
        //    get
        //    {
        //        if(_preview == bmi)
        //        {
        //            model.GetPreviewAsync(FullPath, 150, (image) => Preview = image);
        //        }
        //        return _preview;
        //    }
        //    set
        //    {
        //        _preview = value;
        //        RaisePropertiesChanged("Preview");
        //    } 
        //}

        public BitmapImage Preview { get; set; }
        private BitmapImage _fullImage = null;
        public BitmapImage FullImage 
        {
            get
            {
                if(_fullImage == null)
                    _fullImage = model.GetFullImage(FullPath);
                return _fullImage;
            }
        }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }

        public Point ImageMousePos { get; set; }

        public bool _isSelected;
        public bool IsSelected 
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                RaisePropertiesChanged("IsSelected");
            }
        }

        public void Doot()
        {
            RaisePropertiesChanged("IsSelected");
        }

        public static Action<BitmapImage> SetVisibility;

        private MainModel model = new MainModel();
        private Global global = Global.getInstance();

        #region Конструкторы

        public ImageItem(string fullPath)
        {
            FullPath = fullPath;
        }

        public ImageItem(BitmapImage source)
        {
            Preview = source;
        }

        public ImageItem(BitmapImage source, string fullPath, double width = imgWidgh, double height = imgHeigh)
        {            
            Preview = source;
            FullPath = fullPath;
            Width = width;
            Height = height;
        }
        #endregion

        public Image GetImage()
        {
            Image image = new Image()
            {
                Source = Preview,
                Height = Height,
                Width = Width,
            };
            return image;
        }

        public void Fill(ImageItem it)
        {
            Preview = it.Preview;
            //FullImage = it.FullImage;
            FullPath = it.FullPath;
        }

        #region Commands
        public ICommand ImageSelected
        {
            get => new DCommand((obj) =>
            {
                MainWindowVM.SelectedImageItems.Add(this);
            });
        }

        public ICommand ImageUnselected
        {
            get => new DCommand((obj) =>
            {
                MainWindowVM.SelectedImageItems.Remove(this);
            });
        }

        public ICommand ImageMouseMove
        {
            get => new DCommand((obj) =>
            {
                if (Mouse.LeftButton == MouseButtonState.Pressed && Keyboard.Modifiers != ModifierKeys.Control && global.DraggableImage == null)
                {
                    global.DraggableImage = this;
                    MainWindowVM.SelectedImageItems.Add(this);
                    //DataObject dataObj = new DataObject(this);
                    //DragDrop.DoDragDrop(this.GetImage(), dataObj, DragDropEffects.Move);
                }
            });
        }

        public ICommand ImageMouseEnter
        {
            get => new DCommand((obj) =>
            {
                if (Keyboard.Modifiers == ModifierKeys.Control && Mouse.LeftButton == MouseButtonState.Pressed)
                {
                    IsSelected = !IsSelected;
                }
            });
        }

        public ICommand ImageDrop
        {
            get => new DCommand((obj) =>
            {
                int i = MainWindowVM.ImageItems.IndexOf(this);
                //MainWindowVM.ImageItems.Remove(DraggableImage);
                //MainWindowVM.ImageItems.Insert(i, DraggableImage);
                global.DraggableImage = null;

                foreach (var item in MainWindowVM.SelectedImageItems)
                {
                    MainWindowVM.ImageItems.Remove(item);
                    MainWindowVM.ImageItems.Insert(i, item);
                    //i++;
                }

                MainWindowVM.SelectedImageItems.Clear();

                //foreach (var item in MainWindowVM.SelectedImageItems)
                //{
                //    MainWindowVM.
                //}

                List<int> vs = new List<int>();
            });
        }

        public ICommand ImageLeftButtonUp
        {
            get => new DCommand((obj) =>
            {
                if(global.DraggableImage == null)
                    SetVisibility(FullImage);
                else
                {
                    int i = MainWindowVM.ImageItems.IndexOf(this);
                    global.DraggableImage = null;

                    foreach (var item in MainWindowVM.SelectedImageItems)
                    {
                        MainWindowVM.ImageItems.Remove(item);
                        MainWindowVM.ImageItems.Insert(i, item);
                    }
                    MainWindowVM.SelectedImageItems.Clear();
                }
            });
        }

        public ICommand ImageLeftButtonDown
        {
            get => new DCommand((obj) =>
            {
                global.ImageMouseX = ImageMousePos.X;
                global.ImageMouseY = ImageMousePos.Y;
            });
        }

        public ICommand ImageKeyDown
        {
            get => new DCommand((obj) =>
            {

            });
        }
        #endregion
    }
}
