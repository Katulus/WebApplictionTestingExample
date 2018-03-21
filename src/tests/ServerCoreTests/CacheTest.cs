using System;
using FluentAssertions;
using Moq;
using ServerCore;
using Xunit;

namespace ServerCoreTests
{
    public class CacheTest
    {
        private readonly TimeSpan _cacheLifeTime = TimeSpan.FromMinutes(5);
        private readonly Cache<int> _cache;
        private DateTime _now;

        public CacheTest()
        {
            var configurationProviderMock = new Mock<IConfigurationProvider>();
            configurationProviderMock.SetupGet(x => x.CacheLifetime).Returns(_cacheLifeTime);

            _now = DateTime.UtcNow;
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.SetupGet(x => x.UtcNow).Returns(() => _now);

            _cache = new Cache<int>(configurationProviderMock.Object, dateTimeProvider.Object);
        }

        [Fact]
        public void TryGetData_ForEmptyCache_ReturnsFalse()
        {
            bool result = _cache.TryGetData(out _);

            result.Should().BeFalse("TryGetData for empty cache should not return data.");
        }

        [Fact]
        public void TryGetData_ForEmptyCache_ProvidesDefaultData()
        {
            _cache.TryGetData(out var data);

            data.Should().Be(default(int), "TryGetData for empty cache should provide default value for data.");
        }

        [Fact]
        public void TryGetData_ForFullNotExpiredCache_ReturnsTrue()
        {
            _cache.SetData(123);

            bool result = _cache.TryGetData(out _);

            result.Should().BeTrue("TryGetData for full not expired cache should return true.");
        }

        [Fact]
        public void TryGetData_ForFullNotExpiredCache_ProvidesCorrectData()
        {
            _cache.SetData(123);

            _cache.TryGetData(out var data);

            data.Should().Be(123, "TryGetData for full not expired cache should provide cached data.");
        }

        [Fact]
        public void TryGetData_ForExpiredCache_ReturnsFalse()
        {
            _cache.SetData(123);

            // expire cache by moving DateTime forward
            _now = _now.Add(_cacheLifeTime).AddSeconds(1);

            bool result = _cache.TryGetData(out _);

            result.Should().BeFalse("TryGetData for expired cache should return false.");
        }

        [Fact]
        public void TryGetData_ForExpiredCache_ProvidesCorrectData()
        {
            _cache.SetData(123);

            // expire cache by moving DateTime forward
            _now = _now.Add(_cacheLifeTime).AddSeconds(1);

            _cache.TryGetData(out var data);

            data.Should().Be(default(int), "TryGetData for expired cache should provide default value for data.");
        }

        [Fact]
        public void CacheIsNotExpired_UntilLifeTimeIsExceeded()
        {
            _cache.SetData(123);

            // moving forward by lifetime does not yet expires cache, it needs to exceed the lifetime
            _now = _now.Add(_cacheLifeTime);

            bool result = _cache.TryGetData(out _);

            result.Should().BeTrue("Cache should not be expired until lifetime is exceeded");
        }

        [Fact]
        public void SetData_OverridesPreviousValue()
        {
            _cache.SetData(123);
            _cache.SetData(456);

            _cache.TryGetData(out var data);

            data.Should().Be(456, "New cached data should be returned.");
        }

        [Fact]
        public void SetData_ResetsExpirationInterval()
        {
            _cache.SetData(123);
            // expire cache by moving DateTime forward
            _now = _now.Add(_cacheLifeTime).AddSeconds(1);

            // this resets expiration
            _cache.SetData(456);
            // this is still not enough to expire new data
            _now = _now.Add(_cacheLifeTime).AddSeconds(-1);

            _cache.TryGetData(out var data);

            data.Should().Be(456, "New cached data should be returned.");
        }
    }
}
