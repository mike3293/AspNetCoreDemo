using AspNetCoreDemo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using AspNetCoreDemo.Dependencies;
using System.Collections.Generic;
using AspNetCoreDemo.Filters;
using System.Linq;

namespace AspNetCoreDemo.ApiControllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private static List<User> _users = new() { new() { Id = 1, Name = "One", Title = "Employee" }, new() { Id = 2, Name = "Two", Title = "Employee" } };

        private readonly ILogger<UsersController> _logger;

        private readonly DefaultUserOptions _defaultUserOptions;

        private readonly ITestDependency _testDependency;


        public UsersController(ILogger<UsersController> logger, IOptions<DefaultUserOptions> options, ITestDependency testDependency)
        {
            _logger = logger;
            _defaultUserOptions = options.Value;
            _testDependency = testDependency;
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            return Ok(_users);
        }

        [HttpGet("defaultUser")]
        public IActionResult GetDefaultUser()
        {
            var testString = _testDependency.GetTestSring();
            _logger.LogInformation("Test string: {0}", testString);

            User user = new User() { Name = _defaultUserOptions.Name, Title = _defaultUserOptions.Title };
            _logger.LogDebug("User retrieved: {0}", user);

            return Ok(user);
        }

        [HttpGet("{id:int}")]
        [CookiesAppendFilter]
        public IActionResult GetUserById(int id)
        {
            var user = _users.Find(u => u.Id == id);

            if (user is null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // {name:alpha:maxlength(50)}
        [HttpGet("search")]
        public IActionResult GetUsersByName(string name)
        {
            var users = _users.Where(u => u.Name.StartsWith(name));

            return Ok(users);
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateUser(int id, User user)
        {
            var userToUpdate = _users.Find(u => u.Id == id);

            if (userToUpdate is null)
            {
                return NotFound();
            }

            userToUpdate.Name = user.Name;
            userToUpdate.Title = user.Title;

            return Ok(userToUpdate);
        }

        [HttpPost]
        public IActionResult AddUser([FromBody] User user)
        {
            var newUserId = _users[^1].Id + 1;

            user.Id = newUserId;
            _users.Add(user);

            return Ok(user);
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteUser(int id)
        {
            var user = _users.Find(u => u.Id == id);

            if (user is null)
            {
                return NotFound();
            }

            _users.Remove(user);

            return Ok();
        }
    }
}
