using Project.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Project
{
    public partial class SelectMasterWindow : Window
    {
        public int SelectedRequestId { get; set; }
        public int SelectedMasterId { get; private set; } // Nullable тип

        public SelectMasterWindow(int requestId)
        {
            InitializeComponent();
            SelectedRequestId = requestId;
            LoadMasters();
            CompletionDatePicker.SelectedDate = DateTime.Now;
        }

        private void LoadMasters()
        {
            using (var context = new AppDbContext())
            {
                var users = context.users.ToList();
                var userTypeIds = users.Select(u => u.userID).ToList();

                using (var context2 = new AppDbContext2())
                {
                    var userTypes = context2.userType
                        .Where(ut => ut.typeID == 2 && userTypeIds.Contains(ut.userID))
                        .Select(ut => ut.userID)
                        .ToList();

                    var masters = users.Where(u => userTypes.Contains(u.userID)).ToList();

                    // Добавляем "Убрать мастера" в начало списка
                    masters.Insert(0, new Project.Models.users { userID = -1, fio = "Убрать мастера" });

                    MasterComboBox.ItemsSource = masters;
                    MasterComboBox.DisplayMemberPath = "fio";
                    MasterComboBox.SelectedValuePath = "userID";
                }
            }
        }


        private void AssignMasterButton_Click(object sender, RoutedEventArgs e)
        {
            if (MasterComboBox.SelectedValue != null && CompletionDatePicker.SelectedDate != null)
            {
                SelectedMasterId = (int)MasterComboBox.SelectedValue;
                DateTime completionDate = CompletionDatePicker.SelectedDate.Value;

                using (var context = new AppDbContext())
                {
                    var request = context.request.FirstOrDefault(r => r.requestID == SelectedRequestId);
                    if (request != null)
                    {
                        request.completionDate = completionDate;
                        context.SaveChanges();
                    }
                }

                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите мастера и укажите время завершения", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
