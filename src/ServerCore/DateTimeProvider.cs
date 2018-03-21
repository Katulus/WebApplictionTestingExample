using System;

namespace ServerCore
{
    public interface IDateTimeProvider
    {
        DateTime UtcNow { get; }
    }

    class DateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}