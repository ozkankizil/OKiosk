using OKiosk.Models;
using OKiosk.Services;
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
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        private AppSettings _settings;
        private readonly Action<AppSettings> _onApply;

        public AdminWindow(AppSettings currentSettings, Action<AppSettings> onApply)
        {
            InitializeComponent();
            _settings = currentSettings;
            _onApply = onApply;
            LoadSettings();
        }

        private void LoadSettings()
        {
            TxtHomeUrl.Text = _settings.HomeUrl;
            TxtCustomJs.Text = _settings.CustomJs;
            TxtCustomCss.Text = _settings.CustomCss;
            TxtAllowedOrigins.Text = string.Join("\n", _settings.AllowedOrigins);
        }

        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            _settings.HomeUrl = TxtHomeUrl.Text.Trim();
            _settings.CustomJs = TxtCustomJs.Text;
            _settings.CustomCss = TxtCustomCss.Text;

            // çok satırlı listeyi parçala
            var lines = TxtAllowedOrigins.Text
                .Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Distinct()
                .ToList();
            _settings.AllowedOrigins = lines;

            SettingsService.Save(_settings);
            _onApply(_settings);

            MessageBox.Show("Ayarlar kaydedildi ve uygulandı.", "Bilgi",
                MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

    }
}
