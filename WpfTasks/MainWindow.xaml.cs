using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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

namespace WpfTasks
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

            
        }
        // setup multicast group
        // 224.5.6.7
        private UdpClient udpSend;
        private  UdpClient udpListen;
        private bool listening = false;
        private UdpReceiveResult udpResult;
        string externalip;

        private void getIp() {

            externalip = new WebClient().DownloadString("http://icanhazip.com");
            txtCurrentIp.Text = externalip;
        }
        private void btnSendMessage_Click(object sender, RoutedEventArgs e)
        {
            int port;
            int.TryParse(txtSendPort.Text, out port);
            IPAddress ip;
            IPAddress.TryParse(txtSendIp.Text, out ip);
             
            Byte[] data = Encoding.ASCII.GetBytes(txtMessage.Text);
            try
            {
                if (udpSend == null) {
                }
                    udpSend = new UdpClient();
                    udpSend.Connect(new IPEndPoint(ip, port));

                udpSend.Send(data, data.Length);


            }
            catch (Exception ex)
            {

                txtErrors.Text = ex.Message;
            }
        }

        private async void btnListen_ClickAsync(object sender, RoutedEventArgs e)
        {
            
            
               await listen();
            
        }

        private async Task listen()
        {
            while (true)
            {
                if (udpListen != null)
                {
                    udpListen.Close();
                }
                int listen_port;
                int.TryParse(txtListenPort.Text, out listen_port);

                udpListen = new UdpClient(new IPEndPoint(IPAddress.Any, listen_port));

                udpResult = await udpListen.ReceiveAsync();
                byte[] data = udpResult.Buffer;
                listening = true;

                txtListenResult.Text += "Message From: " + udpResult.RemoteEndPoint.Address.ToString() + ":" + udpResult.RemoteEndPoint.Port.ToString() + ", Message: " + Encoding.ASCII.GetString(data) + "\n";
            }
        }

        private void btnGetIp_Click(object sender, RoutedEventArgs e)
        {
            getIp();
        }
    }
}
