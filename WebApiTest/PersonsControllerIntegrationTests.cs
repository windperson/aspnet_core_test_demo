using Microsoft.AspNetCore.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using aspnet_core_test_demo;
using aspnet_core_test_demo.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using Xunit;

namespace WebApiTest
{
    public class PersonsControllerIntegrationTests
    {
        private readonly HttpClient _client;

        public PersonsControllerIntegrationTests()
        {
            var server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            _client = server.CreateClient();
        }

        [Fact]
        public async Task Persons_Get_All()
        {
            //Act
            var response = await _client.GetAsync("/api/Persons");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            //Assert
            var persons = JsonConvert.DeserializeObject<IEnumerable<Person>>(responseString);
            persons.Count().Should().Be(50);
        }

        [Fact]
        public async Task Persons_Get_Specific()
        {
            //Act
            var response = await _client.GetAsync("/api/Persons/16");

            //Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            var person = JsonConvert.DeserializeObject<Person>(responseString);
            person.Id.Should().Be(16);
        }

        [Fact]
        public async Task Persons_Post_Specific()
        {
            //Arrange
            var personToAdd = new Person
            {
                FirstName = "Tester",
                LastName = "Demo",
                Age = 18,
                Title = "FooBar",
                Phone = "1234567890",
                Email = "abc@def.ghi"
            };
            var content = JsonConvert.SerializeObject(personToAdd);
            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");

            //Act
            var response = await _client.PostAsync("/api/Persons", stringContent);

            //Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var person = JsonConvert.DeserializeObject<Person>(responseString);
            person.Id.Should().Be(51);
        }

        [Fact]
        public async Task Persons_Post_Specific_Invalid()
        {
            //Arrange
            var personToAdd = new Person { FirstName = "MyTest" };
            var content = JsonConvert.SerializeObject(personToAdd);
            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");

            //Act
            var response = await _client.PostAsync("/api/Persons", stringContent);

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            var responseString = await response.Content.ReadAsStringAsync();
            responseString.Should().Contain("The Email field is required")
                .And.Contain("The LastName field is requried")
                .And.Contain("The Phone field is required");
        }

        [Fact]
        public async Task Persons_Put_Specific()
        {
            //Arrange
            var personToChange = new Person
            {
                Id = 16,
                FirstName = "Tester",
                LastName = "MyTest",
                Age = 18,
                Title = "FooBar",
                Phone = "1234567890",
                Email = "abc@def.ghi"
            };

            var content = JsonConvert.SerializeObject(personToChange);
            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");

            //Act
            var response = await _client.PutAsync("/api/Persons/16", stringContent);

            //Assert
            response.EnsureSuccessStatusCode();
            var responseSTtring = await response.Content.ReadAsStringAsync();
            responseSTtring.Should().Be(string.Empty);
        }

        [Fact]
        public async Task Persons_Put_Specific_Invalid()
        {
            //Arrange
            var personToChange = new Person
            {
                FirstName = "Tester"
            };

            var content = JsonConvert.SerializeObject(personToChange);
            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");

            //Act
            var response = await _client.PutAsync("/api/Persons/16", stringContent);

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            var responseSTtring = await response.Content.ReadAsStringAsync();
            responseSTtring.Should().Contain("The Email field is required")
                .And.Contain("The LastName field is requried")
                .And.Contain("The Phone field is required");
        }

        [Fact]
        public async Task Persons_Delete_Specific()
        {
            //Act
            var response = await _client.DeleteAsync("/api/Persons/16");

            //Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            responseString.Should().Be(string.Empty);
        }
    }
}
