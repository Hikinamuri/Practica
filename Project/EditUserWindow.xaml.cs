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
    public partial class EditUserWindow : Window
    {
        private users _user;

        public EditUserWindow(users user)
        {
            InitializeComponent();
            _user = user;
            LoadUserTypes();
        }

        private void LoadUserTypes()
        {
            using (var context = new AppDbContext())
            {
                using (var context2 = new AppDbContext2())
                {
                    UserTypeComboBox.ItemsSource = context.type.ToList();
                    UserTypeComboBox.DisplayMemberPath = "type1";
                    UserTypeComboBox.SelectedValuePath = "id";
                    var userType = context2.userType.FirstOrDefault(ut => ut.userID == _user.userID);
                    if (userType != null)
                    {
                        // Проверяем, что значение не null, прежде чем устанавливать его
                        if (UserTypeComboBox.SelectedValue != null)
                        {
                            UserTypeComboBox.SelectedValue = userType.typeID;
                        }
                    }
                }
            }
        }


        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Проверка на null
                if (UserTypeComboBox.SelectedValue != null)
                {
                    // Получение выбранного значения, только если оно не null
                    var selectedTypeId = (int)UserTypeComboBox.SelectedValue;

                    using (var context = new AppDbContext2())
                    {
                        var userType = context.userType.FirstOrDefault(ut => ut.userID == _user.userID);
                        if (userType != null)
                        {
                            userType.typeID = selectedTypeId;
                        }
                        else
                        {
                            context.userType.Add(new userType
                            {
                                userID = _user.userID,
                                typeID = selectedTypeId
                            });
                        }
                        context.SaveChanges();
                    }
                    this.DialogResult = true;
                    this.Close();
                }
            }
            catch (InvalidOperationException ex)
            {
                // Обработка ошибки, если попытка изменения ключевого свойства вызвала исключение
                MessageBox.Show("Ошибка при попытке изменить тип пользователя: " + ex.Message);
            }
            catch (Exception ex)
            {
                // Обработка других исключений, если таковые возникнут
                MessageBox.Show("Произошла ошибка: " + ex.Message);
            }
        }


    }
}