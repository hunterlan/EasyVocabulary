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
                        }
                    }

                    if (accountUser != null)
                    {
                        // Gmail SMTP server address
                        SmtpServer oServer = new SmtpServer("smtp.gmail.com");
                        // Using 587 port, you can also use 465 port
                        oServer.Port = 587;
                        oServer.ConnectType = SmtpConnectType.ConnectSSLAuto;
                        oServer.User = "EasyTeamHelp@gmail.com";
                        oServer.Password = "20002809ksoh"; //TODO: Hide it

                        SmtpMail oMail = new SmtpMail("TryIt");
                        oMail.From = oServer.User;
                        oMail.To = accountUser.Email;
                        oMail.Subject = "Restroring account";
                        oMail.TextBody = "Your login: " + accountUser.Nickname + "\nYour new password: " + accountUser.Password +
                            "\n\nRespectfully, \nEasy Team";

                        SmtpClient oSmtp = new SmtpClient();
                        oSmtp.SendMail(oServer, oMail);

                        accountUser.Password = SecurePasswordHasher.Hash(accountUser.Password);
                        UserController.UpdateUser(_userContext, accountUser);

                        Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception в лицо!");
            }
        }
    }
}
