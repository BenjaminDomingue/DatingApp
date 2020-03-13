using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Models;
using DatingApp.API.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;

        public AuthController(IAuthRepository repo)
        {
            _repo = repo;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterViewModel userForRegisterViewModel)
        {
            // validate request

            userForRegisterViewModel.Username = userForRegisterViewModel.Username.ToLower();

            if (await _repo.UserExists(userForRegisterViewModel.Username))
                return BadRequest("Username already exists");

            var userToCreate = new User()
            {
                Username = userForRegisterViewModel.Username,
            };

            var createdUser = await _repo.Register(userToCreate, userForRegisterViewModel.Password);

            return StatusCode(201);
        }
    }
}
