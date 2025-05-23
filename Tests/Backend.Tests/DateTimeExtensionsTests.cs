using System;
using Xunit;

public class DateTimeExtensionsTests
{
    [Fact]
    public void GetCurrentWeek_ShouldReturnCorrectStartAndEnd_ForAllDays()
    {
        for (int i = 0; i < 7; i++)
        {
            DateTime testDate = new DateTime(2025, 5, 19).AddDays(i); // Monday, May 19, 2025
            var (startOfWeek, endOfWeek) = testDate.GetCurrentWeek();

            Assert.Equal(DayOfWeek.Monday, startOfWeek.DayOfWeek);
            Assert.Equal(DayOfWeek.Sunday, endOfWeek.DayOfWeek);
            Assert.Equal(startOfWeek.AddDays(6), endOfWeek);
        }
    }
}
