using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScaleSlayer.Core
{
    public enum Accidental
    {
        Natural,
        Flat,
        Sharp
    }

    public enum Interval
    {
        Unison = 0,
        MinorSecond = 1,
        Semitone = 1,
        Half = 1,
        MajorSecond = 2,
        Wholetone = 2,
        Whole = 2,
        MinorThird = 3,
        WholeHalf = 3,
        AugmentedSecond = 3,
        MajorThird = 4,
        Fourth = 5,
        Tritone = 6,
        Fifth = 7,
        MinorSixth = 8,
        MajorSixth = 9,
        MinorSeventh = 10,
        MajorSeventh = 11,
        Octave = 12,
        MinorNinth = 13,
        MajorNinth = 14,
        Eleventh = 17,
        SharpEleventh = 18,
        Thirteenth = 21
    }
}
