using FakeWords.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakeWords
{
    /// <summary>
    /// Graph node for words. Stores info about a word, and links to children nodes.
    /// </summary>
    public class Word
    {
        public enum Leaf { None = 0, IsNotLeaf, IsLeaf, IsMaybeLeaf }
        public UInt32 Count { get; set; }
        public double Probability { get; set; }
        public string Text { get; set; }
        public System.Collections.Concurrent.ConcurrentDictionary<string, Word> Children { get; set; }
        public Leaf IsLeaf { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="word">String representation of the word.</param>
        public Word(string word)
        {
            Count = 1;
            Probability = 0;
            Text = word;
            IsLeaf = Leaf.IsLeaf;
            Children = new System.Collections.Concurrent.ConcurrentDictionary<string, Word>();
        }

        /// <summary>
        /// Adds a link to the next word found in a sentence.
        /// </summary>
        /// <param name="child">Word to add the link to.</param>
        public Word AddChild(ref Word child)
        {
            // Word nodes that were leaves are no longer leaves with a child
            if (IsLeaf.Equals(Leaf.IsLeaf))
            {
                IsLeaf = Leaf.IsNotLeaf;
            }

            return Children.AddOrUpdate(child.Text, child,
                (key, value) =>
                {
                    // We get here if key already exists. Need to update that item,
                    value.Count++;
                    // The leaf status of the node depends on the current status and the new
                    if (value.IsLeaf.Equals(Leaf.IsLeaf))
                    {
                        if (IsLeaf.Equals(Leaf.IsLeaf))
                        {
                            value.IsLeaf = Leaf.IsLeaf;
                        }
                        else
                        {
                            value.IsLeaf = Leaf.IsMaybeLeaf;
                        }
                    }
                    else if (value.IsLeaf.Equals(Leaf.IsNotLeaf))
                    {
                        if (IsLeaf.Equals(Leaf.IsNotLeaf))
                        {
                            value.IsLeaf = Leaf.IsNotLeaf;
                        }
                        else
                        {
                            value.IsLeaf = Leaf.IsMaybeLeaf;
                        }
                    }
                    else
                    {
                        value.IsLeaf = Leaf.IsMaybeLeaf;
                    }
                    return value;
                });
        }
    }
}
