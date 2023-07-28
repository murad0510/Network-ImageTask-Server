using Network_ImageTask_Server.Commands;
using Network_ImageTask_Server.Views.UserControls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Network_ImageTask_Server.ViewModels
{
    public class MainViewModel
    {
        public RelayCommand SendButtonCommand { get; set; }
        public MainViewModel()
        {
            SendButtonCommand = new RelayCommand((_) =>
            {
                SenderImage senderImage = new SenderImage();
                SenderImageViewModel senderImageViewModel = new SenderImageViewModel();
                senderImage.DataContext = senderImageViewModel;
                Task.Run(() =>
                {
                    var ipAdress = IPAddress.Parse("10.2.29.13");
                    var port = 27014;

                    using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                    {
                        var ep = new IPEndPoint(ipAdress, port);
                        socket.Bind(ep);
                        socket.Listen(10);
                        while (true)
                        {
                            var client = socket.Accept();
                            MessageBox.Show($"{client.RemoteEndPoint}");

                            var length = 0;
                            var bytes = new byte[34000];

                            do
                            {
                                length = client.Receive(bytes);
                                var image = Encoding.ASCII.GetString(bytes);
                                senderImageViewModel.Image = "C:\\\\Users\\\\Azizo_pu01\\\\Downloads\\\\TASKSYSTEM2.png";
                            } while (length > 0);

                        }
                    }
                });
                App.MainStackPanel.Children.Add(senderImage);
            });
        }
    }
}
