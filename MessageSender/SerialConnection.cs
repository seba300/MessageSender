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
            SetGSM();
        }

        //Close connection
        ~SerialConnection()
        {
            ClosePort();
        }
        
        //Configure port options
        private void SetPort()
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
        private void OpenPort()
        {
            _SerialPort.Open();
        }

        //Close port connection
        private void ClosePort()
        {
            _SerialPort.Close();
        }

        //Set GSM properties to send Unicode message
        private void SetGSM()
        {
            //Test connection with card
            _SerialPort.WriteLine("AT");
            Thread.Sleep(200);
            _SerialPort.ReadLine();

            //Set sms text mode
            _SerialPort.WriteLine("AT+CMGF=1");
            Thread.Sleep(200);
            _SerialPort.ReadLine();

            //Set modem parameters
            _SerialPort.WriteLine("AT+CSMP=17,168,2,25");
            Thread.Sleep(200);
            _SerialPort.ReadLine();

            //Set Character set to UCS2 - 16-bit universal multiple-octet coded character set
            _SerialPort.WriteLine("AT+CSCS=\"UCS2\"");
            Thread.Sleep(200);
            _SerialPort.ReadLine();

            //After earlier line try this ////confirm the setting if they wouldn't be confirmed
            //_SerialPort.WriteLine("AT + CSCS ?");
            //_SerialPort.ReadLine();
        }

        //Send message
        public void SendMsg(string num, string msg)
        {
            char CtrlZ = (char)26;
            char CR = (char)13;
            string message = EncodeToUnicode(msg);
            string number = EncodeToUnicode(num);

            //Write phone number
            _SerialPort.WriteLine("AT+CMGW=" + number);
            Thread.Sleep(200);

            //Write message
            _SerialPort.WriteLine(msg);
            Thread.Sleep(200);

            //Write CTRL+Z to close writing buffor
            _SerialPort.WriteLine(string.Format("whatever{0}{1}", CtrlZ, CR));
            //or
            //_SerialPort.WriteLine(((char)26).ToString());
            //or
            //string controlZ = "\u001F";
            //_SerialPort.WriteLine(controlZ);
            Thread.Sleep(200);

            //Send message
            _SerialPort.WriteLine("AT+CMSS=1");
            Thread.Sleep(200);

        }

        ////Show gsm response
        //public void ShowGsmResponse()
        //{
        //    MessageBox.Show(_SerialPort.ReadExisting());
        //}

        //Encode message to unicode. This is the only way to send message with all characters
        private string EncodeToUnicode(string message)
        {
            UnicodeEncoding uni = new UnicodeEncoding();
            byte[] encodedBytes = uni.GetBytes(message);
            string text = "";
            for (int i = 0; i < encodedBytes.Length; i += 2)
            {
                text += string.Format("{0:X2}", encodedBytes[i + 1]) + string.Format("{0:X2}", encodedBytes[i]);
            }
            return text;
        }
    }
}
