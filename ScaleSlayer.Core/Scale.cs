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
        /// <summary>
        /// A list of intervals that define where the next note in the scale lies
        /// </summary>
        public List<Interval> Intervals { get; set; } = new List<Interval>();

        /// <summary>
        /// The 1-indexed Mode (Scale degree) used to define where in the <see cref="Intervals"/> list to start generating notes from
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when the Mode is attempted to be set outside the length of the <see cref="Intervals"/> list
        /// </exception>
        public int Mode
        {
            get { return _mode; }
            init
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

        /// <summary>
        /// Helper property to zero-index the current <see cref="Mode"/>
        /// </summary>
        private int ModeIndex
        {
            get => Mode - 1;
        }

        /// <summary>
        /// Constructs a new scale using a set of intervals
        /// </summary>
        /// <param name="intervals">A list of intervals that form the scale</param>
        public Scale(params Interval[] intervals) : this((IEnumerable<Interval>)intervals) { }

        /// <summary>
        /// Constructs a new scale using a set of intervals
        /// </summary>
        /// <param name="intervals">A list of intervals that form the scale</param>
        public Scale(IEnumerable<Interval> intervals)
        {
            Intervals.AddRange(intervals);
        }

        /// <summary>
        /// Generates a list of notes separated by the intervals stored in the scale
        /// </summary>
        /// <param name="root">The root <see cref="Note"/> for the scale to start generating notes from</param>
        /// <param name="retainOrder">Whether to attempt to retain the alphabetical ordering of notes within a scale</param>
        /// <returns>Returns an iterator that points to the next note in the scale</returns>
        /// <remarks>
        /// Returned notes will attempt to resemble the natural ordering of letters.
        /// Double-flats/sharps are not implemented in the <see cref="Note"/> class,
        /// so the natural ordering of returned notes will not be preserved
        /// if the next letter in the sequence would require one of those accidentals.
        /// If <paramref name="retainOrder"/> is false, only sharps or naturals would be returned.
        /// </remarks>
        /// <example>
        /// Generate(new Note('A')).Take(8) will generate the full scale.
        /// </example>
        public IEnumerable<Note> Generate(Note root, bool retainOrder = true)
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

                // ensure that the next note's letter follows the
                // natural ordering of letters in a scale
                char nextLetter = (char)(((previous.Letter + 1 - 'A') % 7) + 'A');
                if (retainOrder && (current.Letter != nextLetter))
                {
                    if (current.Enharmonic().Letter == nextLetter)
                    {
                        current = current.Enharmonic();
                    }    
                }

                // use the next interval in the sequence
                currentIndex = (currentIndex + 1) % Intervals.Count;
            }
        }
    }
}
