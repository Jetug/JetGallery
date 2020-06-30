using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JetControlLibrary
{
    /// <summary>
    /// Логика взаимодействия для TestControl.xaml
    /// </summary>
    public partial class TestControl : UserControl
    {
        public TestControl()
        {
            InitializeComponent();
        }

        public BitmapImage ImageSource
        {
            get { return (BitmapImage)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(BitmapImage), typeof(PictureControl), new PropertyMetadata(null));


        public double BackgroundOpacity
        {
            get { return (double)GetValue(BackgroundOpacityProperty); }
            set { SetValue(BackgroundOpacityProperty, value); }
        }

        public static readonly DependencyProperty BackgroundOpacityProperty =
            DependencyProperty.Register("BackgroundOpacity", typeof(double), typeof(PictureControl), new PropertyMetadata(1.0));

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            BGMouseLeftButtonDown?.Execute(null);
            //Grid grid = new Grid();
            //pictureControl.Visibility = Visibility.Hidden;
        }


        public ICommand BGMouseLeftButtonDown
        {
            get { return (ICommand)GetValue(BGMouseLeftButtonDownProperty); }
            set { SetValue(BGMouseLeftButtonDownProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BGMouseLeftButtonDown.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BGMouseLeftButtonDownProperty =
            DependencyProperty.Register("BGMouseLeftButtonDown", typeof(ICommand), typeof(PictureControl), new PropertyMetadata());
    }
}
