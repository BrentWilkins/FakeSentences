using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FakeWords;

namespace FakeWords.Tests
{
    [TestClass]
    public class WordTests
    {
        /// <summary>
        /// Adding a a child word with the same text twice should only increment the count,
        /// not add a second child. The ability of a word to be a leaf node is as follows:
        /// First word  | Second word | Combined
        /// ------------------------------------
        /// IsLeaf      | IsLeaf      | IsLeaf
        /// IsNotLeaf   | IsLeaf      | IsMaybeLeaf
        /// IsMaybeLeaf | IsLeaf      | IsMaybeLeaf
        /// IsLeaf      | IsNotLeaf   | IsMaybeLeaf
        /// IsNotLeaf   | IsNotLeaf   | IsNotLeaf
        /// IsMaybeLeaf | IsNotLeaf   | IsMaybeLeaf
        /// IsLeaf      | IsMaybeLeaf | IsMaybeLeaf
        /// IsNotLeaf   | IsMaybeLeaf | IsMaybeLeaf
        /// IsMaybeLeaf | IsMaybeLeaf | IsMaybeLeaf
        /// </summary>
        [TestMethod]
        public void AddSameWordTwiceBothLeaves()
        {
            Word root = new Word("root", Word.Leaf.IsNotLeaf);  // Cannot be a leaf and have children
            Word testWord  = new Word("word", Word.Leaf.IsLeaf);
            Word testWord2 = new Word("word", Word.Leaf.IsLeaf);

            root.AddChild(ref testWord);
            root.AddChild(ref testWord2);

            // Only added children to the root, so count must be one
            Assert.AreEqual(root.Count, 1);
            // Word had children and thus cannot be a leaf
            Assert.IsFalse(root.IsLeaf.Equals(Word.Leaf.IsLeaf));
            // Added same word twice, so there should only be one child
            Assert.AreEqual(root.Children.Count, 1);
            // That one child should have a count of two
            Word result;
            root.Children.TryGetValue("word", out result);
            Assert.AreEqual(result.Count, 2);
            // The children should combine to still being a leaf node
            Assert.IsTrue(result.IsLeaf.Equals(Word.Leaf.IsLeaf));
        }

        [TestMethod]
        public void AddSameWordTwiceOneLeafOneNot()
        {
            Word root = new Word("root", Word.Leaf.IsNotLeaf);  // Cannot be a leaf and have children
            Word testWord = new Word("word", Word.Leaf.IsLeaf);
            Word testWord2 = new Word("word", Word.Leaf.IsNotLeaf);

            root.AddChild(ref testWord);
            root.AddChild(ref testWord2);

            // Only added children to the root, so count must be one
            Assert.AreEqual(root.Count, 1);
            // Word had children and thus cannot be a leaf
            Assert.IsFalse(root.IsLeaf.Equals(Word.Leaf.IsLeaf));
            // Added same word twice, so there should only be one child
            Assert.AreEqual(root.Children.Count, 1);
            // That one child should have a count of two
            Word result;
            root.Children.TryGetValue("word", out result);
            Assert.AreEqual(result.Count, 2);
            // The children should combine to still being a possible leaf node
            Assert.IsTrue(result.IsLeaf.Equals(Word.Leaf.IsMaybeLeaf));
        }
    }
}
