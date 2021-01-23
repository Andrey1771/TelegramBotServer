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
        BillyTelegramBot bot;
        

        public MainWindow()
        {
            InitializeComponent();
        }

        private void UpdateDataGridBot(IBillyTelegramBot bot)
        {
            usersDataGrid.DataContext = bot.Users;
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
            bot = new BillyTelegramBot(@"E:\Visual Projects\Skillbox\Lab_9\BillyContent",
               @"C:\Users\Andrey\Desktop\BillyToken.txt", @"E:\Visual Projects\Skillbox\Lab_9\BillyContent", @"C:\Users\Andrey\Desktop\GoogleToken.txt");
            bot.StartBot();
            bot.SendAllMessage("GayChillMans with love, Aniki");
        }

        private void onOffBotButton_Click(object sender, RoutedEventArgs e)
        {
            //bot.
            //bot.
            //pathTokenTelegramTextBox.Text
            if (!File.Exists(pathTokenTelegramTextBox.Text))
            {
                MessageBox.Show("pathTokenTelegramTextBox Неверный путь к файлу токена");
                return;
            }
            if (!Directory.Exists(pathSaveLoadTextBox.Text))
            {
                MessageBox.Show("pathSaveLoadTextBox Неверный путь папки сохранения и загрузки");
                return;
            }
            if (!Directory.Exists(pathSaverSystemTextBox.Text))
            {
                MessageBox.Show("pathSaverSystemTextBox Неверный путь папки системных данных");
                return;
            }

            if(bot == null)
            {
                bot = new BillyTelegramBot(pathSaveLoadTextBox.Text, pathTokenTelegramTextBox.Text, pathSaverSystemTextBox.Text);
                UpdateDataGridBot(bot);
            }

            if(!bot.Enable)
            {
                bot.StartBot();
            }
            else
            {
                bot.StopBot();
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
    }
}
