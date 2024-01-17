
namespace DatabaseAccessLayer.Entities
{
    public class Save
    {
        public long Id                      {get; init; }
        public required Guid Uid            { get; init;}

        public required byte[] SaveData     {get; set; }  

        public string SaveName              { get; set;} = string.Empty;
        
        public  Game Game                   { get; set;}
    };
}
;