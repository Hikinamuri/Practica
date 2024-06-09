using Project.Data;
using Project.Models;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Project
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public TextBox UserName => userName;
        public PasswordBox PasswordBox => Password;
        public bool DisplayedAlert { get; set; }
        public bool UserPageOpened { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        public bool IsAlphaNumeric(string input)
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

        public void DisplayAlert(string title, string message, string buttonText)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        public void Password_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Password.Password == "")
            {
                Password.Password = "";
                Password.Foreground = Brushes.Black;
            }
        }

        public void Password_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Password.Password))
            {
                Password.Password = "Пароль";
                Password.Foreground = Brushes.Gray;
            }
        }
        public void OnLoginTapped(object sender, MouseButtonEventArgs e)
        {
            // Обработка события щелчка мыши
        }
        public void OnRadioButtonCheckedChanged(object sender, RoutedEventArgs e)
        {
            // Ваш код обработки события здесь
        }
        public string Login { get; set; }
        public void OnForgotTapped(object sender, MouseButtonEventArgs e)
        {
            // Ваш код обработки события здесь
        }

        public void OnLoginClicked(object sender, RoutedEventArgs e)
        {
            string username = UserName.Text;
            string password = PasswordBox.Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                DisplayedAlert = true; // Установите свойство DisplayedAlert в true
                DisplayAlert("Ошибка", "Пожалуйста, заполните все поля", "OK");
                return;
            }

            if (!IsAlphaNumeric(username) || !IsAlphaNumeric(password))
            {
                DisplayedAlert = true;
                DisplayAlert("Ошибка", "Имя пользователя и пароль должны содержать только буквы и цифры", "OK");
                return;
            }
            else
            {
                using (var context = new AppDbContext())
                {
                    var user = context.autoriz.SingleOrDefault(u => u.login == username && u.password == password);
                    if (user != null)
                    {
                        using (var context2 = new AppDbContext2())
                        {
                            var userType = context2.userType.FirstOrDefault(u => u.userID == user.userID);
                            if (userType != null)
                            {
                                if (userType.typeID == 1)
                                {
                                    var userPage = new UserPage();
                                    userPage.Show();
                                    this.Close();
                                }
                                else if (userType.typeID == 2)
                                {
                                    var orderChange = new orderChange(user.userID);
                                    orderChange.Show();
                                    this.Close();
                                }
                                else if (userType.typeID == 3)
                                {
                                    var managerPage = new ManagerPage();
                                    managerPage.Show();
                                    this.Close();
                                }
                                else if (userType.typeID == 5)
                                {
                                    var admin = new AdminPanel();
                                    admin.Show();
                                    this.Close();
                                }
                            }
                        }

                    }
                    else
                    {
                        DisplayAlert("Ошибка", "Неверное имя пользователя или пароль", "OK");
                    }
                }
            }
        }

        public void Log_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        public void Label_Click(object sender, RoutedEventArgs e)
        {
            var registration = new Registration();

            registration.Show();
            this.Close();
        }
    }

}
