using Microsoft.AspNetCore.Mvc;
using AdventureWorks.Application.UseCases;

namespace AdventureWorks.Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly GetAllPeopleUseCase _getAllPeopleUseCase;

        public PersonController(GetAllPeopleUseCase getAllPeopleUseCase)
        {
            _getAllPeopleUseCase = getAllPeopleUseCase;
        }

        [HttpGet("GetPeople")]
        public async Task<IActionResult> Get([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 50)
        {
            var people = await _getAllPeopleUseCase.ExecuteAsync(pageNumber, pageSize);
            return Ok(people);
        }
    }
}
