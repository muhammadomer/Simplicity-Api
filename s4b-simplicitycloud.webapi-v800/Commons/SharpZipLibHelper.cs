using System;
using ICSharpCode.SharpZipLib.Zip;
using System.IO;

namespace SimplicityOnlineWebApi.Commons
{
    public static class SharpZipLibHelper
    {
        public static string Message { get; set; }
        public static void ZipFolder(string folderPath, ZipOutputStream zipStream)
        {
            string path = !folderPath.EndsWith("\\") ? string.Concat(folderPath, "\\") : folderPath;
            ZipFolder(path, path, zipStream);
        }

        private static void ZipFolder(string RootFolder, string CurrentFolder,
            ZipOutputStream zipStream)
        {
            const string METHO_NAME = "SharpZipLibHelper.ZipFolder()";
            try
            {
                string[] SubFolders = Directory.GetDirectories(CurrentFolder);
                foreach (string Folder in SubFolders)
                {
                    ZipFolder(RootFolder, Folder, zipStream);
                }
                string relativePath = string.Concat(CurrentFolder.Substring(RootFolder.Length), "/");
                if (relativePath.Length > 1)
                {
                    ZipEntry dirEntry;
                    dirEntry = new ZipEntry(relativePath);
                    dirEntry.DateTime = DateTime.Now;

                }
                foreach (string file in Directory.GetFiles(CurrentFolder))
                {
                    AddFileToZip(zipStream, relativePath, file);
                }
            }
            catch(Exception ex)
            {
                Message = Utilities.GenerateAndLogMessage(METHO_NAME, "Unable to Zip Folder '" + RootFolder + "' and " + CurrentFolder, ex);
            }
        }

        private static void AddFileToZip(ZipOutputStream zStream, string relativePath, string file)
        {
            const string METHO_NAME = "SharpZipLibHelper.ZipFolder()";
            try
            {
                byte[] buffer = new byte[4096];
                string fileRelativePath = string.Concat((relativePath.Length > 1 ? relativePath : string.Empty), Path.GetFileName(file));

                ZipEntry entry = new ZipEntry(fileRelativePath);
                entry.DateTime = DateTime.Now;
                zStream.PutNextEntry(entry);

                using (FileStream fs = File.OpenRead(file))
                {
                    int sourceBytes;
                    do
                    {
                        sourceBytes = fs.Read(buffer, 0, buffer.Length);
                        zStream.Write(buffer, 0, sourceBytes);
                    } while (sourceBytes > 0);
                }
            }
            catch(Exception ex)
            {
                Message = Utilities.GenerateAndLogMessage(METHO_NAME, "Unable to Add File to Zip Path '" + relativePath + "' and File '" + file + "'", ex);
            }
        }
    }
}
