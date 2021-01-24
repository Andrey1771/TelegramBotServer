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

namespace Lab_10
{
    /// <summary>
    /// Логика взаимодействия для MailingWindow.xaml
    /// </summary>
    public partial class MailingWindow : Window
    {
        public string Message { get; set; } = "";

        public MailingWindow()
        {
            InitializeComponent();
        }

        private void sendAllButton_Click(object sender, RoutedEventArgs e)
        {
            Message = sendAllTextBox.Text;
            Close();
        }

        private void sendAllTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Message = sendAllTextBox.Text;
                Close();
            }
        }
    }
}
