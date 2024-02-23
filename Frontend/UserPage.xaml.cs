using System.Net.Http;
using System.Windows;
using System.Windows.Controls;


namespace Frontend
{

    public partial class UserPage : Page
    {
        HttpClient client = App.client;
        private Guid userUid;
        public UserPage()
        {
            InitializeComponent();
        }



        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            var email = fieldEmail.Text;
            var password = fieldPassword.Text;

            userUid = await Login(email, password);
            if (userUid == Guid.Empty) { return; }

            Window mainWindow = Window.GetWindow(this);
            if (mainWindow != null && mainWindow is MainWindow)
            {
                GamePage gamePage = new GamePage(userUid);
                ((MainWindow)mainWindow).GamePage = gamePage;

                ((MainWindow)mainWindow).MainFrame.NavigationService.Navigate(((MainWindow)mainWindow).GamePage);
            }
        }
        private async void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            var email = fieldEmail.Text;
            var password = fieldPassword.Text;

            userUid = await Register(email, password);
            if (userUid == Guid.Empty) { return; }

            Window mainWindow = Window.GetWindow(this);
            if (mainWindow != null && mainWindow is MainWindow)
            {
                GamePage gamePage = new GamePage(userUid);
                ((MainWindow)mainWindow).GamePage = gamePage;

                ((MainWindow)mainWindow).MainFrame.NavigationService.Navigate(((MainWindow)mainWindow).GamePage);
            }
        }

        public async Task<Guid> Register(string email, string password)
        {

            var response = await client.PostAsync($"User/Register?email={email}&password={password}", null);
            var responseGuid = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode) 
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                statusUI.Text = errorMessage; 
            }
            if (Guid.TryParse(responseGuid.Trim('\"'), out Guid uid))
            {
                return uid;
            }
            else
            {
                return Guid.Empty;
            }
        }
        
        public async Task<Guid> Login(string email, string password)
        {
            var response = await client.PostAsync($"User/Login?email={email}&password={password}", null);
            var responseGuid = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode) { statusUI.Text = response.StatusCode.ToString(); }
            if (Guid.TryParse(responseGuid.Trim('\"'), out Guid uid))
            {
                return uid;
            }
            else
            {
                return Guid.Empty;
            }

        }

        
    }
}
