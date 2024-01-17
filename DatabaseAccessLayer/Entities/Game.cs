namespace DatabaseAccessLayer.Entities
{
    public class Game
    {
        public long Id            {get; init; }    
	    public required Guid Uid           {get; init; }    
        public required string Name       {get; set; }    
	    public required string SavePath    {get; set; }
        public  User User { get; set; } = null!;
        public ICollection<Save>? Saves { get; set; } = new List<Save>();
    };
}
;