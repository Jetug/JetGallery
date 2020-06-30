using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PicEditor
{
    static class Global
    {
        public static Action<DialogResult> Show { get; set; }
    }
}
