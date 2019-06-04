 
namespace Eisk.Helpers
{
    using System;
    using System.IO;
    using System.Web;
    using System.Web.UI.HtmlControls;
    /// <summary>
    /// Contains necessary utility methods to facilitate working on file and file paths. 
    /// </summary>
    public sealed class Filer
    {
        public static string ReadFromFile(string filePath)
        {
            FileStream fStream;

            // Reading the file content.
            fStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Read);
            StreamReader sReader = new StreamReader(fStream);
            string line = sReader.ReadToEnd();
            sReader.Close();

            return line;
        }

        public static byte[] GetBytesFromStream(Stream stream)
        {
            int size = Convert.ToInt32(stream.Length);
            byte[] bytes = new byte[size];
            stream.Read(bytes, 0, size);
            return bytes;
        }
    }
}
