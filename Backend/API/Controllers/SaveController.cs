using Backend.API.Services;
using Backend.API.Contract;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace Backend.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SaveController : ControllerBase
    {

        private readonly SaveService _service;
        private readonly IMapper _mapper = CreateMapper();
        public SaveController(SaveService saveService)
        {
            _service = saveService;
        }

        [HttpPost]
        public ActionResult<Guid> AddSave(Guid gameId, string saveName, [FromBody] byte[] saveData) 
        {
            return _service.AddSave(gameId, saveName, saveData) == Guid.Empty ? NotFound("Game not found") : Ok();
        }
        [HttpPost]
        public ActionResult OverrideSave(Guid saveId, byte[]  saveData)
        {
            return _service.OverrideSave(saveId, saveData)? Ok() : NotFound();
        }
        [HttpGet]
        public ActionResult GetSave(Guid saveId) 
        {
            var db_save = _service.GetSave(saveId);

            if (db_save == null) { return NotFound("Save does not exist"); }
            Save contract_save = _mapper.Map<Save>(db_save);

            return Ok(contract_save);
        }
        [HttpGet]
        public ActionResult<ICollection<Save>> GetSaves(Guid gameId)
        {
            var db_saves = _service.GetSaves(gameId);
            if (db_saves == null) { return NotFound(); }

            var contract_saves = new List<Save>();
            foreach (var db_save in db_saves)
            {
                contract_saves.Add(_mapper.Map<Save>(db_save));
            }
            return Ok(contract_saves);
        }
        [HttpDelete]
        public ActionResult RemoveSave(Guid saveId)
        {
            return _service.RemoveSave(saveId) ? Ok() : NotFound("Save does not exist");
        }

        private static IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<DatabaseAccessLayer.Entities.Save, Contract.Save>());
            var mapper = new Mapper(config);

            return mapper;
        }
    }
   
}
