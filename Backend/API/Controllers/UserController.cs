using AutoMapper;
using Backend.API.Contract;
using Backend.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace Backend.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly UserService _service;
        private readonly IMapper _mapper = CreateMapper();
        public UserController(UserService service)
        {
            _service = service;
        }

        [HttpPost]
        public ActionResult<Guid> Register(string email, string password)
        {
            var regex = new Regex(@"^\S+@\S+\.\S+$");

            if(!regex.IsMatch(email))
            {
                ModelState.AddModelError("email", "Invalid email format");

                return BadRequest(ModelState);
            }


            return _service.Register(email, password);
        }
        [HttpPost]
        public ActionResult<Guid> Login(string email, string password) 
        {
            var userUid = _service.Login(email, password);

            if (userUid == null)
            {
                ModelState.AddModelError("user", "Incorrect email or password");

                return BadRequest(ModelState);
            }
            return userUid;

        }
        [HttpGet]
        public ActionResult<UserInfo> GetInfo(Guid uid)
        {
            var user = _service.GetInfo(uid);

            if (user == null)
            {
                return NotFound();
            }

            UserInfo userInfo = _mapper.Map<UserInfo>(user);
            return userInfo;
        }
        [HttpDelete]
        public ActionResult Delete(Guid uid)
        {
            return _service.Delete(uid) ? Ok() : NotFound();
        }
        private static IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<DatabaseAccessLayer.Entities.User, Contract.UserInfo>());
            var mapper = new Mapper(config);

            return mapper;
        }
    }
}
