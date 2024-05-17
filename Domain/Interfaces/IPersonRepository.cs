using Domain.Models;

namespace AdventureWorks.Domain.Interfaces
{
    public interface IPersonRepository
    {
        Task<IEnumerable<Person>> GetPersonList(int pageNumber, int pageSize);
        Task<Person> GetPersonById(int id);
        // Task<Person> AddAsync(Person person);
        Task UpdatePerson(Person person);
        Task DeletePerson(int id);
    }
}
