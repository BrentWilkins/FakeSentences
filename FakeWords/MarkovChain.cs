using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakeWords
{
    public class MarkovChain
    {
        public MarkovChain()
        {

        }

        /// <summary>
        ///  Add a transition from one state to another.
        /// </summary>
        /// <param name="from">The state the new edge starts from.</param>
        /// <param name="to">The state the new edge leads to.</param>
        public void AddTransition(State from, State to)
        {

        }

        /// <summary>
        // Return random neighbor of current state in directed graph.
        /// </summary>
        public void Next(State current)
        {

        }

        /// <summary>
        // Return a string representation of this chain.
        /// </summary>
        /// <returns>String representation of this Markov chain</returns>
        public override string ToString()
        {
            return base.ToString();
        }

        /// <summary>
        /// MyProperty property.
        /// </summary>
        /// <value>
        /// This value tag should be used to describe the property value.</value>
        public int MyProperty { get; set; }
    }
}
