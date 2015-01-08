using System;
using System.IO;

namespace SmartPrint.Model.Helpers
{
    public static class FileHelper
    {
        public static bool IsFileLocked(string filename)
        {
            FileStream stream = null;

            FileInfo file = new FileInfo(filename);
            
            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }
            
            return false;
        }

        public static string ExtractFilename(string filepath)
        {
            if (filepath.Trim().EndsWith(@"\"))
                return String.Empty;

            int position = filepath.LastIndexOf('\\');
            
            if (position == -1)
            {
                if (File.Exists(Environment.CurrentDirectory + Path.DirectorySeparatorChar + filepath))
                    return filepath;
                
                return String.Empty;
            }

            if (File.Exists(filepath))
                return filepath.Substring(position + 1);
            
            return String.Empty;
        }
    }
}