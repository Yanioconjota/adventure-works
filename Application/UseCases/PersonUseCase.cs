using AdventureWorks.Domain.Interfaces;
using Domain.Models;

namespace AdventureWorks.Application.UseCases
{
    public class PersonUseCase
    {
        private readonly IPersonRepository _personRepository;

        public PersonUseCase(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public async Task<IEnumerable<Person>> GetPersonList(CancellationToken cancellationToken, int pageNumber = 1, int pageSize = 50)
        {
            return await _personRepository.GetPersonList(cancellationToken, pageNumber, pageSize);
        }

        public async Task<IEnumerable<Person>> GetPersonByName(string name)
        {
            return await _personRepository.GetPersonByName(name);
        }

        public async Task<Person> GetPersonById(int id)
        {
            return await _personRepository.GetPersonById(id);
        }

        public async Task<Person> AddPerson(Person person)
        {
            return await _personRepository.AddPerson(person);
        }

        public async Task UpdatePerson(Person person)
        {
            await _personRepository.UpdatePerson(person);
        }

        public async Task DeletePerson(int id)
        {
            await _personRepository.DeletePerson(id);
        }
    }
}
