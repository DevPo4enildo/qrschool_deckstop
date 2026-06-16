using System.Windows;
using qrschool_deckstop.Services;

namespace qrschool_deckstop.Views
{
    public partial class LoginWindow : Window
    {
        private readonly AuthenticationService _authService;

        public LoginWindow()
        {
            InitializeComponent();
            _authService = new AuthenticationService();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            var username = txtUsername.Text.Trim();
            var password = txtPassword.Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                txtError.Text = "Введите логин и пароль";
                return;
            }

            if (_authService.Login(username, password))
            {
                if (_authService.IsAccountant)
                {
                    var accountantWindow = new AccountantInventoryWindow(_authService);
                    accountantWindow.Show();
                }
                else if (_authService.IsTech)
                {
                    var techWindow = new TechInventoryWindow(_authService);
                    techWindow.Show();
                }
                Close();
            }
            else
            {
                txtError.Text = "Неверный логин или пароль";
                txtPassword.Password = "";
            }
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            var register = new RegisterWindow();
            register.Owner = this;
            register.ShowDialog();
        }
    }
}
