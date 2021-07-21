using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetMailBYEAGetMailDesktopAPP.Common
{
    /// <summary>
    /// Create this class to make txt file and write exception details
    /// </summary>
    class WriteToFile
    {
        public void WriteExceptionToFile(string Message, string serverPath, string fileName)
        {
            string path = serverPath + "\\Exceptions";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filepath = serverPath + "\\Exceptions\\" + fileName + ".txt";
            if (!File.Exists(filepath))
            {
                // Create a file to write to.   
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
        }
    }
}
