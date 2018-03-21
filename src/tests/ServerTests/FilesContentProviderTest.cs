using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Server;

namespace ServerTests
{
    [TestFixture]
    // this test fixture is using real file system
    class FilesContentProviderTest
    {
        private const string TestDirectoryName = "AddNodeWizardTestDirectory";
        private string _path;
        private FilesContentProvider _provider;

        [SetUp]
        public void SetUp()
        {
            _path = Path.Combine(Path.GetTempPath(), TestDirectoryName);
            Directory.CreateDirectory(_path);

            _provider = new FilesContentProvider();
        }

        [TearDown]
        public void TearDown()
        {
            // clear test plugin files created during test if there are any
            Directory.Delete(Path.Combine(Path.GetTempPath(), TestDirectoryName), true);
        }

        [Test]
        public void GetFilesContent_NoFiles_ReturnsEmptyCollection()
        {
            CollectionAssert.IsEmpty(_provider.GetFilesContent(_path, "*.*"), "No files contents should have been returned");
        }

        [Test]
        public void GetFilesContent_InvalidPath_ReturnsEmptyCollection()
        {
            CollectionAssert.IsEmpty(_provider.GetFilesContent("invalid_path", "*.*"), "No plugins should have been returned");
        }

        // TestCaseSource is powerful feature if you need to test different combinations of data in the same scenario
        [Test]
        [TestCaseSource("GetTestData")]
        public void GetFilesContent_ReturnsValidFilesContents(string[] filesToCreate, string searchPattern, string[] expectedResults)
        {
            CreateTestFiles(filesToCreate);

            IEnumerable<string> result = _provider.GetFilesContent(_path, searchPattern);

            Assert.That(result.Count(), Is.EqualTo(expectedResults.Length), "Wrong number of results returned");
            foreach (string expectedResult in expectedResults)
            {
                Assert.That(result.Any(x => x == expectedResult), Is.True, "{0} content was not returned", expectedResult);
            }
        }

        private void CreateTestFiles(params string[] fileNames)
        {
            foreach (var fileName in fileNames)
            {
                File.WriteAllText(Path.Combine(_path, fileName), fileName + "_content");
            }
        }

        private static IEnumerable GetTestData()
        {
            yield return new TestCaseData(
                new[] {"file1.txt", "file2.txt", "file3.txt"},
                "*.*",
                new[] {"file1.txt_content", "file2.txt_content", "file3.txt_content"}
                ).SetName("all files");
            yield return new TestCaseData(
                new[] { "file1.txt", "file2.dat", "file3.txt" },
                "*.txt",
                new[] { "file1.txt_content", "file3.txt_content" }
                ).SetName("search pattern *.txt");
            yield return new TestCaseData(
                new[] { "file1.txt", "file2.dat", "file2.txt", "file3.xml" },
                "file2.*",
                new[] { "file2.dat_content", "file2.txt_content" }
                ).SetName("search pattern file2.*");
        }
    }
}
