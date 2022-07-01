namespace InterviewExample;

//Given current day as day of the week and an integer K, the task is to find the day of the week after K days.
internal class DayOfTheWeek
{
    internal static string GetDayOfTheWeek(string day, int numOfDays)
    {
        var days = new string[] { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };

        var dayIndex = Array.IndexOf(days, day);

        var index = (dayIndex + numOfDays) % 7;

        if (index < 0)
        {
            return days[index + 7];
        }

        return days[index];
    }
}
