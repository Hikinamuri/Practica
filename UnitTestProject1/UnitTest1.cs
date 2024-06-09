using Microsoft.VisualStudio.TestTools.UnitTesting;
using Project;
using System;

namespace UnitTestProject2
{
    [TestClass]
    public class MainWindowTests
    {
        [TestMethod]
        public void OnLoginClicked_EmptyUserName_DisplaysError()
        {
            // Arrange
            var mainWindow = new MainWindow();
            mainWindow.UserName.Text = "";
            mainWindow.PasswordBox.Password = "PasswordBox";

            // Act
            mainWindow.OnLoginClicked(null, null);

            // Assert
            Assert.IsTrue(mainWindow.DisplayedAlert);
        }

        [TestMethod]
        public void OnLoginClicked_InvalidUserName_DisplaysError()
        {
            // Arrange
            var mainWindow = new MainWindow();
            mainWindow.UserName.Text = "user@name";
            mainWindow.PasswordBox.Password = "PasswordBox";

            // Act
            mainWindow.OnLoginClicked(null, null);

            // Assert
            Assert.IsTrue(mainWindow.DisplayedAlert);
        }

        [TestMethod]
        public void OnLoginClicked_ValidCredentials_UserPageOpens()
        {
            var mainWindow = new MainWindow();
            mainWindow.UserName.Text = "LoginBox";
            mainWindow.PasswordBox.Password = "";

            // Act
            mainWindow.OnLoginClicked(null, null);

            // Assert
            Assert.IsTrue(mainWindow.DisplayedAlert);
        }

        [TestMethod]
        public void OnLoginClicked_InvalidCredentials_DisplaysError()
        {
            // Arrange
            var mainWindow = new MainWindow();
            mainWindow.UserName.Text = "";
            mainWindow.PasswordBox.Password = "";

            // Act
            mainWindow.OnLoginClicked(null, null);

            // Assert
            Assert.IsTrue(mainWindow.DisplayedAlert);
        }

        [TestMethod]
        public void OnLoginClicked_NonAlphanumericCredentials_DisplaysError()
        {
            // Arrange
            var mainWindow = new MainWindow();
            mainWindow.UserName.Text = "!@#$%^&*()";
            mainWindow.PasswordBox.Password = "!@#$%^&*()";

            // Act
            mainWindow.OnLoginClicked(null, null);

            // Assert
            Assert.IsTrue(mainWindow.DisplayedAlert);
        }
    }
}
