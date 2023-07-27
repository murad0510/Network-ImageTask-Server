using Network_ImageTask_Server.Views.UserControls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Network_ImageTask_Server.ViewModels
{
    public class MainViewModel
    {
        public MainViewModel()
        {
            var ipAdress = IPAddress.Parse("192.168.0.109");
            var port = 27001;

            using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                var ep = new IPEndPoint(ipAdress, port);
                socket.Listen(10);
                socket.Bind(ep);
                while (true)
                {
                    var client = socket.Accept();
                    SenderImage senderImage = new SenderImage();
                    SenderImageViewModel senderImageViewModel = new SenderImageViewModel();
                    senderImage.DataContext = senderImageViewModel;
                    Task.Run(() =>
                    {
                        var length = 0;
                        var bytes = new byte[500000];

                        do
                        {
                            length = client.Receive(bytes);
                            var image = Encoding.UTF8.GetString(bytes);
                            senderImageViewModel.Image = image;
                            App.MainStackPanel.Children.Add(senderImage);
                        } while (length > 0);
                    });
                }
            }
        }
    }
}
