using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScaleSlayer.Core.Test
{
    [TestClass]
    public class ScaleTest
    {
        [TestMethod]
        public void TestGenerate()
        {
            // Generate an 8 note scale from root
            var CMajorScale = Scales.Major.Generate("C4").Take(8).ToArray();

            // ensure that the first note returned from generator is root
            Assert.AreEqual("C4", CMajorScale[0]);

            // check if notes in scale are correct
            Assert.AreEqual(8, CMajorScale.Length);
            Assert.AreEqual("D4", CMajorScale[1]);
            Assert.AreEqual("E4", CMajorScale[2]);
            Assert.AreEqual("F4", CMajorScale[3]);
            Assert.AreEqual("G4", CMajorScale[4]);
            Assert.AreEqual("A4", CMajorScale[5]);
            Assert.AreEqual("B4", CMajorScale[6]);
            Assert.AreEqual("C5", CMajorScale[7]);

            // try A major
            var AMajorScale = Scales.Major.Generate("A4").Take(8).ToArray();

            Assert.AreEqual(8, AMajorScale.Length);
            Assert.AreEqual("A4", AMajorScale[0]);
            Assert.AreEqual("B4", AMajorScale[1]);
            Assert.AreEqual("C#5", AMajorScale[2]);
            Assert.AreEqual("D5", AMajorScale[3]);
            Assert.AreEqual("E5", AMajorScale[4]);
            Assert.AreEqual("F#5", AMajorScale[5]);
            Assert.AreEqual("G#5", AMajorScale[6]);
            Assert.AreEqual("A5", AMajorScale[7]);

            // try Bb major
            var BbMajorScale = Scales.Major.Generate("Bb4").Take(8).ToArray();

            Assert.AreEqual(8, BbMajorScale.Length);
            Assert.AreEqual("Bb4", BbMajorScale[0]);
            Assert.AreEqual("C5", BbMajorScale[1]);
            Assert.AreEqual("D5", BbMajorScale[2]);
            Assert.AreEqual("Eb5", BbMajorScale[3]);
            Assert.AreEqual("F5", BbMajorScale[4]);
            Assert.AreEqual("G5", BbMajorScale[5]);
            Assert.AreEqual("A5", BbMajorScale[6]);
            Assert.AreEqual("Bb5", BbMajorScale[7]);

            // make sure proper enharmonic is represented (letters are in order)
            Assert.AreEqual('B', BbMajorScale[0].Letter);
            Assert.AreEqual('E', BbMajorScale[3].Letter);
            Assert.AreEqual('B', BbMajorScale[7].Letter);

            // try a different mode
            var AMinorScale = Scales.Minor.Generate("A4").Take(8).ToArray();
            Assert.AreEqual(8, AMinorScale.Length);
            Assert.AreEqual("A4", AMinorScale[0]);
            Assert.AreEqual("B4", AMinorScale[1]);
            Assert.AreEqual("C5", AMinorScale[2]);
            Assert.AreEqual("D5", AMinorScale[3]);
            Assert.AreEqual("E5", AMinorScale[4]);
            Assert.AreEqual("F5", AMinorScale[5]);
            Assert.AreEqual("G5", AMinorScale[6]);
            Assert.AreEqual("A5", AMinorScale[7]);

            var CHarmonicMinor = Scales.HarmonicMinor.Generate("C4").Take(8).ToArray();
            Assert.AreEqual(8, CHarmonicMinor.Length);
            Assert.AreEqual("C4", CHarmonicMinor[0]);
            Assert.AreEqual("D4", CHarmonicMinor[1]);
            Assert.AreEqual("Eb4", CHarmonicMinor[2]);
            Assert.AreEqual("F4", CHarmonicMinor[3]);
            Assert.AreEqual("G4", CHarmonicMinor[4]);
            Assert.AreEqual("Ab4", CHarmonicMinor[5]);
            Assert.AreEqual("B4", CHarmonicMinor[6]);
            Assert.AreEqual("C5", CHarmonicMinor[7]);
        }
    }
}
