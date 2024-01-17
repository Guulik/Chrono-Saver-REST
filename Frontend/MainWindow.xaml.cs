using System.Net;
using System.Net.Http;
using System.Text;
using System.Windows;
using Newtonsoft.Json;

namespace Frontend
{
    public partial class MainWindow : Window
    {
        public SavePage SavePage { get; set; }
        public GamePage GamePage { get; set; }
        public UserPage UserPage { get; set;  }
        public MainWindow()
        {
            InitializeComponent();
            UserPage = new UserPage();
            MainFrame.NavigationService.Navigate(UserPage);
        }
        
    }
}