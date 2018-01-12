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
        private bool _dataSet = false;

        public Cache(IConfigurationProvider configuration, IDateTimeProvider dateTimeProvider)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));
            if (dateTimeProvider == null)
                throw new ArgumentNullException(nameof(dateTimeProvider));

            _maxCacheLifetime = configuration.CacheLifetime;
            _dateTimeProvider = dateTimeProvider;
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

        private bool IsExpired
        {
            get { return _dateTimeProvider.UtcNow - _dataCreationDateTime > _maxCacheLifetime; }
        }
    }
}