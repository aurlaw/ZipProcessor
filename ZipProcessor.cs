using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STZipProcessor
{
    public class ZipProcessor
    {
        private readonly string _zipFile;
        private readonly string _zipPath;
        private readonly string _zipFileName;
        private readonly string _tempPath;

        public string EntryArchiveName { get; private set; }
        public string NewArchivePath { get; set; }

        public ZipProcessor(string zipFile, string entryArchiveName = null)
        {
            if(string.IsNullOrEmpty(zipFile))
                throw  new ArgumentNullException("zipFile");
            if(!File.Exists(zipFile))
                throw  new FileNotFoundException("zipFile was not found on file system");

            _zipFile = zipFile;
            _zipPath = Path.GetDirectoryName(_zipFile);
            _zipFileName = Path.GetFileName(_zipFile);
            _tempPath = Path.Combine(_zipPath, "tmp");
            if(!string.IsNullOrEmpty(entryArchiveName))
                EntryArchiveName = entryArchiveName;
        }


        private void CreateTmpDir()
        {
            DeleteTempDir();
            Directory.CreateDirectory(_tempPath);
        }

        private void DeleteTempDir()
        {
            if(Directory.Exists(_tempPath))
                Directory.Delete(_tempPath, true);
        }

        private void ExtractArchive()
        {
            ZipFile.ExtractToDirectory(_zipFile, _tempPath);
        }

        private void BatchArchiveEntries()
        {
            if(string.IsNullOrEmpty(EntryArchiveName))
                throw  new NullReferenceException("EntryArchiveName was not provided!");
            // get files from zip dir and perform organization/rename
            EntryArchiveName = Path.GetFileNameWithoutExtension(EntryArchiveName);
            var zipTmpRoot = _tempPath;
            var tDirs = Directory.GetDirectories(_tempPath);
            if (tDirs.Any())
            {
                zipTmpRoot = tDirs.FirstOrDefault();
            }
            foreach (var file in Directory.GetFiles(zipTmpRoot))
            {
                var absFile = file;
                var fileName = Path.GetFileNameWithoutExtension(file);
                var fileExt = Path.GetExtension(file);
                if (string.IsNullOrEmpty(fileName))
                    continue;
                var productId = fileName.Substring(fileName.LastIndexOf("_") + 1);
                Console.WriteLine("Name: {0} Ext: {1} Product Id: {2}", fileName, fileExt, productId);
                // create dir with prod id
                var prodDir = Path.Combine(zipTmpRoot, productId);
                if (!Directory.Exists(prodDir))
                    Directory.CreateDirectory(prodDir);
                // move and rename file to prod id dir
                var newFilePath = Path.Combine(prodDir, string.Concat(EntryArchiveName, fileExt));
                if (File.Exists(newFilePath))
                    File.Delete(newFilePath);
                File.Move(absFile, newFilePath);
            }             
            CreateNewArchive();
        }

        private void CreateNewArchive()
        {
            var newArchive = string.Concat(DateTime.Now.ToString("yyyyMMddHHmmss"), "_",
                Path.GetFileNameWithoutExtension(_zipFileName), ".zip");

            NewArchivePath = Path.Combine(_zipPath, newArchive);

            ZipFile.CreateFromDirectory(_tempPath, NewArchivePath);
            DeleteTempDir();
        }

        #region Public
        public void Process(Func<ZipProcessor, string> promptEntryName)
        {
            CreateTmpDir();
            ExtractArchive();

            if (promptEntryName != null)
            {
               EntryArchiveName =  promptEntryName(this);
            }
            BatchArchiveEntries();
        }

        #endregion

    }
}
