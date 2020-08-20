using PicEditor.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace PicEditor
{
    class Global
    {
        private static Global instance;

        //public Action<DialogResult> Show { get; set; }
        public Action<BitmapImage> ShowPreview { get; set; }
        public Action HidePreview { get; set; }

        private ImageItem _draggableImage = null;
        public ImageItem DraggableImage
        {
            get => _draggableImage;
            set
            {
                _draggableImage = value;
                if (value != null)
                    ShowPreview(value.Preview);
                else HidePreview();
            }
        }

        public double ImageMouseX { get; set; }
        public double ImageMouseY { get; set; }

        private Global() { }

        public static Global getInstance()
        {
            if (instance == null)
                instance = new Global();
            return instance;
        }
    }
}
