using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SEDC.NotesAppFluentApi.DTOs.Users;
using SEDC.NotesAppFluentApi.Services.Interfaces;
using SEDC.NotesAppFluentAPi.Shared.Shared;

namespace SEDC.NotesAppFluentApi.Controllers
{
    [Authorize] //all methods in this contoller require token
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous] //nenajaveni korisnici da go pristapuvaat metodot
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterUserDto registerUserDto)
        {
            //call some service
            try
            {
                _userService.RegisterUser(registerUserDto);
                return Ok();
            }
            catch (ResourceNotFoundException)
            {
                return NotFound("The user was not found");
            }
            catch
            {
                //we use EXCEPTION for logging 
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred");
            }
        }

        [AllowAnonymous]//nenajaveni korisnici da go pristapuvaat metodot
        [HttpPost("login")]
        //the rewsponse will contain the token, and the token is a string
        public ActionResult<string> Login([FromBody] LoginUserDto loginUser)
        {
            try
            {
                string token = _userService.Login(loginUser);
                return Ok(token);
            }
            catch (ResourceNotFoundException)
            {
                return NotFound("The user was not found");
            }
            catch
            {
                //we use EXCEPTION for logging 
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred");
            }
        }
    }
}
