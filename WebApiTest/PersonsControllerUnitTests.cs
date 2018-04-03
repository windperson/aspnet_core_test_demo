using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using aspnet_core_test_demo.Controllers;
using aspnet_core_test_demo.Models;
using aspnet_core_test_demo.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace WebApiTest
{
    public class PersonsControllerUnitTests
    {
        [Fact]
        public async Task Values_Get_All()
        {
            //Arrange
            var controller = new PersonsController(new PersonService());

            //Act
            var result = await controller.Get();

            //Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var persons = okResult.Value.Should().BeAssignableTo<IEnumerable<Person>>().Subject;

            persons.Count().Should().Be(50);
        }

        [Fact]
        public async Task Values_Get_Id16()
        {
            //Arrange
            var controller = new PersonsController(new PersonService());
            const int personId = 16;

            //Act
            var result = await controller.Get(personId);

            //Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var person = okResult.Value.Should().BeAssignableTo<Person>().Subject;
            person.Id.Should().Be(personId);
        }

        [Fact]
        public async Task Persons_Add()
        {
            //Arrange
            var controller = new PersonsController(new PersonService());
            var newPerson = new Person
            {
                FirstName = "Tester",
                LastName = "Test",
                Age = 18,
                Title = "FooBar",
                Email = "tester@foo.bar"
            };

            //Act
            var result = await controller.Post(newPerson);

            //Assert
            var okResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
            var person = okResult.Value.Should().BeAssignableTo<Person>().Subject;
            person.Id.Should().Be(51);
        }

        [Fact]
        public async Task Persons_Change()
        {
            //Arrange
            var service = new PersonService();
            var controller = new PersonsController(service);
            var newPerson = new Person
            {
                FirstName = "Tester",
                LastName = "Test",
                Age = 18,
                Title = "FooBar",
                Email = "tester@foo.bar"
            };
            const int modifyPersonId = 20;

            //Act
            var result = controller.Put(20, newPerson);

            //Assert
            var okResult = result.Should().BeOfType<NoContentResult>().Subject;

            var person = service.Get(modifyPersonId);
            person.Id.Should().Be(modifyPersonId);
            person.FirstName.Should().Be(newPerson.FirstName);
            person.LastName.Should().Be(newPerson.LastName);
            person.Age.Should().Be(newPerson.Age);
            person.Title.Should().Be(newPerson.Title);
            person.Email.Should().Be(newPerson.Email);
        }

        [Fact]
        public async Task Persons_Delete()
        {
            //Arrange
            var service = new PersonService();
            var controller = new PersonsController(service);
            const int deletePersonId = 20;

            //Act
            var result = await controller.Delete(deletePersonId);

            //Assert
            var okResult = result.Should().BeOfType<NoContentResult>().Subject;
            Action action = () => service.Get(deletePersonId);
            action.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public async Task Persons_Get_From_Moq()
        {
            //Arrange
            var serviceMock = new Mock<IPersionService>();
            serviceMock.Setup(x => x.GetAll()).Returns(() => new List<Person>()
            {
                new Person{Id=1, FirstName="Foo", LastName="Bar"},
                new Person{Id=2, FirstName="John", LastName="Doe"},
                new Person{Id=3, FirstName="Juergen", LastName="Gutsch"}
            });

            var controller = new PersonsController(serviceMock.Object);

            //Act
            var result = await controller.Get();

            //Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var persons = okResult.Should().BeAssignableTo<IEnumerable<Person>>().Subject;
            persons.Count().Should().Be(3);
        }
    }
}
