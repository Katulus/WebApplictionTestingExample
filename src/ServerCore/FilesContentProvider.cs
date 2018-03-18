using System;
using System.Collections.Generic;
using System.IO;

namespace ServerCore
{
    public interface IFilesContentProvider
    {
        IEnumerable<string> GetFilesContent(string directoryPath, string searchPattern);
    }

    public class FilesContentProvider : IFilesContentProvider
    {
        public IEnumerable<string> GetFilesContent(string directoryPath, string searchPattern)
        {
            if (directoryPath == null)
                throw new ArgumentNullException(nameof(directoryPath));
            if (searchPattern == null)
                throw new ArgumentNullException(nameof(searchPattern));

            if (!Directory.Exists(directoryPath))
                yield break;

            string[] pluginPaths = Directory.GetFiles(directoryPath, searchPattern);
            foreach (var pluginPath in pluginPaths)
            {
                yield return File.ReadAllText(pluginPath);
            }
        }
    }
}