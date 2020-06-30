using DevExpress.Mvvm;
using DevExpress.Mvvm.Native;
using DevExpress.Mvvm.UI;
//using GongSolutions.Wpf.DragDrop;
using Microsoft.Win32;
using PicEditor.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
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

namespace PicEditor.ViewModel
{
    class MainWindowVM : ViewModelBase
    {
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
                    BitmapImage bmi = new BitmapImage(new Uri(@"C:\Users\kserg\OneDrive\Рабочий стол\Images\Flopp.png"));
                    ImageItems.Add(new ImageItem(bmi));
                }
            };

            ImageItem.SetVisibility = (pic) =>
            {
                PictureVisibility = Visibility.Visible;
                PictureSource = pic;
            };
        }

        enum Sorting
        {
            Name,
            CreationDate,
            ModificationDate
        }

        private MainModel model = new MainModel();

        public static ObservableCollection<ImageItem> ImageItems { get; set; } = new ObservableCollection<ImageItem>();
        public string NewName { get; set; }
        public int SortParamIndex { get; set; } = -1;
        public Visibility PictureVisibility { get; set; } = Visibility.Hidden;
        public BitmapImage PictureSource { get; set; }

        public ICommand Ok
        {
            get => new DelegateCommand(() =>
            {
                PictureVisibility = Visibility.Visible;
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
                model.GetPictures(@"C:\Users\kserg\OneDrive\Рабочий стол\imgs\TestThink");//, () => SortBy(Sorting.Name));
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
                    model.GetPictures(fbd.SelectedPath);
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
                        SortBy(Sorting.Name);
                        break;
                    case 1:
                        SortBy(Sorting.CreationDate);
                        break;
                    case 2:
                        SortBy(Sorting.ModificationDate);
                        break;
                }
            });
        }

        public ICommand WinMouseMove
        {
            get => new DCommand((obj) =>
            { 
                var e = (System.Windows.Input.MouseEventArgs)obj;
                //double d1 = e.GetPosition().X;
                //RectMargin);
            });
        }

        private void SortBy(Sorting sorting)
        {
            IOrderedEnumerable<ImageItem> temp = null;
            switch (sorting)
            {
                case Sorting.Name:
                    temp = ImageItems.OrderBy(p => p.Name, StringComparison.OrdinalIgnoreCase.WithNaturalSort());
                    break;
                case Sorting.CreationDate:
                    temp = ImageItems.OrderBy(p => p.CreationDate);
                    break;
                case Sorting.ModificationDate:
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
