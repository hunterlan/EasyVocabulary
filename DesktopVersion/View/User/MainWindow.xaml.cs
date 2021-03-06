﻿using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using ConsoleVersion.Controllers;
using ConsoleVersion.Helper;
using ConsoleVersion.Models;
using ConsoleVersion.Resources;
using System.Globalization;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace DesktopVersion
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static UserContext _userContext = new UserContext();
        private User sessionUser;
        private readonly string PATH = "user.dat";
        public MainWindow()
        {
            InitializeComponent();
            sessionUser = LoadUser();
            if (sessionUser != null)
                ShowVocabulary(sessionUser);
            //Internalization();
        }

        //private void Internalization()
        //{
        //    SignIn.Content = Resource.SignIn;
        //}

        private bool SaveUser(User user)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            try
            {
                using (FileStream fs = new FileStream(PATH, FileMode.Create))
                {
                    formatter.Serialize(fs, user);
                }
                return true;
            }
            catch(Exception ex)
            {
                Exceptions.Catching(ex);
            }

            return false;
        }

        private User LoadUser()
        {
            User loadedUser = null;
            BinaryFormatter formatter = new BinaryFormatter();

            try
            {
                using(FileStream fs = new FileStream(PATH, FileMode.Open))
                {
                   loadedUser = (User)formatter.Deserialize(fs);
                }
            }
            catch(Exception ex)
            {
                Exceptions.Catching(ex);
            }
            return loadedUser;
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
                    if (UseCookie.IsChecked == true)
                        SaveUser(currentUser);
                    ShowVocabulary(currentUser);
                }
            }
        }

        private void ShowVocabulary(User currentUser)
        {
            Table vocabularyWindow = new Table(currentUser, _userContext);
            vocabularyWindow.Show();
            Close();
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            Registration registrationWindow = new Registration();
            bool ready = false;
            do
            {
                if (registrationWindow.ShowDialog() == true)
                {
                    if (UserController.IsPasswordSecure(registrationWindow.user.Password))
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
                            ready = true;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Password must contain 1 upper case and 1 numeric!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);

                    }
                }
            } while (ready == false);
        }

        private void Hyperlink_Click_1(object sender, RoutedEventArgs e)
        {
            RemindWindow window = new RemindWindow(_userContext);
            window.ShowDialog();
        }
    }
}
