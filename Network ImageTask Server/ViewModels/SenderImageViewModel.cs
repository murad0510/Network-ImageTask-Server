using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network_ImageTask_Server.ViewModels
{
    public class SenderImageViewModel : BaseViewModel
    {
        private string image;

        public string Image
        {
            get { return image; }
            set { image = value; OnPropertyChanged(); }
        }

    }
}
