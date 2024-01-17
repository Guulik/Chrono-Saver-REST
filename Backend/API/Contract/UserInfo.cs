namespace Backend.API.Contract
{
    public class UserInfo
    {
        public required Guid Uid { get; init; }
        public required string email { get; init; }
    }

}
