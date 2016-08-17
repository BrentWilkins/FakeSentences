namespace FakeSentences
{
    /// <summary>
    /// Graph node for words. Stores info about a word, and links to children nodes.
    /// </summary>
    public class Word
    {
        public enum Leaf { None = 0, IsNotLeaf, IsLeaf, IsMaybeLeaf }
        public int Count { get; set; }
        public double Probability { get; set; }
        public string Text { get; set; }
        public System.Collections.Concurrent.ConcurrentDictionary<string, Word> Children { get; set; }
        public Leaf IsLeaf { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="word">String representation of the word.</param>
        public Word(string word, Leaf isLeaf = Leaf.IsLeaf)
        {
            Count = 1;
            Probability = 0;
            Text = word;
            IsLeaf = isLeaf;
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
                (key, newValue) =>
                {
                    // We get here if key already exists. Need to update that item,
                    newValue.Count++;
                    // Get a reference to the current child with same key
                    Word existing;
                    Children.TryGetValue(newValue.Text, out existing);

                    // The leaf status of the node depends on the current status and the new
                    if (newValue.IsLeaf.Equals(Leaf.IsLeaf))
                    {
                        if (existing.IsLeaf.Equals(Leaf.IsLeaf))
                        {
                            newValue.IsLeaf = Leaf.IsLeaf;
                        }
                        else
                        {
                            newValue.IsLeaf = Leaf.IsMaybeLeaf;
                        }
                    }
                    else if (newValue.IsLeaf.Equals(Leaf.IsNotLeaf))
                    {
                        if (existing.IsLeaf.Equals(Leaf.IsNotLeaf))
                        {
                            newValue.IsLeaf = Leaf.IsNotLeaf;
                        }
                        else
                        {
                            newValue.IsLeaf = Leaf.IsMaybeLeaf;
                        }
                    }
                    else
                    {
                        newValue.IsLeaf = Leaf.IsMaybeLeaf;
                    }
                    return newValue;
                });
        }
    }
}
