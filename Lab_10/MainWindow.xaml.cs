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
            
        }

        private void onOffBotButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
