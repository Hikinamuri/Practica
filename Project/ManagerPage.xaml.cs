using Project.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Project
{
    public partial class ManagerPage : Window
    {
        public ManagerPage()
        {
            InitializeComponent();
            LoadRequests();
        }

        private void DisplayAlert(string title, string message, string buttonText)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void SendRequest(object sender, RoutedEventArgs e)
        {
            // Реализация функции отправки запроса
        }

        private void LoadRequests()
        {
            using (var context = new AppDbContext())
            {
                var usersWithType = new List<object>(); // Список для хранения данных пользователей с типами

                List<Project.Models.request> requests = context.request.ToList();
                foreach (var user in requests)
                {
                    var clientMaster = context.requestClientMaster.FirstOrDefault(u => u.requestID == user.requestID);
                    
                    string masterName = "Не назначен"; // Значение по умолчанию, если мастер не найден

                    if (clientMaster != null)
                    {
                        var master = context.users.FirstOrDefault(t => t.userID == clientMaster.masterID);
                        if (master != null)
                        {
                            masterName = master.fio;
                        }
                    }

                    // Создаем анонимный объект с нужными данными и добавляем его в список
                    usersWithType.Add(new
                    {
                        UserID = user.requestID,
                        ProblemDescription = user.problemDescryption,
                        CompletionDate = user.completionDate,
                        MasterName = masterName
                    });
                }
                OrderList.ItemsSource = usersWithType;
            }
        }

        private void Order_List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Реализация обработки выбора элемента списка
        }

        private void SendOrder(object sender, RoutedEventArgs e)
        {
            if (OrderList.SelectedItem == null)
            {
                DisplayAlert("Ошибка", "Пожалуйста, выберите запись", "OK");
                return;
            }

            dynamic selectedRequest = OrderList.SelectedItem;
            int requestId = selectedRequest.UserID;

            SelectMasterWindow selectMasterWindow = new SelectMasterWindow(requestId);
            if (selectMasterWindow.ShowDialog() == true)
            {
                int selectedMasterId = selectMasterWindow.SelectedMasterId;

                using (var context = new AppDbContext())
                {
                    var requestClientMaster = context.requestClientMaster.FirstOrDefault(r => r.requestID == requestId);
                    if (requestClientMaster != null)
                    {
                        requestClientMaster.masterID = selectedMasterId;
                        context.SaveChanges();
                        DisplayAlert("Успех", "Мастер назначен", "OK");
                        LoadRequests(); // Обновление списка заявок
                    }
                }
            }
        }
    }
}
