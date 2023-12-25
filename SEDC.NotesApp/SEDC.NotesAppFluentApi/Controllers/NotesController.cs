using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SEDC.NotesAppFluentApi.Domain.Models;
using SEDC.NotesAppFluentApi.DTOs.Notes;
using SEDC.NotesAppFluentApi.Services.Interfaces;
using SEDC.NotesAppFluentAPi.Shared.Shared;
using Serilog;
using System.Security.Claims;
using InvalidDataException = SEDC.NotesAppFluentAPi.Shared.Shared.InvalidDataException;

namespace SEDC.NotesAppFluentApi.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly INoteService _noteService;

        public NotesController(INoteService noteService) //Dependency Injection
        {
            _noteService = noteService;
        }

        [Authorize]
        /*[Authorize]*/ //user must be logged in to access this method(must send a token)
        [HttpGet]
        public ActionResult<List<NotesDto>> GetAll()
        {
            try
            {
                return Ok(_noteService.GetAll());
            }
            catch(Exception ex)
            {
                Log.Error("An error occured");
                string exceptionString = JsonConvert.SerializeObject(ex);
                Log.Error(exceptionString);

                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred");
            }
        }

        [HttpGet("{id}")]
        public ActionResult<NotesDto> GetById(int id)
        {
            try
            {
                if(id <= 0)
                {
                    return BadRequest("Id can not be null");
                }

                return Ok(_noteService.GetById(id));
            }
            catch (ResourceNotFoundException)
            {
                return NotFound("The note was not found");
            }
            catch
            {
                //we use EXCEPTION for logging 
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred");
            }
        }
        //note can be added only from user with role admin or superAdmin
        [Authorize]
        [HttpPost("addNote")]
        public IActionResult AddNote([FromBody] AddNoteDto addNoteDto)
        {
            try
            {
                //we must validate if the user is with role admin or superAdmin
                string role = User.Claims.First(x => x.Type == "userRole").Value;
                if (role != "Admin")
                {
                    return StatusCode(StatusCodes.Status403Forbidden);
                }
                Log.Information("User authorized");
                _noteService.AddNote(addNoteDto);
                Log.Information("Note added");
                return StatusCode(StatusCodes.Status201Created, "Note created");
            }
            catch (InvalidDataException e)
            {
                Log.Error("Data related error occrured");
                string exceptionString = JsonConvert.SerializeObject(e);
                Log.Error(exceptionString);
                return BadRequest("Invalid data");
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred");
            }
        }

        [HttpPut]
        public IActionResult UpdateNote([FromBody] UpdateNoteDto updateNoteDto)
        {
            try
            {
                _noteService.UpdateNote(updateNoteDto);
                return NoContent();
            }
            catch (ResourceNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (InvalidDataException e)
            {
                return BadRequest(e.Message);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteNote(int id)
        {
            try
            {
                _noteService.DeleteNote(id);
                return Ok("Note succefully deleted");

            }
            catch (ResourceNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred");
            }
        }
    }
}
