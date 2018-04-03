using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using aspnet_core_test_demo.Models;
using aspnet_core_test_demo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace aspnet_core_test_demo.Controllers
{
    public class PersonsController : Controller
    {
        private readonly IPersionService _persionService;

        public PersonsController(IPersionService persionService)
        {
            _persionService = persionService;
        }


        [HttpGet]
#pragma warning disable 1998
        public async Task<IActionResult> Get()
#pragma warning restore 1998
        {
            var models = _persionService.GetAll();

            return Ok(models);
        }

        [HttpGet("{id}")]
        // GET: Persons/Details/5
#pragma warning disable 1998
        public async Task<IActionResult> Get(int id)
#pragma warning restore 1998
        {
            var model = _persionService.Get(id);

            return Ok(model);
        }

        [HttpPost]
#pragma warning disable 1998
        public async Task<IActionResult> Post([FromBody] Person model)
#pragma warning restore 1998
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var person = _persionService.Add(model);

            return CreatedAtAction("Get", new {id = person.Id}, person);
        }

        [HttpPut("{id}")]
#pragma warning disable 1998
        public IActionResult Put(int id, [FromBody] Person model)
#pragma warning restore 1998
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _persionService.Update(id, model);

            return NoContent();
        }

        [HttpDelete("{id}")]
#pragma warning disable 1998
        public async Task<IActionResult> Delete(int id)
#pragma warning restore 1998
        {
            _persionService.Delete(id);
            return NoContent();
        }   
    }
}