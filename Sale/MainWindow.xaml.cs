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
using Sale.Entities;
using System.Data;

namespace Sale
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private SqlConnection _connection;  // Обьект подключения к БД
        private List<Entities.Department>? _departments; //ORM: колекция обьектов - сущностей == таблица
        private List<Entities.Product>? _products;
        private List<Entities.Manager>? _managers;
        private List<Entities.TodaySales>? _todaysales;

        public MainWindow()
        {
            InitializeComponent();
            // создание обьекта-подключения!!!
            _connection = new(App.ConnectionString);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _connection.Open(); // открытие подключения
                MonitorConnection.Content = "Установлено";
                MonitorConnection.Foreground = Brushes.Green;
            }
            catch (SqlException ex)
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


            ShowDepartmentsORM();
            ShowProductsORM();
            ShowManagersORM();
            ShowTodaySalesORM();


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


            cmd.CommandText = $"SELECT TOP 1 m.Surname + ' ' + m.Name " +
                $"FROM Sales s JOIN Managers m  ON s.ID_manager = m.Id " +
                $"JOIN Products p " +
                $"ON s.ID_product = p.Id WHERE CAST(s.Moment AS DATE) = '{date}' " +
                $"GROUP BY  m.Surname + ' ' + m.Name\r\nORDER BY SUM(s.Cnt * p.Price) DESC\r\n ";
            StatTopManager.Content = cmd.ExecuteScalar().ToString();

            cmd.CommandText = $"SELECT TOP 1 d.Name FROM Sales s JOIN Managers m ON s.ID_manager = m.Id JOIN Products p ON s.ID_product = p.Id JOIN Departments d ON m.Id_main_dep = d.Id WHERE CAST(s.Moment AS DATE) = '{date}' GROUP BY d.Name ORDER BY SUM(s.Cnt) DESC ";
            StatTopDepart.Content = cmd.ExecuteScalar().ToString();

            cmd.CommandText = $"SELECT TOP 1 p.Name FROM Sales s JOIN  Products p ON s.ID_product = p.Id WHERE CAST(s.Moment AS DATE) = '{date}' GROUP BY p.Name ORDER BY SUM(s.Cnt) DESC";
            StatTopProduct.Content = cmd.ExecuteScalar().ToString();

            cmd.Dispose();


        }

        private void ShowDepartments()
        {
            using SqlCommand cmd = new("SELECT * FROM Departments", _connection);   //Табличный запрос - возвращает SqlDataReader
            SqlDataReader reader = cmd.ExecuteReader();                             //Передача данных происходит по строчно - по одной строке в выборке(рещультата)
            DepartmentCell.Text = "";                                               // Вызов ExecuteReader не читает данные, только создаёт reader
            while (reader.Read())                                                   // Команда Reader.Read() заполняет reader данными ( строкой - выборкой) - "Самозаполняется"
            {                                                                       //!! Возврат Read() - статус (успех\конец)
                Guid id = reader.GetGuid("id");                                     // После чтения данные доступны 
                String name = (String)reader[1];                                    // а) через Get-теры (GetGuid\GetString...)
                                                                                    // б) через Get-теры с именем поля (using System.Data)
                DepartmentCell.Text += $"{id} {name} \n";                           // в) через индексатор [] с числом - индексом поля
                                                                                    // г) через индексатор [] c строкой - название пол                                                                       //Лучше перечислять поля SELECT id, name FROM Departments
                                                                                    // readre обязательно нужно закрывать, если останеться открытым, то не будут выполняться другие команды
            }
            reader.Dispose();
        }


        private void ShowDepartmentsORM()
        {
            if (_departments is null) // 1) обращение - заполняем коллекцию 
            {
                using SqlCommand cmd = new("SELECT D.Id, D.Name FROM Departments D", _connection);
                try
                {
                    using SqlDataReader reader = cmd.ExecuteReader();
                    _departments = new();
                    while (reader.Read())
                    {
                        _departments.Add(new()
                        {
                            Id = reader.GetGuid("id"),
                            Name = reader.GetString(1)
                        });
                    }
                }
                catch (SqlException ex)
                {

                    MessageBox.Show(ex.Message);
                    return;
                }

            }
            DepartmentCell.Text = "";
            foreach (var department in _departments)
            {
                DepartmentCell.Text += department.ToShortString() + "\n";
            }

        }



        private void ShowProductsORM()
        {
            if (_products is null) // 1) обращение - заполняем коллекцию 
            {
                using SqlCommand cmd = new("SELECT P.Id, P.Name, P.Price FROM Products P", _connection);
                try
                {
                    using SqlDataReader reader = cmd.ExecuteReader();
                    _products = new();
                    while (reader.Read())
                    {
                        _products.Add(new()
                        {
                            Id = reader.GetGuid("id"),
                            Name = reader.GetString(1),
                            Price = reader.GetDouble(2)
                        });
                    }
                }
                catch (SqlException ex)
                {

                    MessageBox.Show(ex.Message);
                    return;
                }

            }
            ProductsCell.Text = "";
            foreach (var product in _products)
            {
                ProductsCell.Text += product.ToShortString() + "\n";
            }
        }

        private void ShowManagersORM()
        {
            if (_managers is null) // 1) обращение - заполняем коллекцию 
            {
                using SqlCommand cmd = new("SELECT M.Id, M.Surname, M.Name, M.Secname FROM Managers M", _connection);
                try
                {
                    using SqlDataReader reader = cmd.ExecuteReader();
                    _managers = new();
                    while (reader.Read())
                    {
                        _managers.Add(new()
                        {
                            Id = reader.GetGuid("id"),
                            Name = reader.GetString("Name"),
                            Surname = reader.GetString("Surname"),
                            Secname = reader.GetString("Secname")
                        });
                    }
                }
                catch (SqlException ex)
                {

                    MessageBox.Show(ex.Message);
                    return;
                }

            }
            ManagersCell.Text = "";
            foreach (var managers in _managers)
            {
                ManagersCell.Text += managers.ToShortString() + "\n";
            }
        }

        private void ShowTodaySalesORM()
        {
            String date = $"2022-{DateTime.Now.Month}-{DateTime.Now.Day}";
            if (_todaysales is null) // 1) обращение - заполняем коллекцию 
            {
                using SqlCommand cmd = new($"SELECT s.ID, p.Name , s.Cnt, p.price FROM Sales s JOIN Products p ON s.ID_product = p.id WHERE CAST(s.Moment AS DATE) = '{date}'", _connection);
                try
                {
                    using SqlDataReader reader = cmd.ExecuteReader();
                    _todaysales = new();
                    while (reader.Read())
                    {
                        _todaysales.Add(new()
                        {
                            Id = reader.GetGuid("ID"),
                            Name = reader.GetString("Name"),
                            Count = reader.GetInt32("Cnt"),
                            Sum = reader.GetDouble("Price") * reader.GetInt32("Cnt")
                        });
                    }
                }
                catch (SqlException ex)
                {

                    MessageBox.Show(ex.Message);
                    return;
                }

            }
            TodaySalesCell.Text = "";
            foreach (var todaySales in _todaysales)
            {
                TodaySalesCell.Text += todaySales.ToShortString() + "\n";
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (_connection?.State == ConnectionState.Open)
            {
                _connection.Close();
            }
        }
    }

}
