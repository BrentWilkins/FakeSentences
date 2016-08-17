using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakeSentences.Utilities
{
    class SentenceUtils
    {
        /// <summary>
        /// Converts all blocks of white space to a single space.
        /// </summary>
        /// <param name="sourceText">The string object to be simplified.</param>
        /// <returns>Returns the simplified string</returns>
        public static string SimplifyWhiteSpace(string sourceText)
        {
            return System.Text.RegularExpressions.Regex.Replace(sourceText, @"\s+", " ");
        }

        ///<summary
        /// Split source text into sentence based on common sentence terminating characters.
        /// Also converts all white space to single spaces.
        ///</summary>
        /// <param name="sourceText">A string to be split into sentences.</param>
        /// <returns>Returns an array of strings containing the sentences.</returns
        public static string[] SplitSentences(string sourceText)
        {
            char[] sentenceSeparators = { '.', '!', '?' };
            return sourceText.Split(sentenceSeparators, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// Cleans sentences of unwanted characters. In this case all numbers will be removed too.
        /// Example of odd text: " Beau--ootiful Soo--oop"...
        /// TODO: Cleanup more stuff like the above example and the single quotes. So many special cases.
        /// </summary>
        /// <param name="sentence">String containg a sentece to be cleaned up.</param>
        /// <returns>Returns the string with the undesired characters removed.</returns>
        public static string CleanSentence(string sentence)
        {
            // TODO: validate input!
            // @"" - Verbatim string literal. Escape sequences are ignored.
            return System.Text.RegularExpressions.Regex.Replace(sentence, @"[^a-zA-Z ']+", string.Empty);
        }

        /// <summary>
        /// Split a string on the space character.
        /// </summary>
        /// <param name="sentence"></param>
        /// <returns></returns>
        public static string[] SplitSentence(string sentence)
        {
            char[] space = { ' ' };
            return sentence.Split(space, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// Clean up special cases of words found in sample texts
        /// </summary>
        /// <param name="word">Word to be cleaned up.</param>
        /// <returns>Returns cleaned word.</returns>
        public static string CleanWord(string word)
        {
            // string methods such as First don't play well with empty strings
            if (!word.Equals(string.Empty))
            {
                // Remove opening single quote
                // TODO: This doesn't play well with contractions
                if (word.First() == '\'' && word.Length > 1)
                {
                    word = word.Substring(1);
                }
                // Remove closing single quote
                // TODO: This doesn't play well with contractions
                if (word.Last() == '\'' && word.Length > 1)
                {
                    word = word.Substring(0, word.Length - 1);
                }
            }
            return word;
        }
    }
}
