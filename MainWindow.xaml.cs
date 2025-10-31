using Microsoft.Web.WebView2.Core;
using OKiosk.Models;
using OKiosk.Services;
using System.Runtime;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OKiosk
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private AppSettings _settings;
        public MainWindow()
        {
            InitializeComponent();
            _settings = SettingsService.Load();
            InitializeWebView();
        }
        private async void InitializeWebView()
        {
            await webView.EnsureCoreWebView2Async();
            webView.CoreWebView2.NavigationStarting += (s, e) =>
            {
                try
                {
                    var targetUri = new Uri(e.Uri);
                    var targetHost = targetUri.Host.ToLowerInvariant();

                    bool isAllowed = _settings.AllowedOrigins.Any(origin =>
                    {
                        try
                        {
                            origin = origin.Trim().ToLowerInvariant();

                            // Jokerli alan adı desteği (*.domain.com)
                            if (origin.StartsWith("*.")) // örn: *.google.com
                            {
                                var domain = origin[2..]; // "google.com"
                                return targetHost == domain || targetHost.EndsWith("." + domain);
                            }

                            // Normal domain kontrolü
                            var allowedHost = new Uri(origin).Host.ToLowerInvariant();
                            return targetHost == allowedHost || targetHost.EndsWith("." + allowedHost);
                        }
                        catch
                        {
                            return false;
                        }
                    });

                    if (!isAllowed)
                    {
                        e.Cancel = true;
                        webView.CoreWebView2.Navigate(_settings.HomeUrl);
                    }
                }
                catch
                {
                    e.Cancel = true;
                    webView.CoreWebView2.Navigate(_settings.HomeUrl);
                }
            };


            // Sayfayı aç
            webView.Source = new Uri(_settings.HomeUrl);

            // Yeni sekme açmayı engelle
            webView.CoreWebView2.NewWindowRequested += (s, e) =>
            {
                e.Handled = true;
                webView.CoreWebView2.Navigate(e.Uri);
            };

            // Sağ tıklamayı engelle
            webView.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;

            // Sayfa yüklendiğinde Custom JS çalıştır
            webView.CoreWebView2.NavigationCompleted += WebView_NavigationCompleted;
        }

        private async void WebView_NavigationCompleted(object? sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(_settings.CustomJs))
            {
                try
                {
                    await webView.ExecuteScriptAsync(_settings.CustomJs);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"JS hatası: {ex.Message}");
                }
            }
            if (!string.IsNullOrWhiteSpace(_settings.CustomCss))
            {
                try
                {
                    string cssInject = $@"
                        (function() {{
                            let style = document.getElementById('custom-kiosk-style');
                            if (!style) {{
                                style = document.createElement('style');
                                style.id = 'custom-kiosk-style';
                                document.head.appendChild(style);
                            }}
                            style.textContent = `{_settings.CustomCss}`;
                        }})();
                    ";
                    await webView.ExecuteScriptAsync(cssInject);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"CSS hatası: {ex.Message}");
                }
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && e.Key == Key.F1)
            {
                var pwdWindow = new PasswordWindow();
                pwdWindow.Owner = this;
                pwdWindow.ShowDialog();

                if (pwdWindow.IsAuthenticated)
                {
                    var adminWin = new AdminWindow(_settings, ApplySettings);
                    adminWin.Owner = this;
                    adminWin.ShowDialog();
                }
            }
        }

        private async void ApplySettings(AppSettings newSettings)
        {
            _settings = newSettings;

            try
            {
                // Yeni sayfaya git
                webView.CoreWebView2.Navigate(_settings.HomeUrl);

                // Sayfa yüklenince yeni JS'i uygula
                webView.CoreWebView2.NavigationCompleted += async (s, e) =>
                {
                    if (!string.IsNullOrWhiteSpace(_settings.CustomJs))
                        await webView.ExecuteScriptAsync(_settings.CustomJs);
                    if (!string.IsNullOrWhiteSpace(_settings.CustomCss))
                        await webView.ExecuteScriptAsync(_settings.CustomCss);
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ayarlar uygulanırken hata: {ex.Message}");
            }
        }

    }
}