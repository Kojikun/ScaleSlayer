using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScaleSlayer.Core
{
    /// <summary>
    /// Provides a collection of pre-defined scales to use
    /// </summary>
    public static class Scales
    {
        #region Major Modes

        public static readonly Scale Major = new(
            Interval.Whole, Interval.Whole, Interval.Half, Interval.Whole, Interval.Whole, Interval.Whole, Interval.Half
        );
        public static readonly Scale Dorian = Major with { Mode = 2 };
        public static readonly Scale Phrygian = Major with { Mode = 3 };
        public static readonly Scale Lydian = Major with { Mode = 4 };
        public static readonly Scale Mixolydian = Major with { Mode = 5 };
        public static readonly Scale Minor = Major with { Mode = 6 };
        public static readonly Scale Locrian = Major with { Mode = 7 };

        // Major mode aliases for nerds
        public static readonly Scale Ionian = Major;
        public static readonly Scale Aeolian = Minor;

        #endregion

        #region Melodic Minor Modes

        public static readonly Scale MelodicMinor = new(
            Interval.Whole, Interval.Half, Interval.Whole, Interval.Whole, Interval.Whole, Interval.Whole, Interval.Half
        );
        public static readonly Scale DorianFlat2 = MelodicMinor with { Mode = 2 };
        public static readonly Scale PhrygianSharp6 = DorianFlat2;
        public static readonly Scale LydianAugmented = MelodicMinor with { Mode = 3 };
        public static readonly Scale Acoustic = MelodicMinor with { Mode = 4 };
        public static readonly Scale Overtone = Acoustic;
        public static readonly Scale LydianDominant = Acoustic;
        public static readonly Scale MixolydianSharp4 = Acoustic;
        public static readonly Scale AeolianDominant = MelodicMinor with { Mode = 5 };
        public static readonly Scale MixolydianFlat6 = AeolianDominant;
        public static readonly Scale Hindu = AeolianDominant;
        public static readonly Scale HalfDiminished = MelodicMinor with { Mode = 6 };
        public static readonly Scale LocrianNatural2 = HalfDiminished;
        public static readonly Scale AeolianFlat5 = HalfDiminished;
        public static readonly Scale Altered = MelodicMinor with { Mode = 7 };
        public static readonly Scale SuperLocrian = Altered;

        #endregion

        #region Harmonic Minor Modes

        public static readonly Scale HarmonicMinor = new(
            Interval.Whole, Interval.Half, Interval.Whole, Interval.Whole, Interval.Half, Interval.WholeHalf, Interval.Half
        );
        public static readonly Scale LocrianSharp6 = HarmonicMinor with { Mode = 2 };
        public static readonly Scale IonianSharp5 = HarmonicMinor with { Mode = 3 };
        public static readonly Scale RomanianMinor = HarmonicMinor with { Mode = 4 };
        public static readonly Scale UkranianDorian = RomanianMinor;
        public static readonly Scale AlteredDorian = RomanianMinor;
        public static readonly Scale PhrygianDominant = HarmonicMinor with { Mode = 5 };
        public static readonly Scale AlteredPhrygian = PhrygianDominant;
        public static readonly Scale DominantFlat2Flat6 = PhrygianDominant;
        public static readonly Scale Freygish = PhrygianDominant;
        public static readonly Scale LydianSharp2 = HarmonicMinor with { Mode = 6 };
        public static readonly Scale AlteredDiminished = HarmonicMinor with { Mode = 7 };

        #endregion

        #region Harmonic Major Modes

        public static readonly Scale HarmonicMajor = new(
            Interval.Whole, Interval.Whole, Interval.Half, Interval.Whole, Interval.Half, Interval.WholeHalf, Interval.Half
        );
        public static readonly Scale DorianFlat5 = HarmonicMajor with { Mode = 2 };
        public static readonly Scale LocrianSharp2Sharp6 = DorianFlat5;
        public static readonly Scale PhrygianFlat4 = HarmonicMajor with { Mode = 3 };
        public static readonly Scale AlteredDominantSharp5 = PhrygianFlat4;
        public static readonly Scale LydianFlat3 = HarmonicMajor with { Mode = 4 };
        public static readonly Scale MelodicMinorSharp4 = LydianFlat3;
        public static readonly Scale MixolydianFlat2 = HarmonicMajor with { Mode = 5 };
        public static readonly Scale LydianAugmentedSharp2 = HarmonicMajor with { Mode = 6 };
        public static readonly Scale LocrianDoubleFlat7 = HarmonicMajor with { Mode = 7 };

        #endregion

        #region Double Harmonic Modes

        public static readonly Scale DoubleHarmonic = new(
            Interval.Half, Interval.WholeHalf, Interval.Half, Interval.Whole, Interval.Half, Interval.WholeHalf, Interval.Half
        );
        public static readonly Scale Mayamalavagowla = DoubleHarmonic;
        public static readonly Scale BhairavRaga = DoubleHarmonic;
        public static readonly Scale Byzantine = DoubleHarmonic;
        public static readonly Scale Arabic = DoubleHarmonic;
        public static readonly Scale GypsyMajor = DoubleHarmonic;
        public static readonly Scale LydianSharp2Sharp6 = DoubleHarmonic with { Mode = 2 };
        public static readonly Scale Ultraphrygian = DoubleHarmonic with { Mode = 3 };
        public static readonly Scale PhrygianDoubleFlat7Flat4 = Ultraphrygian;
        public static readonly Scale AlteredDiminishedSharp5 = Ultraphrygian;
        public static readonly Scale HungarianMinor = DoubleHarmonic with { Mode = 4 };
        public static readonly Scale GypsyMinor = HungarianMinor;
        public static readonly Scale LocrianNatural6Natural3 = DoubleHarmonic with { Mode = 5 };
        public static readonly Scale MixolydianFlat5Flat2 = LocrianNatural6Natural3;
        public static readonly Scale Oriental = LocrianNatural6Natural3;
        public static readonly Scale IonianSharp2Sharp5 = DoubleHarmonic with { Mode = 6 };
        public static readonly Scale LocrianDoubleFlat3DoubleFlat7 = DoubleHarmonic with { Mode = 7 };

        #endregion

        #region Special Scales

        public static readonly Scale Diminished = new(
            Interval.Whole, Interval.Half
        );
        public static readonly Scale Octatonic = Diminished;
        public static readonly Scale InvertedDiminished = Diminished with { Mode = 2 };

        public static readonly Scale WholeTone = new(Interval.Whole );
        public static readonly Scale Chromatic = new(Interval.Half );

        #endregion
    }
}
