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

        public async Task<IEnumerable<Person>> GetPersonList(CancellationToken cancellationToken, int pageNumber, int pageSize, string orderBy)
        {
            IQueryable<Person> query =  _context.People.AsQueryable();
            
            switch (orderBy.ToLower())
            {
                case "asc":
                    query = query.OrderBy(p => p.BusinessEntityId);
                    break;
                case "desc":
                    query = query.OrderByDescending(p => p.BusinessEntityId);
                    break;
                default:
                    throw new ArgumentException("Invalid order by parameter");
            }

            // Paginación
            query = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            return await query.ToListAsync(cancellationToken);
        }



        public async Task<Person> GetPersonById(int id)
        {
            var person = await _context.People.FirstOrDefaultAsync(p => p.BusinessEntityId == id);
            if (person == null)
            {
                throw new KeyNotFoundException("No Person found with the specified ID.");
            }
            return person;
        }

        public async Task<IEnumerable<Person>> GetPersonByName(string name)
        {
            var people = await _context.People
                                    .Where(p => p.FirstName.Contains(name) || p.LastName.Contains(name))
                                    .ToListAsync();
            return people;
        }

        public async Task<Person> AddPerson(Person person)
        {
            // If the person has a BusinessEntity, ensure it's tracked by the context
            if (person.BusinessEntity != null)
            {
                // If the BusinessEntity already exists, attach it to the context
                if (_context.BusinessEntities.Any(b => b.BusinessEntityId == person.BusinessEntity.BusinessEntityId))
                {
                    _context.BusinessEntities.Attach(person.BusinessEntity);
                }
                // If the BusinessEntity is new, add it to the context
                else
                {
                    _context.BusinessEntities.Add(person.BusinessEntity);
                }
            }

            _context.People.Add(person);
            await _context.SaveChangesAsync();

            return person;
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
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    // Delete related records in SalesOrderHeader
                    var salesOrderHeaders = _context.SalesOrderHeaders.Where(soh => soh.Customer.PersonId == id);
                    _context.SalesOrderHeaders.RemoveRange(salesOrderHeaders);

                    // Delete related records in Customer
                    var customers = _context.Customers.Where(c => c.PersonId == id);
                    _context.Customers.RemoveRange(customers);

                    // Delete related records in BusinessEntityContact
                    var businessEntityContacts = _context.BusinessEntityContacts.Where(bec => bec.PersonId == id);
                    _context.BusinessEntityContacts.RemoveRange(businessEntityContacts);

                    // Delete related records in EmailAddress
                    var emailAddresses = _context.EmailAddresses.Where(ea => ea.BusinessEntityId == id);
                    _context.EmailAddresses.RemoveRange(emailAddresses);

                    // Delete related records in PersonPhone
                    var personPhones = _context.PersonPhones.Where(pp => pp.BusinessEntityId == id);
                    _context.PersonPhones.RemoveRange(personPhones);

                    // Delete related records in Password
                    var passwords = _context.Passwords.Where(p => p.BusinessEntityId == id);
                    _context.Passwords.RemoveRange(passwords);

                    // Delete related records in PersonCreditCard
                    var personCreditCards = _context.PersonCreditCards.Where(pcc => pcc.BusinessEntityId == id);
                    _context.PersonCreditCards.RemoveRange(personCreditCards);

                    // Finally, delete the record in Person
                    var person = await _context.People.FindAsync(id);
                    if (person != null)
                    {
                        _context.People.Remove(person);
                    }

                    await _context.SaveChangesAsync();

                    // Complete the transaction if everything has been successful
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }
            }
        }
    }
}
