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
using System.Windows.Forms;
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
        public int FromIdu { get; set; }
        public int ToIdu { get; set; }
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
            //Combobox hidden idu
            string number = PeopleList.Where(x => x.Idu == Convert.ToInt32(PhoneBook.SelectedValue)).Select(x => x.PhoneNumber).First();
            ToIdu = Convert.ToInt32(PhoneBook.SelectedValue);
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
            //Info label layout set
            SendInfo.Visibility = Visibility.Visible;
            SendInfo.Foreground = Brushes.Green;
            SendInfo.Background = Brushes.LightGreen;
            SendInfo.BorderBrush = Brushes.Green;
            SendInfo.Content = "Sended";

            SendingHistory();
        }

        //Red info
        private void SendedInterrupted()
        {
            //Info label layout set
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

        private void SendingHistory()
        {
            Query query = new Query();
            query.AddToSendHistory(FromIdu, ToIdu, DateTime.Now);
        }

        private void SignOut_Click(object sender, RoutedEventArgs e)
        {
            SignIn signin = new SignIn();
            signin.Show();
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ColorDialog cdg = new ColorDialog();
            if(cdg.ShowDialog()==System.Windows.Forms.DialogResult.OK)
            {
                Tab.Background = new SolidColorBrush(Color.FromArgb(cdg.Color.A, cdg.Color.R, cdg.Color.G, cdg.Color.B));
            }
        }
    }
}

