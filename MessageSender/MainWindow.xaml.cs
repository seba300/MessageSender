using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
        public List<Person> PeopleList { get; set; }
       
        public MainWindow()
        {
            InitializeComponent();
            UploadBookList();
        }

        //Complete people list
        private void UploadBookList()
        {
            Query query = new Query();
            PeopleList = query.GetEmployeeData();
            DataContext = this;
        }

        private void SendMsg_Click(object sender, RoutedEventArgs e)
        {
            //Combobox hidden number  
            string number = PhoneBook.SelectedValue.ToString();
            string message = Msg.Text;
            
            if (!String.IsNullOrWhiteSpace(message))
            {
                try
                {
                    SerialConnection serialConnection = new SerialConnection();
                    serialConnection.SendMsg(number, message);
                    SendedConfirmed();
                }
                catch (Exception)
                {
                    SendedInterrupted();
                }
            }
        }

        //Green info
        private void SendedConfirmed()
        {
            SendInfo.Visibility = Visibility.Visible;
            SendInfo.Foreground = Brushes.Green;
            SendInfo.Background = Brushes.LightGreen;
            SendInfo.BorderBrush = Brushes.Green;
            SendInfo.Content = "Sended";
        }

        //Red info
        private void SendedInterrupted()
        {
            SendInfo.Visibility = Visibility.Visible;
            SendInfo.Foreground = Brushes.Maroon;
            SendInfo.Background = Brushes.Red;
            SendInfo.BorderBrush = Brushes.Maroon;
            SendInfo.Content = "Not sended";
        }

        //Hide label with info about sending message
        private void Msg_TextChanged(object sender, TextChangedEventArgs e)
        {
            SendInfo.Visibility = Visibility.Hidden;
        }
    }
}

