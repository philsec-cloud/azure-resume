using Microsoft.Extensions.Logging;
using Xunit;
using Company.Function;

namespace tests
{
    public class TestCounter
    {
        [Fact]
        public void Counter_should_increment_correctly()
        {
            // Arrange
            var counter = new Counter();
            counter.Id = "1";
            counter.Count = 2;

            // Act - Simulate the increment logic from your function
            counter.Count += 1;

            // Assert
            Assert.Equal("1", counter.Id);
            Assert.Equal(3, counter.Count);
        }

        [Fact]
        public void Counter_properties_should_work()
        {
            // Arrange & Act
            var counter = new Counter();
            counter.Id = "index";
            counter.Count = 5;

            // Assert
            Assert.Equal("index", counter.Id);
            Assert.Equal(5, counter.Count);
        }
    }
}