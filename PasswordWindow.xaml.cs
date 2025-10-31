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
using System.Windows.Shapes;

namespace OKiosk
{
    /// <summary>
    /// Interaction logic for PasswordWindow.xaml
    /// </summary>
    public partial class PasswordWindow : Window
    {
        public bool IsAuthenticated { get; private set; } = false;
        private const string AdminPassword = "adm135!"; // burayı istersen settings.json'a taşıyabiliriz

        public PasswordWindow()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            if (PasswordBox.Password == AdminPassword)
            {
                IsAuthenticated = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Hatalı şifre.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
