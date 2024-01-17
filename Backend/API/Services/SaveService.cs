using DatabaseAccessLayer;
using DatabaseAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;


namespace Backend.API.Services
{
    public class SaveService
    {
        private readonly ChronoDbContext _chronoDbContext;

        public SaveService(ChronoDbContext chronoDbContext)
        {
            _chronoDbContext = chronoDbContext;
        }

            
        public Guid AddSave(Guid gameUid, string saveName, byte[] saveData)
        {
            var game = _chronoDbContext.Set<Game>().SingleOrDefault(x => x.Uid == gameUid);

            if (game == null) { return Guid.Empty; }
            var save = new Save
            {
                Uid = Guid.NewGuid(),
                Game = game,
                SaveData = saveData,
                SaveName = saveName
            };

            if (game.Saves.IsNullOrEmpty())
            {
                game.Saves = new List<Save>();
            }

            game.Saves.Add(save);

            _chronoDbContext.Add(save);
            _chronoDbContext.SaveChanges();

            return save.Uid;
        }

        public bool OverrideSave(Guid saveUid, byte[] saveData)
        {
            var save = _chronoDbContext.Set<Save>().SingleOrDefault(x => x.Uid == saveUid);
            if(save == null) { return false; }

            save.SaveData = saveData;

            _chronoDbContext.SaveChanges();

            return true;
        }
        public Save GetSave(Guid saveUid)
        {
            var save = _chronoDbContext.Set<Save>().SingleOrDefault(x => x.Uid == saveUid);

            if (save == null) { return null; }

            var saveData = save.SaveData;
            
            return save;
        }
        public ICollection<Save> GetSaves(Guid gameUid)
        {
            var game = _chronoDbContext.Set<Game>().Include(g=>g.Saves).SingleOrDefault(x => x.Uid == gameUid);

            if (game == null) { return null; }
            var saves = game.Saves;

            return saves; 
        }
       
        public bool RemoveSave(Guid saveUid)
        {
            var save = _chronoDbContext.Set<Save>().Include(g => g.Game).SingleOrDefault(x => x.Uid == saveUid);
            if (save == null) return false;

            var gameSaves = save.Game.Saves;

            if (gameSaves.IsNullOrEmpty()) return false; //not necessary

            gameSaves.Remove(save);

            _chronoDbContext.Set<Save>().Remove(save);
            _chronoDbContext.SaveChanges();
            return true;
        }
    }
}
