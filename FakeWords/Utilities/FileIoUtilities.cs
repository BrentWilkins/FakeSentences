using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakeWords.Utilities
{
    public static class FileIoUtilities
    {
        /// <summary>
        /// Reads the file with the name provided in the parameter if possible.
        /// </summary>
        /// <param name="filename">Name including path of file to read.</param>
        /// <returns>Returns the text read, or null on failure or empty file.</returns>
        public static string ReadTextFile(string filename)
        {
            string retVal = null;
            try
            {
                if (File.Exists(filename))
                {
                    using (StreamReader reader = File.OpenText(filename))
                    {
                        retVal = reader.ReadToEnd();

                        Console.WriteLine("Whole file was read!");
                    }
                }
                else
                {
                    Console.WriteLine($"File '{filename}' does not exist");
                }
            }
            catch (OutOfMemoryException ex)    // We might try and read a file that is too large
            {
                Console.WriteLine($"Not enough memory to read all of {filename}");
                Console.WriteLine(ex.Message);
            }
            return retVal;
        }
    }
}
