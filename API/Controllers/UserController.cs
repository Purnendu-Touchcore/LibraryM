using Microsoft.AspNetCore.Mvc;
using API.Models;
using API.Interfaces;
using API.Middlewares;
using API.DTOs;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserRepository _repository;
        private ILogger<UserController> _logger;    

        public UserController(IUserRepository repository, ILogger<UserController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpPost("Login")]
        public ActionResult<UserDTO> Login(UserDTO user)
        {
            return _repository.Login(user);
        }

        [HttpPost("Register")]
        public ActionResult<Status> Post(UserDTO user)
        {
            _repository.Register(user);
            return Ok(new Status() {message = "User registered successfully"});
        }
    }
}
