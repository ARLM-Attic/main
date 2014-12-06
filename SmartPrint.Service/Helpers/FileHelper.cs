using System.IO;

namespace SmartPrint.Service.Helpers
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
    }
}