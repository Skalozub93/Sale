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

namespace Sale
{
    /// <summary>
    /// Логика взаимодействия для PortalWindow.xaml
    /// </summary>
    public partial class PortalWindow : Window
    {
        public PortalWindow()
        {
            InitializeComponent();
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            this.Hide();
            new MainWindow().ShowDialog();
            this.Show();
        }

        private void ButtonClick_1(object sender, RoutedEventArgs e)
        {
            this.Hide();
            new DisconnectWindow().ShowDialog();
            this.Show();
        }

       
    }
}
