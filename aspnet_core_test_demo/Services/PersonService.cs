using System.Collections.Generic;
using System.Linq;
using aspnet_core_test_demo.Models;

namespace aspnet_core_test_demo.Services
{
    public class PersonService : IPersionService
    {
        private List<Person> Persons { get; set; }

        public PersonService()
        {
            Persons = new List<Person>(50);
            var i = 0;
            Persons.ForEach((person) =>
            {
                i++;
                person.Id = i;
            });
        }

        public IEnumerable<Person> GetAll()
        {
            return Persons;
        }

        public Person Get(int id)
        {
            return Persons.First(p => p.Id == id);
        }

        public Person Add(Person person)
        {
            var newId = Persons.OrderBy(p => p.Id).Last().Id + 1;
            person.Id = newId;
            Persons.Add(person);

            return person;
        }

        public void Update(int id, Person person)
        {
            var existingPerson = Persons.First(p => p.Id == id);

            existingPerson.FirstName = person.FirstName;
            existingPerson.LastName = person.LastName;
            existingPerson.Adress = person.Adress;
            existingPerson.Age = person.Age;
            existingPerson.City = person.City;
            existingPerson.Email = person.Email;
            existingPerson.Phone = person.Phone;
            existingPerson.Title = person.Title;
        }

        public void Delete(int id)
        {
            var existingPerson = Persons.First(p => p.Id == id);
            Persons.Remove(existingPerson);
        }
    }
}