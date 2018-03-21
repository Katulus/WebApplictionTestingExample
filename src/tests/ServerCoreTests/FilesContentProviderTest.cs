using System;
using System.Collections.Generic;
using System.IO;
using FluentAssertions;
using ServerCore;
using Xunit;

namespace ServerCoreTests
{
    // this test fixture is using real file system
    public class FilesContentProviderTest : IDisposable
    {
        private const string TestDirectoryName = "AddNodeWizardTestDirectory";
        private readonly string _path;
        private readonly FilesContentProvider _provider;

        public FilesContentProviderTest()
        {
            _path = Path.Combine(Path.GetTempPath(), TestDirectoryName);
            Directory.CreateDirectory(_path);

            _provider = new FilesContentProvider();
        }

        public void Dispose()
        {
            // clear test plugin files created during test if there are any
            Directory.Delete(Path.Combine(Path.GetTempPath(), TestDirectoryName), true);
        }

        [Fact]
        public void GetFilesContent_NoFiles_ReturnsEmptyCollection()
        {
            _provider.GetFilesContent(_path, "*.*").Should().BeEmpty("No files contents should have been returned");
        }

        [Fact]
        public void GetFilesContent_InvalidPath_ReturnsEmptyCollection()
        {
            _provider.GetFilesContent("invalid_path", "*.*").Should().BeEmpty("No plugins should have been returned");
        }

        // Theory is powerful feature if you need to test different combinations of data in the same scenario
        [Theory]
        [MemberData(nameof(TestData))]
        public void GetFilesContent_ReturnsValidFilesContents(string[] filesToCreate, string searchPattern, string[] expectedResults)
        {
            CreateTestFiles(filesToCreate);

            IEnumerable<string> result = _provider.GetFilesContent(_path, searchPattern);

            result.Should().BeEquivalentTo(expectedResults);
            
            // TODO: Complex assertion without FluentAssertions library
            //Assert.Equal(result.Count(), expectedResults.Length);
            //foreach (string expectedResult in expectedResults)
            //{
            //    Assert.True(result.Any(x => x == expectedResult), $"{expectedResult} content was not returned");
            //}
        }

        private void CreateTestFiles(params string[] fileNames)
        {
            foreach (var fileName in fileNames)
            {
                File.WriteAllText(Path.Combine(_path, fileName), fileName + "_content");
            }
        }

        public static IEnumerable<object[]> TestData
        {
            get
            {
                // TODO: Show NUnit TestCaseData - much nicer than this
                yield return new object[] {
                    new[] {"file1.txt", "file2.txt", "file3.txt"},
                    "*.*",
                    new[] {"file1.txt_content", "file2.txt_content", "file3.txt_content"}
                };
                yield return new object[] {
                    new[] {"file1.txt", "file2.dat", "file3.txt"},
                    "*.txt",
                    new[] {"file1.txt_content", "file3.txt_content"}
                };
                yield return new object[] {
                    new[] {"file1.txt", "file2.dat", "file2.txt", "file3.xml"},
                    "file2.*",
                    new[] {"file2.dat_content", "file2.txt_content"}
                };
            }
        }
    }
}
