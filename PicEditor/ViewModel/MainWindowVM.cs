using DevExpress.Mvvm;
using DevExpress.Mvvm.Native;
using DevExpress.Mvvm.UI;
using Microsoft.Win32;
using PicEditor.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using NaturalSort.Extension;
using Image = System.Windows.Controls.Image;
using DevExpress.Mvvm.UI.Interactivity.Internal;

namespace PicEditor.ViewModel
{
    class MainWindowVM : ViewModelBase
    {
        private const int defaultSize = 150;
        //private const int defaultWidgh = 150;
        //private const int defaultHeigh = 150;

        public static ObservableCollection<ImageItem> ImageItems { get; set; } = new ObservableCollection<ImageItem>();
        public static ObservableCollection<ImageItem> SelectedImageItems { get; set; } = new ObservableCollection<ImageItem>();

        public string NewName { get; set; }
        public int SortParamIndex { get; set; } = -1;
        public Visibility PictureVisibility { get; set; } = Visibility.Hidden;
        public BitmapImage PictureSource { get; set; }
        public double ImageWidth { get; set; } = defaultSize;
        public double ImageHeight { get; set; } = defaultSize;
        public double ScrollViewHeight { get; set; }
        public Point ScrollViewMousePos{ get; set; }
        public BitmapImage DraggablePreview { get; set; }
        public Thickness DraggableMargin { get; set; } = new Thickness(10, 415, 0, 0);
        public Visibility DraggableVisibility { get; set; } = Visibility.Hidden;

        #region Delegates
        public delegate Point PointHandler();
        public delegate double SizeHandler();

        public Action<double> LineUp;
        public Action<double> LineDown;
        public PointHandler GetMausePosOnScrollView;
        public PointHandler GetMausePosOnWindow;
        public SizeHandler GetScrollViewHeigh;
        #endregion
        //public static ImageItem DraggableImage { get; set; } = null;

        public string Str { get; set; } = "TEEEEEEEEEEEEEEEEEST";

        private MainModel model = new MainModel();
        private Global global = Global.getInstance();

        public BitmapImage Prev
        {
            get => new BitmapImage();
        }

        public MainWindowVM()
        {
            model.ShowPicture += (img, i) =>
            {
                for (int j = 0; j < ImageItems.Count; j++)
                {
                    ImageItems[i].Fill(img);
                }
            };

            model.GiveCount += (count) =>
            {
                for (int i = 0; i < count; i++)
                {
                    BitmapImage bmi = new BitmapImage();
                    ImageItems.Add(new ImageItem(bmi));
                }
            };

            global.ShowPreview = (prev) =>
            {
                DraggablePreview = prev;
                DraggableVisibility = Visibility.Visible;
            };

            global.HidePreview = () => DraggableVisibility = Visibility.Hidden; 

            ImageItem.SetVisibility = (pic) =>
            {
                PictureVisibility = Visibility.Visible;
                PictureSource = pic;
            };
        }

        #region Commands
        public ICommand Ok
        {
            get => new DelegateCommand(() =>
            {
                //LineDown();
                //PictureVisibility = Visibility.Visible;
                //Clicks++;
                //for (int i = 0; i < 1000; i++)
                //{
                //    BitmapImage bmi = new BitmapImage(new Uri(@"C:\Users\kserg\OneDrive\Рабочий стол\Images\Flopp.png"));
                //    ImageItems.Add(new ImageItem(bmi));
                //}
            });
        }

        public ICommand Test
        {
            get => new DelegateCommand(() =>
            {
                PictureVisibility = Visibility.Hidden;
            });
        }

        public ICommand WindowLoaded
        {
            get => new DelegateCommand(() =>
            {
                ImageItems.Clear();
                //model.GetPictures(@"C:\Users\kserg\OneDrive\Рабочий стол\imgs\TestThink", SortingType.Name);//, () => SortBy(Sorting.Name));
            });
        }

        public ICommand OpenFolder
        {
            get => new DelegateCommand(() =>
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                if(fbd.ShowDialog() == DialogResult.OK)
                {
                    ImageItems.Clear();
                    model.GetPictures(fbd.SelectedPath, SortingType.Name);
                }
            });
        }

        public ICommand RenameAll
        {
            get => new DelegateCommand(() =>
            {
                model.RenameAll(ImageItems.ToList(), NewName);
            });
        }

        public ICommand SortParamChanged
        {
            get => new DelegateCommand(() =>
            {
                switch (SortParamIndex)
                {
                    case 0:
                        SortBy(SortingType.Name);
                        break;
                    case 1:
                        SortBy(SortingType.CreationDate);
                        break;
                    case 2:
                        SortBy(SortingType.ModificationDate);
                        break;
                }
            });
        }

        public ICommand WindowKeyDown
        {
            get => new DCommand((obj) =>
            {
                //System.Windows.Input.KeyEventArgs key = (System.Windows.Input.KeyEventArgs)obj;
                //if(key.Key == Key.Escape)
                //{
                //    for (int i = 0; i < ImageItems.Count; i++)
                //    {
                //        ImageItem it = ImageItems[i];
                //        if (it.IsSelected == true)
                //        {
                //            it.IsSelected = false;
                //            ImageItems[i] = it;
                //            ImageItems[i].Doot();
                //        }
                //    }
                //}
            });
        }

        public ICommand WindowLeftButtonUp
        {
            get => new DCommand((obj) =>
            {
                global.DraggableImage = null;
            });
        }

        public ICommand Scroll
        {
            get => new DCommand((obj) =>
            {
                if (Mouse.LeftButton == MouseButtonState.Pressed && global.DraggableImage != null)
                {
                    const double trigger = 50;
                    const double offset = 5;
                    const int sleep = 1;
                    Point pos = ScrollViewMousePos;

                    Thread thread = new Thread(() =>
                    {
                        while (ScrollViewMousePos.Y <= trigger)
                        {
                            LineUp(offset);
                            Thread.Sleep(sleep);
                        }
                        while (ScrollViewMousePos.Y >= GetScrollViewHeigh() - trigger)
                        {
                            LineDown(offset);
                            Thread.Sleep(sleep);
                        }
                    });

                    thread.Start();

                    //if (pos.Y <= trigger)
                    //{
                    //    LineUp();
                    //}
                    //if(pos.Y >= GetScrollViewHeigh() - trigger)
                    //{
                    //    LineDown();
                    //}
                }
            });
        }
        
        public ICommand DragOver
        {
            get => new DCommand((obj) =>
            {
                //double tolerance = 30;
                //double posY = GetMausePosOnScrollView().Y;

                //if (posY < tolerance && posY >= 0)
                //{
                //    LineUp();
                //}
            });
        }

        public ICommand MD
        {
            get => new DCommand((obj) =>
            {
                double posY = GetMausePosOnScrollView().Y;
            });
        }

        public ICommand WinMouseMove
        {
            get => new DCommand((obj) =>
            {
                var e = (System.Windows.Input.MouseEventArgs)obj;

                if(Mouse.LeftButton == MouseButtonState.Pressed && global.DraggableImage != null)
                {
                    Point pos = GetMausePosOnWindow();
                    DraggableMargin = new Thickness(pos.X - global.ImageMouseX, pos.Y - global.ImageMouseY, 0, 0);
                }
            });
        }
        #endregion

        private void SortBy(SortingType sorting)
        {
            IOrderedEnumerable<ImageItem> temp = null;
            switch (sorting)
            {
                case SortingType.Name:
                    temp = ImageItems.OrderBy(p => p.Name, StringComparison.OrdinalIgnoreCase.WithNaturalSort());
                    break;
                case SortingType.CreationDate:
                    temp = ImageItems.OrderBy(p => p.CreationDate);
                    break;
                case SortingType.ModificationDate:
                    temp = ImageItems.OrderBy(p => p.ModificationDate);
                    break;
            }
            for (int i = 0; i < temp.Count(); i++)
            {
                ImageItems.Move(ImageItems.IndexOf(temp.ElementAt(i)), i);
            }
        }
    }
}
