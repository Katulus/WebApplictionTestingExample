using System;

namespace Server
{
    public interface IDateTimeProvider
    {
        DateTime UtcNow { get; }
    }

    class DateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow { get { return DateTime.UtcNow; } }
    }
}