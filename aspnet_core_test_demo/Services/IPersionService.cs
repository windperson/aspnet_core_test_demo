using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using aspnet_core_test_demo.Models;

namespace aspnet_core_test_demo.Services
{
    public interface IPersionService
    {
        IEnumerable<Person> GetAll();
        Person Get(int id);
        Person Add(Person person);
        void Update(int id, Person person);
        void Delete(int id);
    }
}
