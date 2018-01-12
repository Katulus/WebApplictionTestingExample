using System;
using Moq;
using NUnit.Framework;
using Server;

namespace ServerTests
{
    [TestFixture]
    public class CacheTest
    {
        private Mock<IConfigurationProvider> _configurationProviderMock;
        private Mock<IDateTimeProvider> _dateTimeProvider;
        private readonly TimeSpan _cacheLifeTime = TimeSpan.FromMinutes(5);
        private Cache<int> _cache;
        private DateTime _now;

        [SetUp]
        public void SetUp()
        {
            _configurationProviderMock = new Mock<IConfigurationProvider>();
            _configurationProviderMock.SetupGet(x => x.CacheLifetime).Returns(_cacheLifeTime);

            _now = DateTime.UtcNow;
            _dateTimeProvider = new Mock<IDateTimeProvider>();
            _dateTimeProvider.SetupGet(x => x.UtcNow).Returns(() => _now);

            _cache = new Cache<int>(_configurationProviderMock.Object, _dateTimeProvider.Object);
        }

        [Test]
        public void TryGetData_ForEmptyCache_ReturnsFalse()
        {
            int data;
            bool result = _cache.TryGetData(out data);

            Assert.That(result, Is.False, "TryGetData for empty cache should not return data.");
        }

        [Test]
        public void TryGetData_ForEmptyCache_ProvidesDefaultData()
        {
            int data;
            _cache.TryGetData(out data);

            Assert.That(data, Is.EqualTo(default(int)), "TryGetData for empty cache should provide default value for data.");
        }

        [Test]
        public void TryGetData_ForFullNotExpiredCache_ReturnsTrue()
        {
            _cache.SetData(123);

            int data;
            bool result = _cache.TryGetData(out data);

            Assert.That(result, Is.True, "TryGetData for full not expired cache should return true.");
        }

        [Test]
        public void TryGetData_ForFullNotExpiredCache_ProvidesCorrectData()
        {
            _cache.SetData(123);

            int data;
            _cache.TryGetData(out data);

            Assert.That(data, Is.EqualTo(123), "TryGetData for full not expired cache should provide cached data.");
        }

        [Test]
        public void TryGetData_ForExpiredCache_ReturnsFalse()
        {
            _cache.SetData(123);

            // expire cache by moving DateTime forward
            _now = _now.Add(_cacheLifeTime).AddSeconds(1);

            int data;
            bool result = _cache.TryGetData(out data);

            Assert.That(result, Is.False, "TryGetData for expired cache should return false.");
        }

        [Test]
        public void TryGetData_ForExpiredCache_ProvidesCorrectData()
        {
            _cache.SetData(123);

            // expire cache by moving DateTime forward
            _now = _now.Add(_cacheLifeTime).AddSeconds(1);

            int data;
            _cache.TryGetData(out data);

            Assert.That(data, Is.EqualTo(default(int)), "TryGetData for expired cache should provide default value for data.");
        }

        [Test]
        public void CacheIsNotExpired_UntilLifeTimeIsExceeded()
        {
            _cache.SetData(123);

            // moving forward by lifetime does not yet expires cache, it needs to exceed the lifetime
            _now = _now.Add(_cacheLifeTime);

            int data;
            bool result = _cache.TryGetData(out data);

            Assert.That(result, Is.True, "Cache should not be expired until lifetime is exceeded");
        }

        [Test]
        public void SetData_OverridesPreviousValue()
        {
            _cache.SetData(123);
            _cache.SetData(456);

            int data;
            _cache.TryGetData(out data);

            Assert.That(data, Is.EqualTo(456), "New cached data should be returned.");
        }

        [Test]
        public void SetData_ResetsExpirationInterval()
        {
            _cache.SetData(123);
            // expire cache by moving DateTime forward
            _now = _now.Add(_cacheLifeTime).AddSeconds(1);

            // this resets expiration
            _cache.SetData(456);
            // this is still not enough to expire new data
            _now = _now.Add(_cacheLifeTime).AddSeconds(-1);

            int data;
            _cache.TryGetData(out data);

            Assert.That(data, Is.EqualTo(456), "New cached data should be returned.");
        }
    }
}
