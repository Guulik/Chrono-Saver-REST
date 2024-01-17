using Backend.API.Contract;
using Backend.API.Services;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System.Text.RegularExpressions;

namespace Backend.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly GameService _service;
        private readonly IMapper _mapper = CreateMapper();
        public GameController(GameService gameService)
        {
            _service = gameService;
        }

        [HttpPost]
        public ActionResult<Guid> AddGame(Guid userId, string savePath, string gameName) 
        {
            var uid = _service.AddGame(userId, savePath, gameName);
            return uid == Guid.Empty ? NotFound("User not found") : Ok(uid);
        }
        [HttpPost]
        public ActionResult EditPath(Guid gameId, string savePath)
        {
            return _service.EditPath(gameId, savePath) == false ? NotFound("Game not found") : Ok();
        }

        [HttpGet]
        public ActionResult<ICollection<Game>> GetGames(Guid userId) 
        {
            var db_games = _service.GetGames(userId);
            if (db_games == null) { return NotFound("User hasn't games"); }

            var contract_games = new List<Game>();
            foreach (var db_game in db_games)
            {
                contract_games.Add(_mapper.Map<Game>(db_game));
            }
            return Ok(contract_games);
        }
        [HttpGet]
        public ActionResult GetGame(Guid gameUid)
        {
            var db_game = _service.GetGame(gameUid);

            if (db_game == null) { return NotFound("Game does not exist"); }
            Game contract_game = _mapper.Map<Game>(db_game);

            return Ok(contract_game);
        }
        [HttpDelete]
        public ActionResult Remove(Guid gameId)
        {
            return _service.Remove(gameId) ? Ok() : NotFound("Game does not exist");
        }
        private static IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<DatabaseAccessLayer.Entities.Game, Contract.Game>());
            var mapper = new Mapper(config);

            return mapper;
        }

    }
    
}
