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

using System.Data.SqlClient; // Подключаем ADD для MS SQL Server (не забыть NuGet)

namespace Sale
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private SqlConnection _connection;  // Обьект подключения к БД

        public MainWindow()
        {
            InitializeComponent();
            String ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\L\source\repos\Sale\Sale\Sales.mdf;Integrated Security=True";
            _connection = new(ConnectionString);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _connection.Open(); // открытие подключения
                MonitorConnection.Content = "Установлено";
                MonitorConnection.Foreground = Brushes.Green;
            }
            catch(SqlException ex)
            {
                MonitorConnection.Content = "Закрыто";
                MonitorConnection.Foreground = Brushes.Red;
                MessageBox.Show(ex.Message);
                this.Close();
            }
            ShowDepartmentsCount();
            ShowManagersCount();
            ShowProductsCount();
        }

        /// <summary>
        /// Выводит в таблицу монитор количество отделов (департамментов)
        /// </summary>
        private void ShowDepartmentsCount()
        {
            String sql = "SELECT COUNT(*) FROM Departments\r\n";
            using var cmd = new SqlCommand(sql, _connection);
            MonitorDepartments.Content = Convert.ToString(cmd.ExecuteScalar()); 
            
        }

        private void ShowManagersCount()
        {
            String sql = "SELECT COUNT(*) FROM Departments\r\n";
            using var cmd = new SqlCommand(sql, _connection);
            MonitorManagers.Content = Convert.ToString(cmd.ExecuteScalar());

        }

        private void ShowProductsCount()
        {
            String sql = "SELECT COUNT(*) FROM Departments\r\n";
            using var cmd = new SqlCommand(sql, _connection);
            MonitorProducts.Content = Convert.ToString(cmd.ExecuteScalar());

        }
    }
}
