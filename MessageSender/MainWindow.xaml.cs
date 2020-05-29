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
            UploadBookList();

        }
        private void UploadBookList()
        {
            Query query = new Query();
            PeopleList = query.GetEmployeeData();
            DataContext = this;
        }

        private void SendMsg_Click(object sender, RoutedEventArgs e)
        {
            string a = PhoneBook.SelectedValue.ToString();
            MessageBox.Show(a);
        }
        public void EncodeToUnicode(string message)
        {
            UnicodeEncoding uni = new UnicodeEncoding();
            byte[] encodedBytes = uni.GetBytes(message);
            string text = "";
            for (int i = 0; i < encodedBytes.Length; i += 2)
            {
                text += string.Format("{0:X2}", encodedBytes[i + 1]) + string.Format("{0:X2}", encodedBytes[i]);
            }
        }
    }
}

