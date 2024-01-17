namespace DatabaseAccessLayer.Entities
{
    public class User
    {
        public long Id            {get; init; }    
	    public required Guid Uid           {get; init; }    
        public required string Email       {get; set; }    
	    public required string Password    {get; init; }

        public ICollection<Game> Games { get; set; } = new List<Game>(); 
    };
}
;