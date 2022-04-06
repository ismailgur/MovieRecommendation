using Project.Business.MovieIntegration;
using System;
using Xunit;

namespace Project.UnitTest
{
    public class UnitTest1
    {
        [Theory]
        [InlineData(1, 20)]
        public void Test1(int page, int expected)
        {
            // Bu testte api den d�nen liste adedinin 20 olmas� bekleniyor.

            MovieAPI api = new MovieAPI("https://api.themoviedb.org/3", "23818476598777d4bf155e9500fefb82");

            var data = api.GetUpcomingList(page, out int totalResultCount);


            Assert.Equal(expected, data.Count);
        }
    }
}
