using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Network_ImageTask_Server.ViewModels
{
    public class SenderImageViewModel : BaseViewModel
    {
        private BitmapImage image;

        public BitmapImage Image
        {
            get { return image; }
            set { image = value; OnPropertyChanged(); }
        }
    }
}
