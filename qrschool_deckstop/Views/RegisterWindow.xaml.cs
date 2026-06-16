using System;
using System.Windows;
using qrschool_deckstop.Models;
using qrschool_deckstop.Services;
using qrschool_deckstop.Helpers;

namespace qrschool_deckstop.Views
{
    public partial class RegisterWindow : Window
    {
        private readonly AuthenticationService _authService;
        private readonly UserService _userService;

        public RegisterWindow()
        {
            InitializeComponent();
            _authService = new AuthenticationService();
            _userService = new UserService();
        }

        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            var username = txtUsername.Text.Trim();
            var password = txtPassword.Password;
            var fullName = txtFullName.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                txtError.Text = "Введите логин и пароль";
                return;
            }

            if (_userService.GetByUsername(username) != null)
            {
                txtError.Text = "Пользователь с таким логином уже существует";
                return;
            }

            var user = new User
            {
                Username = username,
                PasswordHash = PasswordHelper.HashPassword(password),
                FullName = fullName,
                Role = "tech",
                CreatedAt = DateTime.Now
            };

            try
            {
                _userService.Create(user);
                MessageBox.Show("Пользователь создан", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            catch (Exception ex)
            {
                txtError.Text = "Ошибка создания пользователя: " + ex.Message;
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}