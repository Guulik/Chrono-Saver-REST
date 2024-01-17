using DatabaseAccessLayer;
using DatabaseAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Backend.API.Services
{
    public class GameService
    {
        private readonly ChronoDbContext _chronoDbContext;
        private readonly SaveService _saveService;
        public GameService(ChronoDbContext chronoDbContext, SaveService saveService)
        {
            _chronoDbContext = chronoDbContext;
            _saveService = saveService;
        }


        public Guid AddGame(Guid userUid, string savePath, string gameName)
        {
            User user = _chronoDbContext.Set<User>().Include(u=>u.Games).SingleOrDefault(x=>x.Uid == userUid);
            
            if (user == null) { return Guid.Empty; }

            var game = new Game
            {
                Uid = Guid.NewGuid(),
                Name = gameName,
                SavePath = savePath,
                User = user
            };

            if (user.Games.IsNullOrEmpty()) 
            {
                user.Games = new List<Game>();
            }

            user.Games.Add(game);

            _chronoDbContext.Add(game);
            _chronoDbContext.SaveChanges();

            return game.Uid;
        }
        
        public bool EditPath(Guid gameUid, string savePath)
        {
            var game = _chronoDbContext.Set<Game>().SingleOrDefault(x => x.Uid == gameUid);

            if (game == null) { return false; }

            game.SavePath = savePath;
            _chronoDbContext.SaveChanges();

            return true;
        }

        public ICollection<Game> GetGames(Guid userUid)
        {
            var user = _chronoDbContext.Set<User>().Include(u => u.Games).SingleOrDefault(x => x.Uid == userUid);
            var userGames = user != null ? user.Games : null ;

            if (userGames.IsNullOrEmpty()) { return null; }

            return userGames;
        }
        public Game GetGame(Guid gameUid)
        {
            var game = _chronoDbContext.Set<Game>().SingleOrDefault(x => x.Uid == gameUid);
            if (game == null) { return null; }

            return game;
        }
        public bool Remove(Guid gameUid)
        {
            var game = _chronoDbContext.Set<Game>().Include(g=>g.Saves).SingleOrDefault(x => x.Uid == gameUid);
            if (game == null) return false;

            var gameSaves = game.Saves;
            if (gameSaves == null) { return false; }

            foreach(var save in gameSaves)
            {
                _saveService.RemoveSave(save.Uid);
            }
            _chronoDbContext.SaveChanges();

            _chronoDbContext.Set<Game>().Remove(game);
            _chronoDbContext.SaveChanges();
            return true;
        }

    }
}
