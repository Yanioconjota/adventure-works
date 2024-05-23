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

        public async Task<IEnumerable<Person>> GetPersonList(CancellationToken cancellationToken, int pageNumber, int pageSize)
        {
            if (!cancellationToken.IsCancellationRequested)
                return await _context.People
                                .Skip((pageNumber - 1) * pageSize)
                                .Take(pageSize)
                                .ToListAsync();
            {
                return null;
            }
            
        }


        public async Task<Person> GetPersonById(int id)
        {
            var person = await _context.People.FirstOrDefaultAsync(p => p.BusinessEntityId == id);
            return person;
        }

        public async Task<IEnumerable<Person>> GetPersonByName(string name)
        {
            var people = await _context.People
                                    .Where(p => p.FirstName.Contains(name) || p.LastName.Contains(name))
                                    .ToListAsync();
            return people;
        }



        public async Task UpdatePerson(Person person)
        {
            var existingPerson = await _context.People
                .FirstOrDefaultAsync(p => p.BusinessEntityId == person.BusinessEntityId);

            if (existingPerson == null)
            {
                throw new KeyNotFoundException("No se encontró la persona con el ID especificado.");
            }

            _context.Entry(existingPerson).CurrentValues.SetValues(person);
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
