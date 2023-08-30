using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace ScaleSlayer.Core
{
    public record Scale
    {
        public List<Interval> Intervals { get; set; } = new List<Interval>();

        public int Mode
        {
            get { return _mode; }
            set
            {
                if (value > 0 && value <= Intervals.Count)
                    _mode = value;
                else if (value <= 0)
                    throw new ArgumentOutOfRangeException("Value set for Mode must be greater than 0");
                else
                    throw new ArgumentOutOfRangeException("Value set for Mode cannot be greater than the number of intervals in a scale.");
            }
        }
        private int _mode = 1;

        private int ModeIndex
        {
            get => Mode - 1;
        }

        public Scale(params Interval[] intervals) : this((IEnumerable<Interval>)intervals) { }

        public Scale(IEnumerable<Interval> intervals)
        {
            Intervals.AddRange(intervals);
        }

        /// <summary>
        /// Generates a list of notes separated by the intervals stored in the scale
        /// </summary>
        /// <param name="root"></param>
        /// <returns>Returns an iterator that points to the next note in the scale</returns>
        /// <example>
        /// Generate(new Note('A')).Take(8) will generate the full scale.
        /// </example>
        public IEnumerable<Note> Generate(Note root)
        {
            // start from root note
            Note current = root;
            var currentIndex = ModeIndex;

            while (true)
            {
                // yield current note to caller
                yield return current;
                var previous = current;

                // calculate next note using current interval
                var currentInterval = Intervals[currentIndex];
                current += (int)currentInterval;

                // ensure that the next letter does not equal the previous one
                if (current.Letter == previous.Letter)
                    // if so, return the enharmonic instead
                    // incrementing up the scale will always return sharps,
                    // so this ensures that flats will be displayed, too
                    current = current.Enharmonic();

                // use the next interval in the sequence
                currentIndex = (currentIndex + 1) % Intervals.Count;
            }
        }
    }
}
