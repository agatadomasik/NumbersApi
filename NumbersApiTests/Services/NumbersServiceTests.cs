using NumbersApi.Services;

namespace NumbersApiTests.Services
{
    public class NumbersServiceTests
    {
        private readonly NumbersService _service;

        public NumbersServiceTests()
        {
            _service = new NumbersService();
        }

        [Fact]
        public void AddNumbers_ShouldAddNumbersCorrectly()
        {
            // Arrange
            var numbers = new List<int> { 1, 2, 3 };

            // Act
            _service.AddNumbers(numbers);
            var result = _service.GetAllNumbers();

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Contains(1, result);
            Assert.Contains(2, result);
            Assert.Contains(3, result);
        }

        [Fact]
        public void GetAllNumbers_ShouldReturnCorrectNumbers()
        {
            // Arrange
            var numbers = new List<int> { 1, 2, 3 };
            _service.AddNumbers(numbers);

            // Act
            var result = _service.GetAllNumbers();

            // Assert
            Assert.Equal(numbers, result);
        }

        [Theory]
        [InlineData("asc", new[] { 1, 2, 3, 4 })]
        [InlineData("desc", new[] { 4, 3, 2, 1 })]
        [InlineData("other", new[] { 1, 2, 3, 4 })]
        public void GetSortedNumbers_ShouldSortCorrectly(string order, int[] expected)
        {
            // Arrange
            _service.AddNumbers(new[] { 2, 4, 3, 1 });

            // Act
            var result = _service.GetSortedNumbers(order);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void SearchNumber_ShouldReturnFoundTrue_WhenNumberExists()
        {
            // Arrange
            _service.AddNumbers(new[] { 1, 2, 3 });

            // Act
            var response = _service.SearchNumber(2);

            // Assert
            Assert.True(response.Found);
            Assert.Equal(2, response.Value);
        }

        [Fact]
        public void SearchNumber_ShouldReturnFoundFalse_WhenNumberNotExists()
        {
            // Arrange
            _service.AddNumbers(new[] { 1, 2, 3 });

            // Act
            var response = _service.SearchNumber(4);

            // Assert
            Assert.False(response.Found);
            Assert.Equal(4, response.Value);
        }

        [Fact]
        public void GetStatistics_ShouldReturnZeros_WhenEmpty()
        {
            // Act
            var stats = _service.GetStatistics();

            // Assert
            Assert.Equal(0, stats.Average);
            Assert.Equal(0, stats.Median);
        }

        [Fact]
        public void GetStatistics_ShouldReturnCorrectStats()
        {
            // Arrange
            _service.AddNumbers(new[] { 1, 2, 3, 4 });

            // Act
            var stats = _service.GetStatistics();

            // Assert
            Assert.Equal(2.5, stats.Average);
            Assert.Equal(2.5, stats.Median);
        }

        [Fact]
        public async Task ProcessNumbersParallelAsync_ShouldReturnCorrectStats()
        {
            // Arrange
            _service.AddNumbers(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });

            // Act
            var response = await _service.ProcessNumbersParallelAsync();

            // Assert
            Assert.Equal(10, response.Count);
            Assert.Equal(55, response.Sum);
            Assert.Equal(5.5, response.Average);
        }

        [Fact]
        public async Task ProcessNumbersParallelAsync_ShouldReturnZeros_WhenEmpty()
        {
            // Act
            var response = await _service.ProcessNumbersParallelAsync();

            // Assert
            Assert.Equal(0, response.Count);
            Assert.Equal(0, response.Sum);
            Assert.Equal(0, response.Average);
        }
    }
}
