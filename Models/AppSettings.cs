using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OKiosk.Models
{
    public class AppSettings
    {
        public string HomeUrl { get; set; } = "https://www.google.com";
        public List<string> AllowedOrigins { get; set; } = new List<string> { "https://www.google.com" };
        public string CustomJs { get; set; } = string.Empty;
        public string CustomCss { get; set; } = string.Empty;
    }
}
