using System;

namespace AspNetCoreDemo.Dependencies
{
    public class TestDependency : ITestDependency
    {
        private string _operationId;

        public TestDependency()
        {
            _operationId = Guid.NewGuid().ToString();
        }

        public string GetTestSring() => $"test id {_operationId}";
    }
}
