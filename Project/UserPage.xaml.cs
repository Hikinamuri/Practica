using Project.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Project
{
    public partial class orderChange : Window
    {
        private int userID;
        public orderChange(int userID)
        {
            InitializeComponent();
            this.userID = userID;
            LoadData();
        }

        private void LoadData()
        {

            using (var context = new AppDbContext())
            {
                // Находим все requestID и ClientID для указанного userID
                var requests = context.requestClientMaster
                    .Where(rcm => rcm.masterID == userID)
                    .Select(rcm => new { rcm.requestID, rcm.clientID })
                    .ToList();

                // Создаем список для хранения данных
                var data = new List<object>();

                foreach (var request in requests)
                {
                    // Получаем данные из таблицы request для каждого requestID
                    var requestDetails = context.request.FirstOrDefault(r => r.requestID == request.requestID);
                    if (requestDetails != null)
                    {
                        // Получаем данные из таблицы users для каждого clientID
                        var user = context.users.FirstOrDefault(u => u.userID == request.clientID);
                        if (user != null)
                        {
                            // Добавляем данные в список
                            data.Add(new
                            {
                                RequestId = request.requestID,
                                ClientId = request.clientID,
                                StartDate = requestDetails.startDate,
                                ProblemDescription = requestDetails.problemDescryption,
                                CompletionDate = requestDetails.completionDate,
                                UserName = user.fio
                            });
                        }
                    }
                }

                // Устанавливаем источник данных для DataGrid
                OrderDataGrid.ItemsSource = data;
            }
        }

        // Главная форма
        private void SendRequest(object sender, RoutedEventArgs e)
        {
            if (OrderDataGrid.SelectedItem != null)
            {
                var selectedRequest = (dynamic)OrderDataGrid.SelectedItem;
                var requestId = selectedRequest.RequestId;

                var editForm = new assigndResponsable(selectedRequest, requestId, userID);
                editForm.ShowDialog();

                LoadData();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите запрос для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
