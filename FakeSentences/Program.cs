using FakeSentences.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace FakeSentences
{
    class Program
    {
        static void Main(string[] args)
        {
            var startingWords = new RootWord("fake");

            var filename = PromptUserForFilename();
            var fileContents = FileIoUtilities.ReadTextFile(filename);

            if (string.IsNullOrEmpty(fileContents))
            {
                Console.WriteLine("Didn't read any training data. Cannot generate words.");
                return;
            }
            else
            {
                // TODO: These might have made good unit tests
                //startingWords.TrainOnData(startingWords, "'Shan't,' said the cook.");
                //startingWords.TrainOnData(startingWords, "'Yes, please do!' pleaded Alice");

                startingWords.TrainOnData(startingWords, fileContents);
                //foreach (KeyValuePair<string, Word> kvp in startingWords.Children)
                //{
                //    Console.WriteLine($"Key = {kvp.Key}, Value = {kvp.Value.Count}");
                //}
            }

            Console.WriteLine(startingWords.GenerateSentence(5));
        }

        /// <summary>
        /// Prompts user to enter the [path]name of a text file.
        /// </summary>
        /// <returns>A string which is hopefully the name of a text file.</returns>
        private static string PromptUserForFilename()
        {
            Console.WriteLine("Enter name of text file to read");
            string filename = Console.ReadLine();
            Console.WriteLine($"You entered '{filename}', attempting read of that file");
            return filename;
        }
    }
}
