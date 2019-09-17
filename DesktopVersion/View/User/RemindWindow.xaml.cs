using ConsoleVersion.Controllers;
using ConsoleVersion.Helper;
using ConsoleVersion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
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
            var users = _userContext.Users.ToList();
            User accountUser = null;

            if(EmailBox.Text.Length > 0)
            {
                foreach (var item in users)
                {
                    if (item.Email == EmailBox.Text)
                    {
                        accountUser = new User();
                        accountUser.Nickname = item.Nickname;
                        accountUser.Password = item.Password;
                        accountUser.Email = EmailBox.Text;
                    }
                }

                //TODO: Сбросить пароль, и задать пользователю новый.

                if(accountUser != null)
                {
                    MailAddress from = new MailAddress("EasyTeamHelp@gmail.com", "Easy Team Support");
                    MailAddress to = new MailAddress(accountUser.Email);
                    MailMessage message = new MailMessage(from, to);

                    message.Subject = "Restoring account";
                    message.Body = "Your login: " + accountUser.Nickname + "\nYour password: " + accountUser.Password +
                        "\n\nRespectfully, \nEasy Team";

                    SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);

                    smtp.Credentials = new NetworkCredential("EasyTeamHelp@gmail.com", "20002809ksoh");
                    smtp.EnableSsl = true;
                    smtp.Send(message);

                    Close();
                }
            }
        }
    }
}
