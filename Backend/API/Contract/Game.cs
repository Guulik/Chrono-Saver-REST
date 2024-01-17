namespace Backend.API.Contract
{
    public class Game
    {
        public required Guid uid { get; init; }
        public required string name { get; set; }
        public required string savePath { get; set; }
    }

}
