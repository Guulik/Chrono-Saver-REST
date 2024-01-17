namespace Backend.API.Contract
{
    public class Save
    {
        public required Guid Uid { get; init; }

        public required byte[] SaveData { get; set; }

        public string SaveName { get; set; } = string.Empty;
    }

}
