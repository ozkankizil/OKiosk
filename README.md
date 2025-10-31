🖥️ OKiosk — .NET 8 WPF Kiosk Tarayıcı

OKiosk, .NET 8 ve WPF tabanlı, tam ekran çalışan, kullanıcı etkileşimini sınırlandırılmış bir kiosk tarayıcı uygulamasıdır.
Kütüphane, müze kioskları veya tanıtım ekranları gibi durumlarda güvenli ve kontrol edilebilir web içeriği göstermek için geliştirilmiştir.

🚀 Özellikler
Özellik	Açıklama
🔒 Tam ekran mod	Kapatma / küçültme düğmeleri gizlenir.
🚫 Yeni sekme engelleme	target="_blank" veya window.open() çağrıları aynı sekmede açılır.
🌐 Domain kısıtlama	Yalnızca izin verilen alan adlarında gezilebilir (ör. google.com, bing.com).
⚙️ Admin Paneli (Ctrl + F1)	Şifre korumalı yönetici menüsü ile ayarlar değiştirilebilir.
🧭 Ayar dosyası (settings.json)	Açılış sayfası, izin verilen siteler, özel JS ve CSS kodları kaydedilir.
💻 Anında güncelleme	Admin panelinden yapılan değişiklikler anında uygulanır, uygulama yeniden başlatılmaz.
🧩 Custom JS & CSS desteği	Sayfa yüklenince özel JavaScript veya CSS kodları otomatik uygulanır.
🧠 Kalıcı ayarlar	Kapatıp açıldığında son kaydedilen ayarlarla devam eder.
⚙️ Ayarlar (settings.json)

Uygulama çalıştırıldığında bu dosya otomatik olarak oluşturulur.
{
  "HomeUrl": "https://www.google.com",
  "AllowedOrigins": [
    "https://www.google.com",
    "*.google.com"
  ],
  "CustomJs": "alert('Kiosk uygulaması başladı!');",
  "CustomCss": "body { background-color: black; }"
}

Alan	Açıklama
HomeUrl:	Uygulamanın açılışta gideceği varsayılan sayfa
AllowedOrigins:	Gezilebilecek domain listesi. Joker (*.domain.com) desteği vardır.
CustomJs:	Sayfa yüklenince çalışacak JavaScript kodu
CustomCss:	Sayfa yüklenince uygulanacak CSS kodu

🧑‍💼 Yönetici Paneli
Açmak için: Ctrl + F1 tuşlarına bas
Varsayılan şifre: admin123
Yetkiler:
Açılış sayfasını değiştirme
JS / CSS kodlarını düzenleme
İzin verilen domain listesini yönetme

🏗️ Proje Yapısı
OKiosk/
 ├─ App.xaml / App.xaml.cs
 ├─ MainWindow.xaml / MainWindow.xaml.cs
 ├─ AdminWindow.xaml / AdminWindow.xaml.cs
 ├─ PasswordWindow.xaml / PasswordWindow.xaml.cs
 ├─ Models/
 │   └─ AppSettings.cs
 ├─ Services/
 │   └─ SettingsService.cs
 ├─ settings.json
 ├─ OKiosk.csproj
 └─ README.md

🧩 Geliştirici Notları
Framework: .NET 8.0
UI: WPF (XAML)
Web motoru: Microsoft WebView2
Derleme: Visual Studio 2022 veya dotnet build

⚠️ Güvenlik
Admin şifresi düz metin olarak kodda tutulur (PasswordWindow.cs).
→ İleri seviyede AES ile şifrelenmiş biçimde settings.json içine taşınabilir.
Yeni domainlere gitme, yeni pencere açma veya sağ tıklama menüsü devre dışıdır.
Gerekirse Alt+Tab, Ctrl+Alt+Del gibi tuşlar da sistem düzeyinde engellenebilir (ek kodla).

Uygulama tek başına çalışır, .NET runtime gerekmez.


