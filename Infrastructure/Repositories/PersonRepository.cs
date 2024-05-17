using AdventureWorks.Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AdventureWorks.Infrastructure.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly AdventureWorksContext _context;

        public PersonRepository(AdventureWorksContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Person>> GetPersonList(int pageNumber, int pageSize)
{
    return await _context.People
                         .Skip((pageNumber - 1) * pageSize)
                         .Take(pageSize)
                         .ToListAsync();
}


        public async Task<Person> GetPersonById(int id)
        {
            var person = await _context.People.FirstOrDefaultAsync(p => p.BusinessEntityId == id);
            return person;
        }

        public async Task UpdatePerson(Person person)
        {
            _context.Entry(person).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeletePerson(int id)
        {
            var person = await _context.People.FindAsync(id);
            if (person != null)
            {
                _context.People.Remove(person);
                await _context.SaveChangesAsync();
            }
        }
    }
}
