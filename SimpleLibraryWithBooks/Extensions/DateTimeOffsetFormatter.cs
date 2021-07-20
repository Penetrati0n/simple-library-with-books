using System;

namespace SimpleLibraryWithBooks.Extensions
{
    public static class DateTimeOffsetFormatter
    {
        public static DateTimeOffset SubstringTicks(this DateTimeOffset dateTime)
        {
            var ticks = dateTime.Ticks;
            var resultTime = new DateTimeOffset(ticks - ticks % 10000, dateTime.Offset);

            return resultTime;
        }

        public static DateTimeOffset ChangeTimeZone(this DateTimeOffset dateTime, double hourOffset = 4)
        {
            var offset = TimeSpan.FromHours(hourOffset);

            return dateTime.ToOffset(offset);
        }
    }
}
