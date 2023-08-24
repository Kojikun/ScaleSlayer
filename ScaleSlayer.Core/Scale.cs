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
        /// <exception cref="NotImplementedException"></exception>
        public IEnumerable<Note> Generate(Note root)
        {
            throw new NotImplementedException();
        }
    }
}
