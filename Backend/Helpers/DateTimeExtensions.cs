public static class DateTimeExtensions
{
    public static (DateTime StartOfWeek, DateTime EndOfWeek) GetCurrentWeek(this DateTime date)
    {
        int daysSinceStart = ((int)date.DayOfWeek - (int)DayOfWeek.Monday + 7) % 7;
        DateTime startOfWeek = date.AddDays(-daysSinceStart).Date;
        DateTime endOfWeek = startOfWeek.AddDays(6).Date;
        return (startOfWeek, endOfWeek);
    }
}