using FakeSentences.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FakeSentences
{
    public class RootWord : Word
    {
        // Start pseudorandom number generator (PRNG) once
        Random _randomGenerator = new Random(Environment.TickCount);

        public RootWord(string word) : base(word)
        {
        }

        /// <summary>
        /// Process the training data to generate statistics for later generation.
        /// </summary>
        /// <param name="startingWords">A structure to maintain a list of sentence starting words.</param>
        /// <param name="trainingData">A string containg the training text.</param>
        public void TrainOnData(Word startingWords, string trainingData)
        {
            string[] sentences = SentenceUtils.SplitSentences(SentenceUtils.SimplifyWhiteSpace(trainingData));

            //foreach (var sentence in sentences) // TODO: go back to multi threaded
            Parallel.ForEach(sentences, (sentence) =>   // Saves about 9% in single test
            {
                string cleanSentence = SentenceUtils.CleanSentence(sentence);
                string[] words = SentenceUtils.SplitSentence(cleanSentence);
                // Store previous word so we can find it to add a child
                Word previousWord = null;
                foreach (string word in words)
                {
                    // Some sample text generated "'", which isn't a word
                    if (word.Equals("'"))
                    {
                        continue;
                    }
                    string lowerWord = SentenceUtils.CleanWord(word).ToLower();
                    Word currentWord;
                    if (startingWords.Children.ContainsKey(lowerWord))   // TODO: BUG: This likely captures repeated words
                    {
                        currentWord = new Word(string.Empty);
                        startingWords.Children.TryGetValue(lowerWord, out currentWord);
                    }
                    else
                    {
                        currentWord = new Word(lowerWord);
                    }

                    // TODO: This test is likely invalid in cases of repeated words in a sentence
                    if (words.First() == word)
                    {
                        // This is a valid word to start a sentence with
                        previousWord = startingWords.AddChild(ref currentWord);
                    }
                    else
                    {
                        previousWord = previousWord?.AddChild(ref currentWord);
                    }
                }
            });
            //foreach (KeyValuePair<string, int> kvp in allWords)
            //{
            //    Console.WriteLine($"Key = {kvp.Key}, Value = {kvp.Value}");
            //}
            Console.WriteLine("Done processing training data");
        }

        /// <summary>
        /// Generate one or more pseudo-random sentences.
        /// </summary>
        /// <param name="numSentences">Number of sentences to generate. Defaults to one of not specified.</param>
        public string GenerateSentence(uint numSentences = 1)
        {
            var sortedArray = Children.OrderByDescending(sw => sw.Value.Count).ToArray();
            if (sortedArray.Length >= 5)
            {
                Console.WriteLine("Top 5 most common sentence starting words:");
                for (int i = 0; i < 5; i++)
                {
                    Console.WriteLine($"  {i + 1}: {sortedArray[i].Value.Text,-15} ({sortedArray[i].Value.Count})");
                }
            }

            string returnString = string.Empty;
            for (uint i = 0; i < numSentences; i++)
            {
                Word word = GetRandomElement(sortedArray);
                // Add first word with first char uppercased.
                returnString += word.Text.First().ToString().ToUpper() + word.Text.Substring(1);
                // TODO: Doesn't handle cases where words sometimes end sentences
                while (word.Children.Count != 0)
                {
                    word = GetRandomElement(word.Children.OrderByDescending(sw => sw.Value.Count).ToArray());
                    returnString += " " + word.Text;
                }
                // Found a leaf, add a period to end sentence.
                // TODO: Add more punctuation options and remove extra space
                returnString += ". ";
            }
            return returnString;
        }

        /// <summary>
        /// Returns a pseudorandom, but weighted Word from a sortedArray of them.
        /// </summary>
        /// <param name="sortedArray">Sorted array of Words.</param>
        /// <returns>Returns instance of Word.</returns>
        private Word GetRandomElement(KeyValuePair<string, Word>[] sortedArray)
        {
            long totalNumberWords = sortedArray.Sum(word => word.Value.Count);

            // Calculate probability of each word
            foreach (KeyValuePair<string, Word> word in sortedArray)
            {
                word.Value.Probability = word.Value.Count / (double)totalNumberWords;
                //Console.WriteLine($"Probablitlity of {word.Value.Text} is {word.Value.Probability}");
            }

            double randSum = 0, rand = _randomGenerator.NextDouble();
            Word chosenWord = sortedArray[0].Value; // Default to most probable
            foreach (KeyValuePair<string, Word> word in sortedArray)
            {
                randSum += word.Value.Probability;
                if (rand <= randSum)
                {
                    chosenWord = word.Value;
                    break;
                }
            }
            return chosenWord;
        }
    }
}
