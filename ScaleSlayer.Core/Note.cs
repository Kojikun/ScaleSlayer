using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace ScaleSlayer.Core
{
    /// <summary>
    /// Represents a musical note
    /// </summary>
    public struct Note
    {
        #region Static Data

        /// <summary>
        /// Dictionary containing valid letters for note names
        /// </summary>
        public static readonly Dictionary<char, char> ValidLetters = new()
        {
            { 'A', 'A' },
            { 'a', 'A' },
            { 'B', 'B' },
            { 'b', 'B' },
            { 'C', 'C' },
            { 'c', 'C' },
            { 'D', 'D' },
            { 'd', 'D' },
            { 'E', 'E' },
            { 'e', 'E' },
            { 'F', 'F' },
            { 'f', 'F' },
            { 'G', 'G' },
            { 'g', 'G' }
        };

        /// <summary>
        /// Dictionary containing valid string representations for accidentals
        /// </summary>
        public static readonly Dictionary<string, Accidental> ValidAccidentals = new()
        {
            { "flat", Accidental.Flat },
            { "b", Accidental.Flat },
            { "♭", Accidental.Flat },
            { "sharp", Accidental.Sharp },
            { "#", Accidental.Sharp },
            { "♯", Accidental.Sharp }
        };

        private static readonly double ratio_equalTemperament =
            Math.Pow(2, 1 / 12);

        /// <summary>
        /// Regex used to convert a string representation of a note into a <see cref="Note"/>.
        /// </summary>
        private static readonly Regex NoteRegex =
            new(@"^\s*(?<letter>[A-G])\s*(?<accidental>flat|b|sharp|\#|♭|♯)?\s*(?<octave>([1-9][0-9]+)|[0-9])?\s*$",
                RegexOptions.IgnoreCase | RegexOptions.Compiled);

        #endregion



        #region Public Properties

        /// <summary>
        /// The Letter used to designate the note (A, B, C, D, E, F, G)
        /// </summary>
        /// <exception cref="KeyNotFoundException">Thrown when set letter not between A-G</exception>
        public char Letter
        {
            get { return _letter; }
            set { _letter = ValidLetters[value]; }
        }
        private char _letter;

        /// <summary>
        /// Whether the note is a Flat, Sharp, or Natural
        /// </summary>
        public Accidental Accidental { get; set; }

        /// <summary>
        /// The number used to indicate the octave of the note.
        /// </summary>
        public int Octave { get; set; }


        /// <summary>
        /// The percent of detuning between two semitones (100 cents = 1 semitone)
        /// </summary>
        public int Cents
        {
            get { return _cents; }
            set
            {
                _cents = value;
            }
        }
        private int _cents;

        #endregion



        #region Constructors

        /// <summary>
        /// Instantiates a new <see cref="Note"/> object at A4.
        /// </summary>
        public Note() : this('A') { }

        /// <summary>
        /// Instantiates a new <see cref="Note"/> of a given <paramref name="letter"/>
        /// </summary>
        /// <param name="letter">The letter name used to designate the note</param>
        /// <param name="accidental">Whether the note is flat, sharp, or natural</param>
        /// <param name="octave">An integer denoting the octave of the note (Middle C is octave 4)</param>
        /// <param name="cents">The amount of detuning applied to the note</param>
        /// <exception cref="KeyNotFoundException">Thrown when set letter not between A-G</exception>
        public Note(char letter, Accidental accidental = Accidental.Natural, int octave = 4, int cents = 0)
        {
            Letter = letter;
            Accidental = accidental;
            Octave = octave;
            Cents = cents;
        }

        #endregion



        #region Public Functions

        /// <summary>
        /// Returns a note that is identical in pitch but notated as its enharmonic.
        /// </summary>
        /// <returns>Returns a new <see cref="Note"/> struct that is equal to the calling struct, but represented as its enharmonic.</returns>
        /// <exception cref="InvalidEnumArgumentException">Thrown when <see cref="Note.Accidental"/> is set to a value outside its specified enumeration</exception>
        public Note Enharmonic() => Accidental switch
        {
            Accidental.Natural => Letter switch
            {
                'B' => this with { Letter = 'C', Accidental = Accidental.Flat, Octave = Octave + 1 },
                'C' => this with { Letter = 'B', Accidental = Accidental.Sharp, Octave = Octave - 1 },
                'E' => this with { Letter = 'F', Accidental = Accidental.Flat },
                'F' => this with { Letter = 'E', Accidental = Accidental.Sharp },
                _ => this
            },
            Accidental.Sharp => Letter switch
            {
                'B' => this with { Letter = 'C', Accidental = Accidental.Natural, Octave = Octave + 1 },
                'E' => this with { Letter = 'F', Accidental = Accidental.Natural },
                'G' => this with { Letter = 'A', Accidental = Accidental.Flat },
                _ => this with { Letter = (char)(Letter + 1), Accidental = Accidental.Flat }
            },
            Accidental.Flat => Letter switch
            {
                'A' => this with { Letter = 'G', Accidental = Accidental.Sharp },
                'C' => this with { Letter = 'B', Accidental = Accidental.Natural, Octave = Octave - 1 },
                'F' => this with { Letter = 'E', Accidental = Accidental.Natural },
                _ => this with { Letter = (char)(Letter - 1), Accidental = Accidental.Sharp }
            },
            _ => throw new InvalidEnumArgumentException("Property \"Accidental\" has a non-standard enum value.")
        };

        #endregion



        #region Implicit Type Conversions

        /// <summary>
        /// Implicitly converts a string representation of a note into a <see cref="Note"/>
        /// </summary>
        /// <param name="note">The string representation of a note</param>
        public static implicit operator Note(string note)
        {
            // match string with regex
            var match = NoteRegex.Match(note);
            if (!match.Success)
            {
                throw new ArgumentException("Note string could not be parsed into a valid Note", "note");
            }

            // get regex match groups
            var groups = match.Groups;
            var letter = char.Parse(groups["letter"].Value);
            var accidental = groups["accidental"].Success ? ValidAccidentals[groups["accidental"].Value.ToLower()] : Accidental.Natural;
            var octave = groups["octave"].Success ? int.Parse(groups["octave"].Value) : 4;

            // construct new note object
            return new Note(letter, accidental, octave);
        }

        #endregion



        #region Function Overrides

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            // force usage of equality operator if other object is a Note
            // default equality for structs compares member data, which can be different for enharmonics
            if (obj is Note note)
                return this == note;

            return base.Equals(obj);
        }

        public override int GetHashCode() =>
            (Letter, Accidental, Octave, Cents).GetHashCode();

        #endregion



        #region Operator Overloads

        /// <summary>
        /// Comparison Equality operator between two <see cref="Note"/>s.
        /// </summary>
        /// <param name="left">The <see cref="Note"/> on the left-hand side of the operator</param>
        /// <param name="right">The <see cref="Note"/> on the right-hand side of the operator</param>
        /// <returns>Returns true if the notes compared represent the same note (or are enharmonic)</returns>
        public static bool operator ==(Note left, Note right)
        {
            var enharmonic = right.Enharmonic();

            if (left.Cents == right.Cents)
            {
                if (left.Octave == right.Octave || left.Octave == enharmonic.Octave)
                {
                    // notes have same cents and octave, just check letter and accidental
                    return
                        (left.Letter == right.Letter && left.Accidental == right.Accidental) ||
                        (left.Letter == enharmonic.Letter && left.Accidental == enharmonic.Accidental);
                }
            }

            return false;
        }

        /// <summary>
        /// Comparison Inequality operator between two <see cref="Note"/>s.
        /// </summary>
        /// <param name="left">The <see cref="Note"/> on the left-hand side of the operator</param>
        /// <param name="right">The <see cref="Note"/> on the right-hand side of the operator</param>
        /// <returns>Returns true if the notes compared do note represent the same note (or are not enharmonic)</returns>
        public static bool operator !=(Note left, Note right) => !(left == right);

        #endregion


        ///// <summary>
        ///// Returns a new <see cref="Note"/> that's offset by a certain number of <paramref name="semitones"/>.
        ///// </summary>
        ///// <param name="note"></param>
        ///// <param name="semitones"></param>
        ///// <returns></returns>
        //public static Note operator +(Note note, int semitones)
        //{
        //    int newOctave = note.Octave + (semitones / 12);
        //    return new 
        //}

        ///// <summary>
        ///// Implicitly converts a <see cref="Note"/> into its frequency value in Hertz (Hz) using equal temperment
        ///// </summary>
        ///// <param name="note">The <see cref="Note"/> object to convert into Hz</param>
        //public static implicit operator double(Note note)
        //{
        //    const double standardTuning = 440.0;
        //
        //}
        //
        //public static bool operator >(Note left, Note right)
        //{
        //    if (left.Octave != right.Octave)
        //    {
        //        return left.Octave > right.Octave;
        //    }
        //    else
        //    {
        //        if 
        //    }
        //}
        //
        //public static (int Semitones, int Cents) operator -(Note left, Note right)
        //{
        //
        //}
    }

    public enum Accidental
    {
        Natural,
        Flat,
        Sharp
    }
}
