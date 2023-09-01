using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScaleSlayer.Core.Test
{
    [TestClass]
    public class ScaleExtensionsTest
    {
        [TestMethod]
        public void OfAccidental()
        {
            var AMajorScale = Scales.Major.Generate("A4").Take(8);
            Assert.AreEqual("ABCDEFGA", AMajorScale.Letters());

            // Force usage of flats
            Assert.AreEqual("ABDDEGAA", AMajorScale.OfAccidental(Accidental.Flat).Letters());

            // Scale that only uses sharps should not change
            Assert.AreEqual(AMajorScale.Letters(), AMajorScale.OfAccidental(Accidental.Sharp).Letters());

            // Forcing accidentals would use strange enharmonics, such as B Sharp, C Flat, E Sharp, F Flat
            var CMajorScale = Scales.Major.Generate("C4").Take(8);
            Assert.AreEqual("CDEFGABC", CMajorScale.Letters());

            Assert.AreEqual("CDEFGABC", CMajorScale.OfAccidental(Accidental.Flat).Letters());
            Assert.AreEqual("CDFFGACC", CMajorScale.OfAccidental(Accidental.Flat, convertNaturals: true).Letters());

            Assert.AreEqual("CDEFGABC", CMajorScale.OfAccidental(Accidental.Sharp).Letters());
            Assert.AreEqual("BDEEGABB", CMajorScale.OfAccidental(Accidental.Sharp, convertNaturals: true).Letters());

        }

        [TestMethod]
        public void WithinRange()
        {
            var CMajorScale = Scales.Major.Generate("C");

            // Get the notes that are bound by the fifth
            Assert.AreEqual("CDEF", CMajorScale.WithinRange((int)Interval.Fifth).Letters());

            // Include the fifth
            Assert.AreEqual("CDEFG", CMajorScale.WithinRange((int)Interval.Fifth, exclusive: false).Letters());

            // Get an octave
            Assert.AreEqual("CDEFGAB", CMajorScale.WithinRange((int)Interval.Octave).Letters());

            // Range should be agnostic of ordering of notes
            Assert.AreEqual("CBAGFED", CMajorScale.Take(15).Reverse().WithinRange((int)Interval.Octave).Letters());

            // Range of 0 should return an empty enumerable if exclusive
            Assert.AreEqual("", CMajorScale.WithinRange(0).Letters());

            // If inclusive, a range of 0 should return the root
            Assert.AreEqual("C", CMajorScale.WithinRange(0, exclusive: false).Letters());
        }

        [TestMethod]
        public void Octaves()
        {
            var root = new Note('C');
            var CMajorScale = Scales.Major.Generate(root);

            // Generate 1 octave
            Assert.AreEqual("CDEFGABC", CMajorScale.Octaves(1).Letters());
            Assert.AreEqual("CDEFGAB", CMajorScale.Octaves(1, containsLastRoot: false).Letters());

            // Generate 2 octaves
            Assert.AreEqual("CDEFGABCDEFGABC", CMajorScale.Octaves(2).Letters());
            Assert.AreEqual("CDEFGABCDEFGAB", CMajorScale.Octaves(2, containsLastRoot: false).Letters());

            // Calculate far ahead
            int octaves = 10;
            Assert.AreEqual(root with { Octave = root.Octave + octaves }, CMajorScale.Octaves(octaves).Last());

            // 0 should return just the root (or empty if root not contained)
            Assert.AreEqual("C", CMajorScale.Octaves(0).Letters());
            Assert.AreEqual("", CMajorScale.Octaves(0, containsLastRoot: false).Letters());

        }
    }
}
