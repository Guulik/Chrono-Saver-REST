using Microsoft.Win32;
using System.Windows;

namespace Frontend
{

    public partial class AddGameDialog : Window
    {
        newGame _game;
        OpenFileDialog openFileDialog;
        MainWindow ownerWindow;
        public AddGameDialog()
        {
            InitializeComponent();
            ownerWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Выберите файл";
            openFileDialog.Filter = "Все файлы (*.*)|*.*";
            btnOk.IsEnabled = false;
            _game = new newGame();
        }

        public class newGame()
        {
            public string savePath;
            public string gameName = "Game";
        }
        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            
            if (openFileDialog.ShowDialog() == true)
            {
                string selectedFilePath = openFileDialog.FileName;
                savePathUI.Text = selectedFilePath; //binding надо
                ownerWindow.GamePage.tempSavePath = selectedFilePath;
            }
            if (ownerWindow.GamePage.tempSavePath != string.Empty)
            {
                btnOk.IsEnabled = true;
            }
        }
        private void btnOk_Click(object sender, RoutedEventArgs e)
        {            
            if (ownerWindow != null)
            {
                ownerWindow.GamePage.tempGameName = gameNameUI.Text;
            }
            this.Close();
        }
    }
}
