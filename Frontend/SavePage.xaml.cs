using System.Net.Http;
using System.Text;
using System.Windows;
using Newtonsoft.Json;
using System.Windows.Controls;

namespace Frontend
{
    public partial class SavePage : Page
    {
        HttpClient client = App.client;
        FileHandler fileHandler = new FileHandler();
        Save? selectedSave;
        private string? SavePath = string.Empty;
        public SavePage(string? savePath = @"O:\Desktop")
        {
            InitializeComponent();
            btnLoad.IsEnabled = false;
            btnDelete.IsEnabled = false;
            SavePath = savePath;
            DisplaySaves();
        }

        public async Task<List<Save>> GetAllSaves()
        {
            var gameUid = App.GameUid; //настроить выбор игры

            var response = await client.GetAsync($"Save/GetSaves?gameId={gameUid}");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                if (jsonString == null) return null;

                List<Save> saves = JsonConvert.DeserializeObject<List<Save>>(jsonString);

                return saves;
            }
            else return null;
        }

        public async void DisplaySaves()
        {
            List<Save> saves = await GetAllSaves();
            saveListView.ItemsSource = saves;
        }


        private async void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            var saveUid = selectedSave.Uid;
            //var saveUid = Guid.Parse("C52AF27D-4DFA-463C-BDA1-C0A4642E7018");

            Save save = await GetSave(saveUid);

            try
            {
                fileHandler.DownloadFile(save.SaveData, SavePath, save.SaveName);

            }
            catch (Exception ex)
            {
                txtb_response.Text = ex.Message;
            }

            
        }

        private async Task<Save> GetSave(Guid saveUid)
        {
            var response = await client.GetAsync($"Save/GetSave?saveId={saveUid}");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var save = JsonConvert.DeserializeObject<Save>(jsonString);
                return save;
            }
            else return null;
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            //тестовые значения
            //var savePath = (@"C:\Users\sasha\AppData\Roaming\DarkSoulsII\011000010af8a98c");
            //var savePath = (@"P:\Steam\steamapps\common\Dead Cells\save");


            byte[] saveData = fileHandler.ReadFile(SavePath);
            string saveName = fileHandler.ReadFileName(SavePath);

            if (saveData == null)
            { 
                return;
            }
            await UploadSave(App.GameUid, saveName, saveData);

            DisplaySaves();
        }

        private async Task UploadSave(Guid gameUid, string saveName, byte[] saveData)
        {
            string jsonData = JsonConvert.SerializeObject(saveData);

            var httpContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"Save/AddSave?gameId={gameUid}&saveName={saveName}", httpContent);

            txtb_response.Text = response.StatusCode.ToString();
        }

        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var saveId = selectedSave.Uid;
            await DeleteSave(saveId);
            saveListView.SelectedItem = null;
            DisplaySaves();
        }

        private async Task DeleteSave(Guid saveUid)
        {
            await client.DeleteAsync($"Save/RemoveSave?saveId={saveUid}");
        }

        private void saveListView_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (saveListView.SelectedItem != null)
            {
                selectedSave = (Save)saveListView.SelectedItem;
                btnLoad.IsEnabled = true;
                btnDelete.IsEnabled = true;
            }
            else
            {
                btnLoad.IsEnabled = false;
                btnDelete.IsEnabled = false;
            }
        }

        private void btnBack_Navigation_Click(object sender, RoutedEventArgs e)
        {
            Window mainWindow = Window.GetWindow(this);
            if (mainWindow != null && mainWindow is MainWindow)
            {
                ((MainWindow)mainWindow).MainFrame.NavigationService.Navigate(((MainWindow)mainWindow).GamePage);
            }
        }
    }

}