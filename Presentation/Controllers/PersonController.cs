using Microsoft.AspNetCore.Mvc;
using AdventureWorks.Application.UseCases;
using Domain.Models;

namespace AdventureWorks.Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly PersonUseCase _getAllPeopleUseCase;

        public PersonController(PersonUseCase getAllPeopleUseCase)
        {
            _getAllPeopleUseCase = getAllPeopleUseCase;
        }

        [HttpGet("GetPeople")]
        public async Task<IActionResult> Get([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 50)
        {
            var people = await _getAllPeopleUseCase.ExecuteAsync(pageNumber, pageSize);
            return Ok(people);
        }

        [HttpGet("GetPeopleByName")]
        public async Task<IActionResult> GetByName([FromQuery] string name)
        {
            var people = await _getAllPeopleUseCase.GetPersonByName(name);
            return Ok(people);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var person = await _getAllPeopleUseCase.GetPersonById(id);
            if (person == null)
            {
                return NotFound();
            }
            return Ok(person);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Person person)
        {
            if (id != person.BusinessEntityId)
            {
                return BadRequest();
            }

            try
            {
                await _getAllPeopleUseCase.UpdatePerson(person);
                return NoContent();
            }
            catch (Exception ex)
            {
                // Aquí podrías manejar excepciones específicas si el elemento no existe o hay un conflicto.
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _getAllPeopleUseCase.DeletePerson(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                // Manejar la excepción si la persona no existe o no se puede eliminar.
                return NotFound();
            }
        }

    }
}
