using System.Net.Http;
using System.Windows;

namespace Frontend
{
    public partial class App : Application
    {
        public static HttpClient client { get; } = new HttpClient();
        public static Guid GameUid { get; set; } = Guid.Empty;
        public App() {
            client.BaseAddress = new Uri("http://localhost:5194/api/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json")
            );
        }
    }
    public class Game
    {
        public required Guid uid { get; init; }
        public required string name { get; set; }
        public required string savePath { get; set; }
    }
    public class Save
    {
        public required Guid Uid { get; init; }

        public required byte[] SaveData { get; set; }

        public string SaveName { get; set; } = string.Empty;
    }
}
