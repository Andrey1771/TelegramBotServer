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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using Lab_9;
using System.IO;

namespace Lab_10
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        AlexeyTelegramBot bot;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void UpdateDataGridBot(IAlexeyTelegramBot bot)
        {
            usersDataGrid.ItemsSource = bot.Users;
        }

        private void pathTokenTelegramButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            

            if(openFileDialog.ShowDialog(this) == true)
            {
                pathTokenTelegramTextBox.Text = openFileDialog.FileName;
            }
        }

        private void pathSaveLoadButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();


            if (openFileDialog.ShowDialog(this) == true)
            {
                pathSaveLoadTextBox.Text = openFileDialog.FileName;
            }
        }

        private void mailingButton_Click(object sender, RoutedEventArgs e)
        {
            if(bot == null)
            {
                MessageBox.Show("Бот не включен", "Bot is off");
                return;
            }else if(bot.Enable == false)
            {
                MessageBox.Show("Бот не включен", "Bot is off");
                return;
            }

            MailingWindow mailWindow = new MailingWindow();
            mailWindow.ShowDialog();
            if(mailWindow.Message != "")
                bot.SendAllMessage(mailWindow.Message);
        }


        private bool CheckPathesBot()
        {
            if (!File.Exists(pathTokenTelegramTextBox.Text))
            {
                MessageBox.Show("pathTokenTelegramTextBox Неверный путь к файлу токена", "Неверный путь");
                return false;
            }
            if (!Directory.Exists(pathSaveLoadTextBox.Text))
            {
                MessageBox.Show("pathSaveLoadTextBox Неверный путь папки сохранения и загрузки", "Неверный путь");
                return false;
            }
            if (!Directory.Exists(pathSaverSystemTextBox.Text))
            {
                MessageBox.Show("pathSaverSystemTextBox Неверный путь папки системных данных", "Неверный путь");
                return false;
            }

            return true;
        }

        private bool CreateBot()
        {
            if(CheckPathesBot())
            {
                bot = new AlexeyTelegramBot(pathSaveLoadTextBox.Text, pathTokenTelegramTextBox.Text, pathSaverSystemTextBox.Text);
                UpdateDataGridBot(bot);
                return true;
            }
            return false;
        }

        private void onOffBotButton_Click(object sender, RoutedEventArgs e)
        {
            if (bot == null)
            {
                if (!CreateBot())
                    return;
            }


            if(!bot.Enable)
            {
                bot.StartBot();
                onOffBotButton.Background = Brushes.Green;
                onOffBotButton.Content = "Off";
            }
            else
            {
                bot.StopBot();
                onOffBotButton.Background = Brushes.Red;
                onOffBotButton.Content = "On";
            }
        }

        private void pathSaverSystemButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();


            if (openFileDialog.ShowDialog(this) == true)
            {
                pathSaverSystemTextBox.Text = openFileDialog.FileName;
            }
        }

        private void settings1CheckBox_Click(object sender, RoutedEventArgs e)
        {
            if(bot == null)
            {
                settings1CheckBox.IsChecked = !settings1CheckBox.IsChecked;
                return;
            }
            Settings newSettings = new Settings(bot.Settings);
            newSettings.isLoadData = !newSettings.isLoadData;
            bot.Settings = newSettings;
        }

        private void settings2CheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (bot == null)
            {
                settings2CheckBox.IsChecked = !settings2CheckBox.IsChecked;
                return;
            }
            Settings newSettings = new Settings(bot.Settings);
            newSettings.isSendData = !newSettings.isSendData;
            bot.Settings = newSettings;
        }

        private void settings3CheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (bot == null)
            {
                settings1CheckBox.IsChecked = !settings1CheckBox.IsChecked;
                return;
            }
            Settings newSettings = new Settings(bot.Settings);
            newSettings.isRegistrationNewUsers = !newSettings.isRegistrationNewUsers;
            bot.Settings = newSettings;
        }
    }
}
