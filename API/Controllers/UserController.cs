using Microsoft.AspNetCore.Mvc;
using API.Models;
using API.Interfaces;
using API.Middlewares;
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
        public ActionResult<User> Login(User user)
        {
            return _repository.Login(user);
        }

        [HttpPost("Register")]
        public ActionResult<Status> Post(User user)
        {
            _repository.Register(user);
            return Ok(new Status() {message = "User registered successfully"});
        }
    }
}
