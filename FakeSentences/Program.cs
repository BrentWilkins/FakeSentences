using FakeSentences.Utilities;

namespace FakeSentences
{
    class Program
    {
        static void Main(string[] args)
        {
            var startingWords = new RootWord("fake");

            int filesLoaded = 0;
            foreach (string filename in PromptUserForFilenames())
            {
                var fileContents = FileIoUtilities.ReadTextFile(filename);
                if (!string.IsNullOrEmpty(fileContents))
                {
                    startingWords.TrainOnData(startingWords, fileContents);
                    filesLoaded++;
                }
            }

            if (filesLoaded == 0)
            {
                Console.WriteLine("Didn't read any training data. Cannot generate words.");
                return;
            }

            Console.WriteLine(startingWords.GenerateSentence(5));
        }

        /// <summary>
        /// Prompts user to enter one or more text file paths, one per line.
        /// An empty line signals the end of input.
        /// </summary>
        /// <returns>Each filename entered by the user.</returns>
        private static IEnumerable<string> PromptUserForFilenames()
        {
            Console.WriteLine("Enter training files one per line, then press Enter to start:");
            string? filename;
            while (!string.IsNullOrWhiteSpace(filename = Console.ReadLine()))
            {
                Console.WriteLine($"  -> '{filename}'");
                yield return filename;
            }
        }
    }
}
