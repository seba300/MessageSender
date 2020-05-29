using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Threading;
using System.Windows;

namespace MessageSender
{
    class SerialConnection
    {
        private SerialPort _SerialPort { get; set; }

        //Set port options and open connection
        public SerialConnection()
        {
            SetPort();
            OpenPort();
        }

        //Close connection
        ~SerialConnection()
        {
            ClosePort();
        }
        
        //Configure port options
        public void SetPort()
        {
            _SerialPort = new SerialPort();

            _SerialPort.PortName = "COM8";
            _SerialPort.BaudRate = 9600;
            _SerialPort.Parity = Parity.None;
            _SerialPort.DataBits = 8;
            _SerialPort.StopBits = StopBits.One;
            _SerialPort.Handshake = Handshake.None;

            // Set the read/write timeouts
            _SerialPort.ReadTimeout = 500;
            _SerialPort.WriteTimeout = 500;
        }

        //Open port connection
        public void OpenPort()
        {
            _SerialPort.Open();
        }

        //Close port connection
        public void ClosePort()
        {
            _SerialPort.Close();
        }

        //Gsm commands which would be send to controller
        public void GsmCommands()
        {
            _SerialPort.WriteLine("AT");
            Thread.Sleep(200);
            _SerialPort.ReadLine();

            _SerialPort.WriteLine("AT+CMGF=1");
            Thread.Sleep(200);
            _SerialPort.ReadLine();

            _SerialPort.WriteLine("AT+CSMP=17,168,2,25");
            Thread.Sleep(200);
            _SerialPort.ReadLine();

            _SerialPort.WriteLine("AT+CSCS=\"UCS2\"");
            Thread.Sleep(200);
            _SerialPort.ReadLine();
        }

        //Show gsm response
        public void ShowGsmResponse()
        {
            MessageBox.Show(_SerialPort.ReadExisting());
        }
    }
}
