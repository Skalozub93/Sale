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
            ShowSalesCount();
            ShowDailyStatistic();
        }



        #region Show Monitor
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
            String sql = "SELECT COUNT(*) FROM Managers\r\n";
            using var cmd = new SqlCommand(sql, _connection);
            MonitorManagers.Content = Convert.ToString(cmd.ExecuteScalar());

        }

        private void ShowProductsCount()
        {
            String sql = "SELECT COUNT(*) FROM Products\r\n";
            using var cmd = new SqlCommand(sql, _connection);
            MonitorProducts.Content = Convert.ToString(cmd.ExecuteScalar());

        }


        private void ShowSalesCount()
        {
            String sql = "SELECT COUNT(*) FROM Sales\r\n";
            using var cmd = new SqlCommand(sql, _connection);
            MonitorSales.Content = Convert.ToString(cmd.ExecuteScalar());

        }

        #endregion


        /// <summary>
        /// Заполняет блок "Статистика за день"
        /// </summary>
        private void ShowDailyStatistic()
        {
            SqlCommand cmd = new SqlCommand()
            {
                Connection = _connection

            };
            // В БД информация за 2022 год, поэтому формируя дату с текущим днем и месяцем, но за 2022
            String date = $"2022-{DateTime.Now.Month}-{DateTime.Now.Day}";

            // Всего продаж (чеков)
            cmd.CommandText = $"SELECT COUNT(*) FROM Sales S WHERE CAST( S.Moment AS DATE ) = '{date}'\r\n";
            StatTotalSales.Content = Convert.ToString(cmd.ExecuteScalar());

            // Всего продаж (товарров, штук)
            cmd.CommandText = $"SELECT SUM(S.Cnt) FROM Sales S WHERE CAST( S.Moment AS DATE ) = '{date}'\r\n";
            StatTotalProducts.Content = Convert.ToString(cmd.ExecuteScalar());
            cmd.Dispose();

            // Всего продаж (грн, деньги)
            cmd.CommandText = cmd.CommandText = $"SELECT ROUND( SUM( S.Cnt * P.Price ), 2 ) FROM Sales S JOIN Products P ON S.Id_product = P.Id WHERE CAST( S.Moment AS DATE ) = '{date}'";
            StatTotalMoney.Content = Convert.ToString(cmd.ExecuteScalar());
            cmd.Dispose();

        }
    }
}

/* Дополнить блок "Статистика за день" данными следующих категорий:
 * Самый эффективный менеджер [Фамилия, Имя](по деньгам)
 * Самый эффективный отдел [Название]( по кол-ву проданных товаров)
 * Самый популярный товар [Название] (по кол-ву чеков)
*/