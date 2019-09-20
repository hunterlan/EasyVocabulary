using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using ConsoleVersion.Controllers;
using ConsoleVersion.Helper;
using ConsoleVersion.Models;
using ConsoleVersion.Resources;
using System.Globalization;

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
            //Internalization();
        }

        private void Internalization()
        {
            SignIn.Content = Resource.SignIn;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(BoxLogin.Text.Length == 0 || BoxPassword.Password.Length == 0)
                MessageBox.Show("Password or/and nickname box empty!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                User currentUser = new User
                {
                    Nickname = BoxLogin.Text,
                    Password = BoxPassword.Password
                };

                currentUser = UserController.CompareUser(_userContext, currentUser);
                if (currentUser == null)
                {
                    if (Exceptions.IsError != 1)
                        MessageBox.Show("Nickname or password wrong!");
                    else
                    {
                        MessageBox.Show(Exceptions.ErrorMessage);
                        Exceptions.IsError = 0;
                    }
                }
                else
                {
                    Table vocabularyWindow = new Table(currentUser, _userContext);
                    vocabularyWindow.Show();
                    Close();
                }
            }
            
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            Registration registrationWindow = new Registration();
            if (registrationWindow.ShowDialog() == true)
            {
                if(UserController.IsPasswordSecure(registrationWindow.user.Password))
                {
                    if (UserController.AddUser(_userContext, ref registrationWindow.user) == false)
                    {
                        MessageBox.Show(Exceptions.ErrorMessage);
                        Exceptions.IsError = 0;
                    }
                    else
                    {
                        Table vocabularyWindow = new Table(registrationWindow.user, _userContext);
                        vocabularyWindow.Show();
                        Close();
                    }
                }
               else
                {
                    MessageBox.Show("Password must contain 1 upper case and 1 numeric!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Hyperlink_Click_1(object sender, RoutedEventArgs e)
        {
            RemindWindow window = new RemindWindow(_userContext);
            window.ShowDialog();
        }
    }
}
