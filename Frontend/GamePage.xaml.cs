using Microsoft.Win32;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace Frontend
{
    public partial class GamePage : Page
    {
        HttpClient client = App.client;
        Game selectedGame;
        Guid _userUid = Guid.Empty;
        OpenFileDialog openFileDialog;
        //string selectedGameSavePath = string.Empty;
        public GamePage(Guid userUid)
        {
            InitializeComponent();
            _userUid = userUid;
            if (App.GameUid == Guid.Empty)
            {
                btnInspect.IsEnabled = false;
                btnDelete.IsEnabled = false;
                btnEdit.IsEnabled = false;
            }
            openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Выберите файл";
            openFileDialog.Filter = "Все файлы (*.*)|*.*";
            DisplayGames();
        }
        

        private void gameListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (gameListView.SelectedItem != null)
            {
                selectedGame = (Game)gameListView.SelectedItem;
                App.GameUid = selectedGame.uid;
                btnInspect.IsEnabled = true;
                btnDelete.IsEnabled = true;
                btnEdit.IsEnabled = true;
                savePathUI.Text = selectedGame.savePath;
            }
        }

        public async void DisplayGames()
        { 
            List<Game> games = await GetAllGames();
            gameListView.ItemsSource = games;

        }

        private async Task<List<Game>> GetAllGames()
        {
            //var userUid = Guid.Parse("75E416A4-D292-482E-BD47-ABCECD68CF37"); //настроить выбор пользователя

            var userUid = _userUid;

            var response = await client.GetAsync($"Game/GetGames?userId={userUid}");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                if (jsonString == null) return null;

                List<Game> games = JsonConvert.DeserializeObject<List<Game>>(jsonString);

                return games;
            }
            else return null;
        }
        public string tempSavePath = string.Empty;
        public string tempGameName = string.Empty;
        private async void btnAdd_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var dataWindow = new AddGameDialog();
            dataWindow.Owner = Window.GetWindow(this); 
            dataWindow.ShowDialog();
            //string dummyUserId = @"75E416A4-D292-482E-BD47-ABCECD68CF37";

            await AddGame(_userUid, tempSavePath, tempGameName);

            DisplayGames();
        }
        
        private async Task AddGame(Guid userUid, string savePath, string gameName)
        {
            
            var response = await client.PostAsync($"Game/AddGame?userId={userUid}&savePath={savePath}&gameName={gameName}", null);
            responseUI.Text = response.StatusCode.ToString();
        }

        

        private async void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            selectedGame = gameListView.SelectedItem as Game;
            if (selectedGame == null) return;

            if (openFileDialog.ShowDialog() == true) //сейчас оно позволяет только выбрать(и потом перезаписать) уже имеющееся сохранение
            {
                string selectedFilePath = openFileDialog.FileName;
                selectedGame.savePath = selectedFilePath;
                savePathUI.Text = selectedGame.savePath; //binding надо
            }
            await EditPath(selectedGame.savePath);
        }

        private async Task EditPath(string savePath)
        {
            var regex = new Regex(@"^\S+\.\S+$");

            if (!regex.IsMatch(savePath))
            {
                savePathUI.Text = "invalid file format";
                return;
            }

            var response = await client.PostAsync($"Game/EditPath?gameId={App.GameUid}&savePath={savePath}", null);
        }

        private async void btnDelete_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var gameId = selectedGame.uid;
            await DeleteGame(gameId);
            gameListView.SelectedItem = null;
            DisplayGames();
        }

        private async Task DeleteGame(Guid gameId)
        {
            var response = await client.DeleteAsync($"Game/Remove?gameId={gameId}");

            responseUI.Text = response.StatusCode.ToString();
        }

        private void btnInspect_Navigation_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Window mainWindow = Window.GetWindow(this);
            if (mainWindow != null && mainWindow is MainWindow)
            {
                SavePage savePage = new SavePage(selectedGame.savePath);

                ((MainWindow)mainWindow).MainFrame.NavigationService.Navigate(savePage);
            }
        }


        // for binding
        /*public string SelectedGameSavePath
        {
            get { return selectedGameSavePath; }
            set
            {
                if (selectedGameSavePath != value)
                {
                    selectedGameSavePath = value;
                    OnPropertyChanged(nameof(SelectedGameSavePath));
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }*/
    }
}
