using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using ConsoleVersion.Controllers;
using ConsoleVersion.Helper;
using ConsoleVersion.Models;

namespace DesktopVersion
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static UserContext _userContext = new UserContext();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            User currentUser = new User
            {
                Nickname = BoxLogin.Text,
                Password = BoxPassword.Password
            };

            currentUser = UserController.CompareUser(_userContext, currentUser);
            if (currentUser == null)
            {
                if(Exceptions.IsError != 1)
                    MessageBox.Show("Nickname or password wrong!");
                else
                    MessageBox.Show(Exceptions.ErrorMessage);
            }
            else
            {
                Table vocabularyWindow = new Table(currentUser);
                vocabularyWindow.Show();
                Close();
            }
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            Registration registrationWindow = new Registration();
            if(registrationWindow.ShowDialog() == true)
            {
                UserController.AddUser(_userContext, ref registrationWindow.user);
                Table vocabularyWindow = new Table(registrationWindow.user);
                vocabularyWindow.Show();
                Close();
            }
        }
    }
}
