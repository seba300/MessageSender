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
            SetGSMModule();
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
        private void SetGSMModule()
        {
            //Test connection with SIM card
            _SerialPort.WriteLine("AT");
            Thread.Sleep(200);
            _SerialPort.ReadLine(); //AT
            _SerialPort.ReadLine(); //OK/ERROR

            //Clean message storage
            _SerialPort.WriteLine("AT+CMGD=1,4");
            Thread.Sleep(200);
            _SerialPort.ReadLine();//OK/ERROR

            //Set sms text mode
            _SerialPort.WriteLine("AT+CMGF=1");
            Thread.Sleep(200);
            _SerialPort.ReadLine();//OK/ERROR

            //Set the parameters for an outgoing message 
            _SerialPort.WriteLine("AT+CSMP=17,168,2,25");
            Thread.Sleep(200);
            _SerialPort.ReadLine();//OK/ERROR

            //Set Character set to UCS2 - 16-bit universal multiple-octet coded character set that's allows polish letters
            _SerialPort.WriteLine("AT+CSCS=\"UCS2\"");
            Thread.Sleep(200);
            _SerialPort.ReadLine();//OK/ERROR
        }

        //Send message
        public void SendMsg(string num, string msg)
        {
            string message = EncodeToUnicode(msg);
            string number = EncodeToUnicode(num);

            //Write phone number. +CMGS command giving us possibilty to send message immediately after giving text. We don't need to storage this message in memory
            _SerialPort.WriteLine("AT+CMGS=" + "\""+number+ "\"");
            Thread.Sleep(200);

            //Write message
            _SerialPort.WriteLine(message+char.ConvertFromUtf32(26));
            Thread.Sleep(200);
            
            //MESSAGE SENDED

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

//AT <-- check card connection
//AT+CMGD=1,4 <-- clean storage
//AT+CMGF=1 <-- text mode
//AT+CSMP=17,168,2,25
//AT+CSCS="UCS2" <-- message coding format (UTF-16)
//AT+CMGS="002b00340038003600300032003700340038003300320033"<--- utf-16 <-- number +48602748323
//> 0107017c <--ćż

