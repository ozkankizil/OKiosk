using OKiosk.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OKiosk.Services
{
    public static class SettingsService
    {
        private static string FilePath;

        public static AppSettings Load()
        {
            string basePath = AppContext.BaseDirectory;
            string runtimePath = Path.Combine(basePath, "settings.json");

            // Eğer bin klasöründe yoksa proje kökündeki dosyayı dene
            string? devPath = Directory.GetParent(basePath)?.Parent?.Parent?.FullName;
            string devFile = devPath != null ? Path.Combine(devPath, "settings.json") : runtimePath;

            // Öncelik sırası: bin > proje kökü
            FilePath = File.Exists(runtimePath) ? runtimePath : devFile;

            // Dosya yoksa oluştur
            if (!File.Exists(FilePath))
            {
                var defaults = new AppSettings();
                Save(defaults);
                return defaults;
            }

            // Dosyayı oku
            string json = File.ReadAllText(FilePath);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var settings = JsonSerializer.Deserialize<AppSettings>(json, options);

            if (settings == null)
            {
                settings = new AppSettings();
                Save(settings);
            }

            return settings;
        }

        public static void Save(AppSettings settings)
        {
            string basePath = AppContext.BaseDirectory;
            string runtimeFile = Path.Combine(basePath, "settings.json");

            string json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });

            // 1) Çalışma dizinine yaz
            File.WriteAllText(runtimeFile, json);

            // 2) Proje köküne da yaz (OKiosk/settings.json)
            string? projectRoot = Directory.GetParent(basePath)?.Parent?.Parent?.Parent?.FullName; // ↑ 3 parent
            if (!string.IsNullOrEmpty(projectRoot))
            {
                string devFile = Path.Combine(projectRoot, "settings.json");
                try { File.WriteAllText(devFile, json); } catch { /* yayın ortamında yok olabilir, yut */ }
            }
        }
    }
}
