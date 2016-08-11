using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakeWords
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter name of text file to read");
            string fileName = Console.ReadLine();
            Console.WriteLine($"You entered {fileName}, reading that file");

            try
            {
                using (StreamReader reader = File.OpenText(fileName))
                {
                    string text = reader.ReadToEnd();

                    Console.WriteLine("Whole file was read!");
                    // Do stuff with the text here
                }
            }
            catch (OutOfMemoryException ex)    // We might try and read a file that is too large
            {
                Console.WriteLine($"Not enough memory to read all of {fileName}");
                Console.WriteLine(ex.Message);
                //Parallel.For(0, 12, x =>
                //{
                //    // Do stuff
                //});
            }

            MarkovChain chain = new MarkovChain();
            chain.AddTransition(null, null);
        }
    }
}
