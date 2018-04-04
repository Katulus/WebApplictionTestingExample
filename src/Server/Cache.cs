using System;

namespace Server
{
    public interface ICache<T>
    {
        void SetData(T data);
        bool TryGetData(out T data);
    }

    public class Cache<T> : ICache<T>
    {
        private readonly TimeSpan _maxCacheLifetime;
        private readonly IDateTimeProvider _dateTimeProvider;
        private DateTime _dataCreationDateTime;
        private T _data;
        private bool _dataSet;

        public Cache(Configuration configuration, IDateTimeProvider dateTimeProvider)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            _maxCacheLifetime = configuration.CacheLifetime;
            _dateTimeProvider = dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));
        }

        public void SetData(T data)
        {
            _dataCreationDateTime = _dateTimeProvider.UtcNow;
            _data = data;
            _dataSet = true;
        }

        public bool TryGetData(out T data)
        {
            data = default(T);

            if (!_dataSet || IsExpired)
            {
                return false;
            }

            data = _data;
            return true;
        }

        private bool IsExpired => _dateTimeProvider.UtcNow - _dataCreationDateTime > _maxCacheLifetime;
    }
}