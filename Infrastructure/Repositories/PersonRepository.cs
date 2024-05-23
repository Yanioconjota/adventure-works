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
                    // TODO: Handle failure
                    transaction.Rollback();
                }
            }
        }
    }
}
