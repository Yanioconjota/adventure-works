using Microsoft.AspNetCore.Mvc;
using AdventureWorks.Application.UseCases;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using AdventureWorks.Application.DTOs;

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
        public async Task<IActionResult> Get(
            [FromQuery] CancellationToken cancellationToken,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string orderBy = "desc")
        {
            var people = await _getAllPeopleUseCase.GetPersonList(cancellationToken, pageNumber, pageSize, orderBy);
            return Ok(people);
        }


        [HttpGet("GetPeopleByName")]
        public async Task<IActionResult> GetByName([FromQuery] string name)
        {
            var people = await _getAllPeopleUseCase.GetPersonByName(name);
            return Ok(people);
        }

        [HttpGet("GetpersonById")]
        public async Task<IActionResult> GetById(int id)
        {
            var person = await _getAllPeopleUseCase.GetPersonById(id);
            if (person == null)
            {
                return NotFound();
            }
            return Ok(person);
        }

        [HttpPost("AddPerson")]
        public async Task<IActionResult> Add([FromBody] PersonDto personDto)
        {
            try
            {
                var person = new Person
                {
                    PersonType = personDto?.PersonType ?? string.Empty,
                    NameStyle = personDto?.NameStyle ?? false,
                    Title = personDto?.Title,
                    FirstName = personDto?.FirstName,
                    MiddleName = personDto?.MiddleName,
                    LastName = personDto?.LastName ?? string.Empty,
                    Suffix = personDto?.Suffix,
                    EmailPromotion = (int)(personDto?.EmailPromotion),
                    AdditionalContactInfo = personDto?.AdditionalContactInfo,
                    Demographics = personDto?.Demographics,
                    Rowguid = Guid.NewGuid(),
                    ModifiedDate = DateTime.UtcNow
                };

                var newPerson = await _getAllPeopleUseCase.AddPerson(person);
                return CreatedAtAction(nameof(GetById), new { id = newPerson.BusinessEntityId }, newPerson);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpPut("UpdatePerson")]
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
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("DeletePerson")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _getAllPeopleUseCase.DeletePerson(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

    }
}
