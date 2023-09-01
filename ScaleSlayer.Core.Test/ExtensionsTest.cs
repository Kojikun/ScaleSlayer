using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScaleSlayer.Core.Test
{
    [TestClass]
    public class ExtensionsTest
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
    }
}
