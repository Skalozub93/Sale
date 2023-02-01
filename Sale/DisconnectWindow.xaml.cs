using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
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
    /// Логика взаимодействия для DisconnectWindow.xaml
    /// </summary>
    public partial class DisconnectWindow : Window
    {
        public ObservableCollection<Entities.Department> Departments { get; set; }
        public ObservableCollection<Entities.Product> Products { get; set; }
        public ObservableCollection<Entities.Manager> Managers { get; set; }

        public DisconnectWindow()
        {
            InitializeComponent();
            // Связывание. Часть 1. Контекст
            DataContext = this; // Представление получает доступ к всему обьекту окна
            using SqlConnection connection = new(App.ConnectionString);
           
            try
            {
                connection.Open();

                #region Departments
                Departments = new();
                using SqlCommand cmd = new SqlCommand("SELECT Id, Name FROM Departments", connection);
                {
                    using var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Departments.Add(new()
                        {
                            Id = reader.GetGuid(0),
                            Name = reader.GetString(1),
                        });
                    }
                   
                }
                #endregion

                #region Products
                Products = new();
                using SqlCommand cmd2 = new SqlCommand("SELECT Id, Name, Price FROM Products", connection);
                {
                    using var reader = cmd2.ExecuteReader();
                    while (reader.Read())
                    {
                        Products.Add(new()
                        {
                            Id = reader.GetGuid(0),
                            Name = reader.GetString(1),
                            Price = reader.GetDouble(2)
                        }); 
                    }
                }
                #endregion


                #region Managers
                Managers = new();
                using SqlCommand cmd3 = new SqlCommand("SELECT Id, Name FROM Managers", connection);
                {
                    using var reader = cmd3.ExecuteReader();
                    while (reader.Read())
                    {
                        Managers.Add(new()
                        {
                            Id = reader.GetGuid(0),
                            Name = reader.GetString(1),            
                        }); 
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Close();
            }          
        }


        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListViewItem item)  // item = sender as ListViewItem 
            {
                if(item.Content is Entities.Department department)
                {
                    MessageBox.Show(department.ToString());
                }
                
            }
        }

        private void ListViewItem_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListViewItem item)  // item = sender as ListViewItem 
            {
                if (item.Content is Entities.Product product)
                {
                    MessageBox.Show(product.ToString());
                }

            }
        }

        private void ListViewItem_MouseDoubleClick_2(object sender, MouseButtonEventArgs e)
        {

            if (sender is ListViewItem item)  // item = sender as ListViewItem 
            {
                if (item.Content is Entities.Manager manager)
                {
                    MessageBox.Show(manager.ToString());
                }

            }
        }
    }
}
