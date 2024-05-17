using AdventureWorks.Domain.Interfaces;
using Domain.Models;

namespace AdventureWorks.Application.UseCases
{
    public class GetAllPeopleUseCase
    {
        private readonly IPersonRepository _personRepository;

        public GetAllPeopleUseCase(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public async Task<IEnumerable<Person>> ExecuteAsync(int pageNumber = 1, int pageSize = 50)
        {
            return await _personRepository.GetPersonList(pageNumber, pageSize);
        }
    }
}
