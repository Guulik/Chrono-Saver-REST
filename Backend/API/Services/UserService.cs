using DatabaseAccessLayer;
using DatabaseAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;


namespace Backend.API.Services
{
    public class UserService
    {
        private readonly ChronoDbContext _chronoDbContext;
        private readonly GameService _gameService;

        public UserService(ChronoDbContext chronoDbContext, GameService gameService)
        {
            _chronoDbContext = chronoDbContext;
            _gameService = gameService;
        }

        
        public Guid Register(string email, string password)
        {
            var user = new User
            {
                Uid = Guid.NewGuid(),
                Email = email,
                Password = GetHash(password)
            };
            _chronoDbContext.Set<User>().Add(user);
            _chronoDbContext.SaveChanges();

            return user.Uid;
        }
        
        public Guid? Login(string email, string password)
        {
            var user = _chronoDbContext.Set<User>().SingleOrDefault(u => u.Email == email && u.Password == GetHash(password));
            
            return user?.Uid;
        }
        
        public Contract.UserInfo? GetInfo(Guid uid)
        {
            var user = _chronoDbContext.Set<User>().SingleOrDefault(x =>  x.Uid == uid);

            if (user == null) { return null; }

            return new Contract.UserInfo
            {
                Uid = user.Uid,
                email = user.Email,
            };
        }
       
        public bool Delete(Guid uid)
        {
            var transaction = _chronoDbContext.Database.BeginTransaction();
            var user = _chronoDbContext.Set<User>().Include(u=>u.Games).SingleOrDefault(x => x.Uid == uid);
            if (user == null) return false;

            var userGames = user.Games;
            foreach (var game in userGames)
            {
                if (!_gameService.Remove(game.Uid))
                {
                    transaction.Rollback();
                    return false;
                }
            }

            _chronoDbContext.Set<User>().Remove(user);
            _chronoDbContext.SaveChanges();

            transaction.Commit();
            return true;
            
        }
        private string GetHash(string password)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));

            return Convert.ToHexString(bytes);
        }
    }
}
