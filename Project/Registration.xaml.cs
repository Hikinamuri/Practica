using Project.Data;
using Project.Models;
using System.Linq;
using System.Windows;

namespace Project
{
    public partial class Registration : Window
    {
        public Registration()
        {
            InitializeComponent();
        }

        private bool IsAlphaNumeric(string input)
        {
            foreach (char c in input)
            {
                if (!char.IsLetterOrDigit(c))
                {
                    return false;
                }
            }
            return true;
        }

        private void DisplayAlert(string title, string message, string buttonText)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private bool IsNumeric(string input)
        {
            foreach (char c in input)
            {
                if (!char.IsDigit(c))
                {
                    return false;
                }
            }
            return true;
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var fullName = FullName.Text;
            var username = Username.Text;
            var password = Password.Password;
            var phoneNumber = PhoneNumber.Text;

            if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(phoneNumber)) 
            {
                DisplayAlert("Ошибка", "Пожалуйста, заполните все поля", "OK");
                return;
            }

            if (!IsAlphaNumeric(fullName) || !IsAlphaNumeric(username) || !IsAlphaNumeric(password))
            {
                DisplayAlert("Ошибка", "Имя пользователя и пароль должны содержать только буквы и цифры", "OK");
            }
            else if (!IsNumeric(phoneNumber))
            {
                DisplayAlert("Ошибка", "Номер телефона должен содержать только цифры", "OK");
            }
            else
            {
                using (var context = new AppDbContext())
                {
                    var autoriz = context.autoriz.SingleOrDefault(u => u.login == username);
                    if (autoriz == null)
                    {
                        var maxUserId = context.users.Max(u => u.userID);
                        var newUserId = maxUserId + 1;

                        var user = new users
                        {
                            userID = newUserId,
                            fio = fullName,
                            phone = phoneNumber
                        };

                        var auth = new autoriz
                        {
                            userID = newUserId,
                            login = username,
                            password = password,
                            users = user
                        };

                        context.users.Add(user);
                        context.autoriz.Add(auth);
                        context.SaveChanges();
                    }
                    else
                    {
                        DisplayAlert("Ошибка", "Логин уже существует", "OK");
                        return;
                    }
                    
                }
                
                DisplayAlert("Успех!", "Пользователь зарегистрирован!", "OK");
                var userPage = new UserPage();

                userPage.Show();
                this.Close();
            }
        }
    }
}
