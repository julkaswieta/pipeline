using Pipeline;

namespace Testing
{
    public class UnitTest1
    {

        TestService testService = new TestService();

        [Fact]
        public void PluralCheck_NotPlural()
        {
            Assert.Equal("Clicked 1 time", testService.PluralChecker(1));
        }

        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void PluralCheck_IsPlural(int count)
        {
            Assert.Equal($"Clicked {count} times", testService.PluralChecker(count));
        }

        [Fact]
        public void GetBooks_ReturnsBooks()
        {
            List<Book> testBooks = testService.GetBooks();

            Assert.True(testBooks.Count() >= 1);
        }

    }
}