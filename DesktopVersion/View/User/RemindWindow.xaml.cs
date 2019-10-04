using ConsoleVersion.Controllers;
using ConsoleVersion.Helper;
using ConsoleVersion.Models;
using System.Linq;
using System.Windows;
using EASendMail;
using System;

namespace DesktopVersion
{
    /// <summary>
    /// Логика взаимодействия для RemindWindow.xaml
    /// </summary>
    public partial class RemindWindow : Window
    {
        UserContext _userContext;
        public RemindWindow(UserContext getContext)
        {
            InitializeComponent();
            _userContext = getContext;
        }

        private void Restore_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var users = _userContext.Users.ToList();
                User accountUser = null;

                if (EmailBox.Text.Length > 0)
                {
                    foreach (var item in users)
                    {
                        if (item.Email == EmailBox.Text)
                        {
                            accountUser = new User();
                            accountUser.Id = item.Id;
                            accountUser.Nickname = item.Nickname;
                            accountUser.Password = UserController.GeneratePassword();
                            accountUser.Email = EmailBox.Text;
                            break;
                        }
                    }

                    if (accountUser != null)
                    {
                        bool result = UserController.RestoreUser(accountUser, _userContext);
                        if(result == false)
                        {
                            Exceptions.IsError = 0;
                            MessageBox.Show(Exceptions.ErrorMessage, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            MessageBox.Show("Check your email.");
                            Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("User doesn't exist accoring this email!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Close();
            }
        }
    }
}
