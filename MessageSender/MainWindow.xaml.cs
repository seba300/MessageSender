using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace MessageSender
{

   

    public partial class MainWindow : Window
    {
        public SerialPort _serialPort;
        public MainWindow()
        {
            //SetPort();
            //OpenPort();
            //GsmCommands();
            //ShowGsmResponse();
            //ClosePort();
            
        }

        public void SetPort()
        {
            _serialPort = new SerialPort();

            _serialPort.PortName = "COM8";
            _serialPort.BaudRate = 9600;
            _serialPort.Parity = Parity.None;
            _serialPort.DataBits = 8;
            _serialPort.StopBits = StopBits.One;
            _serialPort.Handshake = Handshake.None;

            // Set the read/write timeouts
            _serialPort.ReadTimeout = 500;
            _serialPort.WriteTimeout = 500;
        }
        public void OpenPort()
        {
            _serialPort.Open();
        }
        public void ClosePort()
        {
            _serialPort.Close();
        }
        public void GsmCommands()
        {
            _serialPort.WriteLine("AT");
            Thread.Sleep(100);
        }
        public void ShowGsmResponse()
        {
            MessageBox.Show(_serialPort.ReadExisting());
        }
    }
}

