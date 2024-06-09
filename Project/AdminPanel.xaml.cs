using Project.Data;
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
using Project.Models;

namespace Project
{
    /// <summary>
    /// Логика взаимодействия для UserPage.xaml
    /// </summary>
    public partial class AdminPanel : Window
    {
        public AdminPanel()
        {
            InitializeComponent();
            LoadUsers();
        }
        private void LoadUsers()
        {
            using (var context = new AppDbContext())
            {
                var usersWithType = new List<object>(); // Список для хранения данных пользователей с типами

                List<Project.Models.users> users = context.users.ToList();
                foreach (var user in users)
                {
                    using (var userTypeContext = new AppDbContext2())
                    {
                        // Находим тип пользователя
                        var userType = userTypeContext.userType.FirstOrDefault(u => u.userID == user.userID);
                        string typeName = string.Empty;

                        if (userType != null)
                        {
                            // Находим название типа
                            var type = context.type.FirstOrDefault(t => t.id == userType.typeID);
                            if (type != null)
                            {
                                typeName = type.type1; // Получаем название типа
                            }
                        }

                        // Создаем анонимный объект с нужными данными и добавляем его в список
                        usersWithType.Add(new
                        {
                            UserID = user.userID,
                            Fio = user.fio,
                            Phone = user.phone,
                            Type = typeName
                        });
                    }
                }
                UsersDataGrid.ItemsSource = usersWithType;
            }
        }





        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void EditUser(object sender, RoutedEventArgs e)
        {
            var selectedUser = UsersDataGrid.SelectedItem as dynamic;
            if (selectedUser != null)
            {
                int userId = selectedUser.UserID;

                using (var context = new AppDbContext())
                {
                    var user = context.users.FirstOrDefault(u => u.userID == userId);
                    if (user != null)
                    {
                        var editUserWindow = new EditUserWindow(user);
                        if (editUserWindow.ShowDialog() == true)
                        {
                            LoadUsers(); // Перезагрузить пользователей после изменения
                        }
                    }
                }
            }
        }

    }
}
