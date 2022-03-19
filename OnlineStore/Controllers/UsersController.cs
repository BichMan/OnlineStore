using Microsoft.AspNetCore.Mvc;
using OnlineStore.Authorization;
using OnlineStore.Entities;
using OnlineStore.Helpers;
using OnlineStore.Models.Users;
using OnlineStore.Services;
using System.Threading.Tasks;
using BCryptNet = BCrypt.Net.BCrypt;

namespace OnlineStore.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;
        private DataContext _context;

        public UsersController(IUserService userService, DataContext context)
        {
            _userService = userService;
            _context = context;
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public IActionResult Authenticate(AuthenticateRequest model)
        {
            var response = _userService.Authenticate(model);
            return Ok(response);
        }

        [Authorize(Role.Admin)]
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            // only admins can access other user records
            var currentUser = (User)HttpContext.Items["User"];
            if (id != currentUser.Id && currentUser.Role != Role.Admin)
                return Unauthorized(new { message = "Unauthorized" });

            var user =  _userService.GetById(id);
            return Ok(user);
        }
        [Authorize(Role.Admin)]
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(RegisterRequest model)
        {

            if (model.Username == null || model.Password == null)
            {
                return NotFound(new { message = "Điền đầy đủ thông tin." });
            }
            var user = new User();
            user.Username = model.Username;
            user.PasswordHash = BCryptNet.HashPassword(model.Password);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Đăng ký thành công." });


        }
    }
}
