using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SalimgareevaShoes
{
    /// <summary>
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        public AuthPage()
        {
            InitializeComponent();
        }

        private void AuthButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(LoginTB.Text) || string.IsNullOrWhiteSpace(PasswordTB.Text))
            {
                MessageBox.Show("Введите логин и пароль");
                return;
            }
            Users user = SalimgarevaShoesEntities.GetContext().Users.Where(u => u.UserLogin.Equals(LoginTB.Text) && u.UserPassword.Equals(PasswordTB.Text)).FirstOrDefault();
            if (user != null)
            {
                if (user.UserRoleID == 1)
                {
                    Manager.MainFrame.Navigate(new AdminShoesPage(user));
                    return;
                }
                Manager.MainFrame.Navigate(new ShoesPage(user));
                return;

            }
            MessageBox.Show("Неверный логин или пароль");
        }

        private void LoginAsGuestButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new ShoesPage(null));
        }
    }
}
