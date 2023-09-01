using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ScaleSlayer.Core
{
    /// <summary>
    /// Extension methods typically used on the result of a <see cref="Scale.Generate"/>
    /// </summary>
    public static class ScaleExtensions
    {
        /// <summary>
        /// Forces the usage of an <paramref name="accidental"/> for a set of generated notes.
        /// </summary>
        /// <param name="source">A list of <see cref="Note"/>s, typically as a result of a call to <see cref="Scale.Generate"/></param>
        /// <param name="accidental">The <see cref="Accidental"/> to convert the current note to</param>
        /// <param name="convertNaturals">Whether to force naturals to use strange enharmonics such as B sharp or C flat</param>
        /// <returns>An <see cref="IEnumerable{Note}"/> of notes with enharmonics that represent the supplied <paramref name="accidental"/></returns>
        public static IEnumerable<Note> OfAccidental(this IEnumerable<Note> source, Accidental accidental, bool convertNaturals = false)
        {
            foreach (var note in source)
            {
                yield return (note.Accidental == accidental) switch
                {
                    // yield unmodified note if the accidental is of the correct type
                    true => note,
                    false => note.Accidental switch
                    {
                        // if current note is a natural, only yield its enharmonic if
                        // option is set to do so
                        Accidental.Natural => 
                            convertNaturals
                            ? (note.Enharmonic().Accidental == accidental ? note.Enharmonic() : note)
                            : note,
                        // note of opposite accidental; yield enharmonic
                        _ => note.Enharmonic()
                    }
                };
            }
        }

        /// <summary>
        /// Returns a list of notes constrained within a given number of semitones.
        /// </summary>
        /// <param name="source">A list of <see cref="Note"/>s, typically as a result of a call to <see cref="Scale.Generate"/></param>
        /// <param name="semitones">The number of semitones to constrain the <paramref name="source"/> list</param>
        /// <param name="exclusive">Whether the end of the range is inclusive (false) or exclusive (true)</param>
        /// <returns>Returns a finite <see cref="IEnumerable{Note}"/> of notes constrained by the number of <paramref name="semitones"/> given.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="semitones"/> is less than 0.</exception>
        public static IEnumerable<Note> WithinRange(this IEnumerable<Note> source, int semitones, bool exclusive = true)
        {
            if (semitones < 0)
                throw new ArgumentException($"\"{nameof(semitones)}\" cannot be less than 0.", nameof(semitones));

            // get first note in sequence
            var root = source.FirstOrDefault();
            semitones += exclusive ? 0 : 1;

            foreach (var note in source)
            {
                // yield current note if distance between current note and root within range
                if (Math.Abs(note - root) < semitones)
                {
                    yield return note;
                }
                else
                {
                    yield break;
                }
            }
        }

        /// <summary>
        /// Returns a list of notes constrained within a given number of octaves.
        /// </summary>
        /// <param name="source">A list of <see cref="Note"/>s, typically as a result of a call to <see cref="Scale.Generate"/></param>
        /// <param name="count">The number of octaves to constrain the <paramref name="source"/> list</param>
        /// <param name="containsLastRoot">Whether or not to include the note that bounds the last octave</param>
        /// <returns>Returns a finite <see cref="IEnumerable{Note}"/> of notes constrained by the number of octaves given.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="count"/> is less than 0.</exception>
        public static IEnumerable<Note> Octaves(this IEnumerable<Note> source, int count, bool containsLastRoot = true)
        {
            if (count < 0)
                throw new ArgumentException($"\"{nameof(count)}\" cannot be less than 0.", nameof(count));

            // an octave is 12 semitones
            return source.WithinRange(count * 12, !containsLastRoot);
        }
    }
}
