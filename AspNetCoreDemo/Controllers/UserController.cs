using AspNetCoreDemo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using AspNetCoreDemo.Dependencies;

namespace AspNetCoreDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;

        private readonly DefaultUserOptions _defaultUserOptions;

        private readonly ITestDependency _testDependency;

        public UserController(ILogger<UserController> logger, IOptions<DefaultUserOptions> options, ITestDependency testDependency)
        {
            _logger = logger;
            _defaultUserOptions = options.Value;
            _testDependency = testDependency;
        }

        [HttpGet]
        public User Get()
        {
            var testString = _testDependency.GetTestSring();
            _logger.LogInformation("Test string: {0}", testString);

            User user = new User() { Name = _defaultUserOptions.Name, Title = _defaultUserOptions.Title };
            _logger.LogCritical("User retrieved: {0}", user);
            _logger.LogDebug("User retrieved: {0}", user);
            _logger.LogError("User retrieved: {0}", user);
            _logger.LogInformation("User retrieved: {0}", user);
            _logger.LogWarning("User retrieved: {0}", user);
            _logger.LogTrace("User retrieved: {0}", user);

            return user;
        }
    }
}
