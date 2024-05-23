using Domain.Models;

namespace AdventureWorks.Domain.Interfaces
{
    public interface IPersonRepository
    {
        Task<IEnumerable<Person>> GetPersonList(CancellationToken cancellationToken, int pageNumber, int pageSize, string orderBy);
        Task<Person> GetPersonById(int id);
        Task<Person> AddPerson(Person person);
        Task<IEnumerable<Person>> GetPersonByName(string name);
        Task UpdatePerson(Person person);
        Task DeletePerson(int id);
    }
}
