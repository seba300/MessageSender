using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MessageSender
{
    /// <summary>
    /// Interaction logic for SignIn.xaml
    /// </summary>
    public partial class SignIn : Window
    {
        public SignIn()
        {
            InitializeComponent();
        }

        private void SignIn_Click(object sender, RoutedEventArgs e)
        {
            string login = Login.Text;
            string password = Password.Password;

            Query query = new Query();

            int idEmployee = query.GetIdEmployee(login, password);

            if(idEmployee!=0)
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.FromIdu = idEmployee;
                mainWindow.Show();
                this.Close();
            }
            else
            {
                LogFailed.Visibility = Visibility.Visible;
            }
        }
        private void Login_TextChanged(object sender, TextChangedEventArgs e)
        {
            LogFailed.Visibility = Visibility.Hidden;
        }
        private void Password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            LogFailed.Visibility = Visibility.Hidden;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Login.Focus();
        }
    }
}

