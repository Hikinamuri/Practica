using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Project.Data;
using Project.Models;
using ZXing;
using ZXing.QrCode;
using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;
using System.Drawing.Imaging;

namespace Project
{
    public partial class UserPage : Window
    {
        public UserPage()
        {
            InitializeComponent();
        }
        private void DisplayAlert(string title, string message, string buttonText)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Warning);
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
        private void EquipmentType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem selectedItem = (ComboBoxItem)EquipmentType.SelectedItem;
            string selectedEquipmentType = selectedItem.Content.ToString();

            using (var context = new AppDbContext())
            {
                var models = context.homeTech
                                  .Where(ht => ht.homeTechType == selectedEquipmentType)
                                  .Select(ht => ht.homeTechModel)
                                  .ToList();

                DeviceType.Items.Clear();

                foreach (var model in models)
                {
                    DeviceType.Items.Add(new ComboBoxItem { Content = model });
                }
            }
        }


        private void SendOrder(object sender, RoutedEventArgs e)
        {
            var dateRequest = DateTime.Now;
            var equipmentType = EquipmentType.Text;
            var deviceType = DeviceType.Text;
            var issueDescription = IssueDescription.Text;

            if (string.IsNullOrEmpty(equipmentType) ||
                string.IsNullOrEmpty(deviceType) ||
                string.IsNullOrEmpty(issueDescription))
            {
                DisplayAlert("Ошибка", "Пожалуйста, заполните все поля", "OK");
            }
            else
            {
                using (var context = new AppDbContext())
                {
                    var maxRequestId = context.request.Max(u => u.requestID);
                    var newRequestId = maxRequestId + 1;

                    var selectedModel = (ComboBoxItem)DeviceType.SelectedItem;
                    string selectedModelName = selectedModel.Content.ToString();

                    var selectedHomeTech = context.homeTech.FirstOrDefault(ht => ht.homeTechModel == selectedModelName);



                    if (selectedHomeTech != null)
                    {
                        var newRequest = new request
                        {
                            requestID = newRequestId,
                            startDate = dateRequest,
                            problemDescryption = issueDescription,
                            completionDate = null,
                            repairParts = null
                        };
                        var newHomeTechId = selectedHomeTech.id;
                        var newRequestHomeTech = new requestHomeTech
                        {
                            requestID = newRequestId,
                            homeTechID = newHomeTechId
                        };
                        context.request.Add(newRequest);
                        context.requestHomeTech.Add(newRequestHomeTech);
                        context.SaveChanges();

                        DisplayAlert("Успех!", $"Заявка создана, ваш ID - {newRequestId}", "OK");

                    }
                    else
                    {
                        DisplayAlert("Ошибка", "Выбранная модель оборудования не найдена", "OK");
                    }
                }
            }
        }
        private void CheckRequest(object sender, RoutedEventArgs e)
        {
            var requestIdtext = OrderID.Text;
            
            if (string.IsNullOrEmpty(requestIdtext))
            {
                DisplayAlert("Ошибка", "Пожалуйста, заполните все поля", "OK");
                return;
            }
            if (!IsAlphaNumeric(requestIdtext))
            {
                DisplayAlert("Ошибка", "Пожалуйста, вводите только буквы и цифры", "OK");
                return;
            }
            var requestId = Int32.Parse(requestIdtext);

            using (var context2 = new AppDbContext2())
            {
                var requestStatus = context2.requestStatus.FirstOrDefault(r => r.requestID == requestId);

                if (requestStatus != null)
                {
                    var statusId = requestStatus.statusID;

                    using (var context = new AppDbContext())
                    {
                        var status = context.status.FirstOrDefault(r => r.id == statusId);
                        var requestHomeTech = context.requestHomeTech.FirstOrDefault(r => r.requestID == requestId);
                        var commentMasterClient = context.commentMasterClient.FirstOrDefault(r => r.requestID == requestId);

                        if (commentMasterClient != null)
                        {
                            var commentId = commentMasterClient.commentID;
                            var masterId = commentMasterClient.masterID;
                            var homeTechID = requestHomeTech.homeTechID;

                            var master = context.users.FirstOrDefault(u => u.userID == masterId);
                            var homeTech = context.homeTech.FirstOrDefault(u => u.id == homeTechID);

                            if (master != null && homeTech != null)
                            {
                                var comment = context.comment.FirstOrDefault(r => r.commentID == commentId);

                                if (comment != null)
                                {
                                    MasterTextBox.Text = $"{master.fio}";
                                    CommentTextBox.Text = $"{comment.message}";
                                    HomeTechTextBox.Text = $"Тип: {homeTech.homeTechType}, Модель: {homeTech.homeTechModel}";
                                }
                                else
                                {
                                    CommentTextBox.Text = "Комментарий не найден.";
                                }
                            }
                            else
                            {
                                MasterTextBox.Text = "Мастер не найден.";
                            }
                        }
                        else
                        {
                            CommentTextBox.Text = "Комментарий не найден для данного запроса.";
                        }

                        if (status != null && status.requestStatus == "Готова к выдаче")
                        {
                            OrderStatus.Text = $"{status.requestStatus}";
                            var qrCodeValue = "https://docs.google.com/forms/d/e/1FAIpQLSdhZcExx6LSIXxk0ub55mSu-WIh23WYdGG9HY5EZhLDo7P8eA/viewform?usp=sf_link";
                            var qrCodeBitmap = GenerateQRCode(qrCodeValue);
                            QRCodeImage.Source = qrCodeBitmap;
                        }

                        else
                        {
                            OrderStatus.Text = "Статус не найден.";
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Запрос не найден.");
                }
            }
        }


        private BitmapImage GenerateQRCode(string qrCodeValue)
        {
            var writer = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions
                {
                    Width = 200,
                    Height = 200,
                    Margin = 0
                }
            };

            var bitmap = writer.Write(qrCodeValue);

            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                var imageSource = new BitmapImage();
                imageSource.BeginInit();
                imageSource.CacheOption = BitmapCacheOption.OnLoad;
                imageSource.StreamSource = memory;
                imageSource.EndInit();

                return imageSource;
            }
        }



    }
}
