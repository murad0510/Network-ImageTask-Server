using Network_ImageTask_Server.Commands;
using Network_ImageTask_Server.Views.UserControls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Network_ImageTask_Server.ViewModels
{
    public class MainViewModel
    {
        public RelayCommand SendButtonCommand { get; set; }

        public MainViewModel()
        {
            SendButtonCommand = new RelayCommand((_) =>
            {
                Task.Run(() =>
                {
                    var ipAdress = IPAddress.Parse("192.168.0.109");
                    var port = 27001;

                    using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                    {
                        var ep = new IPEndPoint(ipAdress, port);
                        socket.Bind(ep);
                        MessageBox.Show("Server is up");
                        socket.Listen(10);
                        while (true)
                        {
                            var client = socket.Accept();

                            MessageBox.Show($"Client connected : {client.RemoteEndPoint}");

                            var length = 0;
                            var bytes = new byte[900000];

                            App.Current.Dispatcher.Invoke((System.Action)delegate
                            {
                                do
                                {
                                    length = client.Receive(bytes);
                                    WrapPanelAdd(bytes);
                                    break;
                                } while (length > 0);
                            });
                        }
                    }
                });
            });
        }

        public void WrapPanelAdd(byte[] bytes)
        {
            SenderImage senderImage = new SenderImage();
            SenderImageViewModel senderImageViewModel = new SenderImageViewModel();
            senderImage.DataContext = senderImageViewModel;

            senderImageViewModel.Image = LoadImage(bytes);

            App.MainMyPanel.Children.Add(senderImage);
            //MessageBox.Show($"{App.MainMyPanel.Children.Count}");
        }

        public static BitmapImage LoadImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0) return null;
            var image = new BitmapImage();
            using (var mem = new MemoryStream(imageData))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }
    }
}
