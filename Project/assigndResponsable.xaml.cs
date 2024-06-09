using Project.Data;
using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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

namespace Project
{
    public partial class assigndResponsable : Window
    {
        private dynamic selectedRequest;
        private int requestId;
        private int masterId;


        private void DisplayAlert(string title, string message, string buttonText)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        public assigndResponsable(dynamic request, int requestId, int masterId)
        {
            InitializeComponent();
            selectedRequest = request;
            this.requestId = requestId;
            this.masterId = masterId;
            LoadData();
        }

        private void LoadData()
        {
            // Получаем доступ к базе данных
            using (var context = new AppDbContext())
            {
                StatusComboBox.Items.Clear();
                StatusComboBox.ItemsSource = context.status.ToList();
                StatusComboBox.DisplayMemberPath = "requestStatus";
                StatusComboBox.SelectedValuePath = "id";

                CommentComboBox.Items.Clear();
                CommentComboBox.ItemsSource = context.comment.ToList();
                CommentComboBox.DisplayMemberPath = "message";
                CommentComboBox.SelectedValuePath = "commentID";
            }
        }


        private void SaveChangesButton_Click(object sender, RoutedEventArgs e)
        {

            var replacedPartsTextBox = ReplacedPartsTextBox.Text;
            var statusId = (int)StatusComboBox.SelectedValue;
            var commentId = (int)CommentComboBox.SelectedValue;

            if (string.IsNullOrEmpty(replacedPartsTextBox))
            {
                MessageBox.Show("Ошибка", "Заполните все поля", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (var context = new AppDbContext())
            {
                using (var context2 = new AppDbContext2())
                {
                    var existingRequestStatus = context2.requestStatus.FirstOrDefault(rs => rs.requestID == requestId);
                    if (existingRequestStatus != null)
                    {
                        // Удаляем существующую запись
                        context2.requestStatus.Remove(existingRequestStatus);
                    }

                    // Создаем новую запись
                    var newRequestStatus = new requestStatus
                    {
                        requestID = requestId,
                        statusID = statusId,
                    };
                    context2.requestStatus.Add(newRequestStatus);

                    context2.SaveChanges();
                }

                var commentMasterClient = context.commentMasterClient.FirstOrDefault(rs => rs.requestID == requestId);
                /*string requestDetails = $"Comment ID: {commentId}, Request ID: {requestId}, Master ID: {masterId}";
                MessageBox.Show(requestDetails);*/
                var request = context.request.FirstOrDefault(rs => rs.requestID == requestId);
                if (request != null)
                {
                    request.repairParts = replacedPartsTextBox;
                }

                if (commentMasterClient != null)
                {
                    // Удаляем существующую запись
                    context.commentMasterClient.Remove(commentMasterClient);
                }
                // Создаем новую запись
                var newCommentMasterClient = new commentMasterClient
                {
                    commentID = commentId,
                    requestID = requestId,
                    masterID = masterId,
                };
                context.commentMasterClient.Add(newCommentMasterClient);

                context.SaveChanges();
            }

            this.Close();
        }



    }
}
