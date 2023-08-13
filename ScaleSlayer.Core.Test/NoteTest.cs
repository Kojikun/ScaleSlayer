
namespace ScaleSlayer.Core.Test
{
    [TestClass]
    public class NoteTest
    {
        [TestMethod]
        public void Constructor()
        {
            // invoke parameterless constructor
            var parameterless = new Note();
            Assert.AreEqual('A', parameterless.Letter);
            Assert.AreEqual(Accidental.Natural, parameterless.Accidental);
            Assert.AreEqual(4, parameterless.Octave);
            Assert.AreEqual(0, parameterless.Cents);

            // check default args
            var defaultArgsNote = new Note('C');
            Assert.AreEqual('C', defaultArgsNote.Letter);
            Assert.AreEqual(Accidental.Natural, defaultArgsNote.Accidental);
            Assert.AreEqual(4, defaultArgsNote.Octave);
            Assert.AreEqual(0, defaultArgsNote.Cents);

            // use fully filled out constructor
            var complexNote = new Note('G', Accidental.Sharp, 7, 13);
            Assert.AreEqual('G', complexNote.Letter);
            Assert.AreEqual(Accidental.Sharp, complexNote.Accidental);
            Assert.AreEqual(7, complexNote.Octave);
            Assert.AreEqual(13, complexNote.Cents);

            // valid letters are converted to uppercase
            Assert.AreEqual('A', new Note('A').Letter);
            Assert.AreEqual('A', new Note('a').Letter);
            Assert.AreEqual('B', new Note('B').Letter);
            Assert.AreEqual('B', new Note('b').Letter);
            Assert.AreEqual('C', new Note('C').Letter);
            Assert.AreEqual('C', new Note('c').Letter);
            Assert.AreEqual('D', new Note('D').Letter);
            Assert.AreEqual('D', new Note('d').Letter);
            Assert.AreEqual('E', new Note('E').Letter);
            Assert.AreEqual('E', new Note('e').Letter);
            Assert.AreEqual('F', new Note('F').Letter);
            Assert.AreEqual('F', new Note('f').Letter);
            Assert.AreEqual('G', new Note('G').Letter);
            Assert.AreEqual('G', new Note('g').Letter);

            // invalid letter throws a KeyNotFoundException
            Assert.ThrowsException<KeyNotFoundException>(() => new Note('H'));
        }

        [TestMethod]
        public void Enharmonic()
        {
            // enharmonic of basic natural note should be the same note
            var A4 = new Note('A');
            var A4_en = A4.Enharmonic();
            Assert.AreEqual('A', A4_en.Letter);
            Assert.AreEqual(Accidental.Natural, A4_en.Accidental);
            Assert.AreEqual(4, A4_en.Octave);

            // Enharmonic of a sharp should be flat
            var Cs4 = new Note('C', Accidental.Sharp);
            var Db4 = Cs4.Enharmonic();
            Assert.AreEqual('D', Db4.Letter);
            Assert.AreEqual(Accidental.Flat, Db4.Accidental);
            Assert.AreEqual(4, Db4.Octave);

            // Enharmonic of a flat should be a sharp
            var Eb4 = new Note('E', Accidental.Flat);
            var Ds4 = Eb4.Enharmonic();
            Assert.AreEqual('D', Ds4.Letter);
            Assert.AreEqual(Accidental.Sharp, Ds4.Accidental);
            Assert.AreEqual(4, Ds4.Octave);

            // Enharmonic of an enharmonic should be the original note
            var Cs4_new = Db4.Enharmonic();
            var Eb4_new = Ds4.Enharmonic();
            Assert.AreEqual(Cs4, Cs4_new);
            Assert.AreEqual(Eb4, Eb4_new);

            // edge cases

            // C natural is enharmonic to B sharp
            // This should also drop the octave
            var C4 = new Note('C');
            var Bs3 = C4.Enharmonic();
            Assert.AreEqual('B', Bs3.Letter);
            Assert.AreEqual(Accidental.Sharp, Bs3.Accidental);
            Assert.AreEqual(3, Bs3.Octave);

            // Conversely, Bsharp is a C natural
            Assert.AreEqual(C4, Bs3.Enharmonic());

            // F flat is enharmonic to E natural
            var Fb4 = new Note('F', Accidental.Flat);
            var E4 = Fb4.Enharmonic();
            Assert.AreEqual('E', E4.Letter);
            Assert.AreEqual(Accidental.Natural, E4.Accidental);

            // brute force every note-harmonic combination
            // equality operator should compare enharmonics
            Assert.AreEqual(new Note('A', Accidental.Sharp), new Note('B', Accidental.Flat));
            Assert.AreEqual(new Note('B', Accidental.Sharp), new Note('C', Accidental.Natural, 5));
            Assert.AreEqual(new Note('C', Accidental.Flat), new Note('B', Accidental.Natural, 3));
            Assert.AreEqual(new Note('C', Accidental.Sharp), new Note('D', Accidental.Flat));
            Assert.AreEqual(new Note('D', Accidental.Sharp), new Note('E', Accidental.Flat));
            Assert.AreEqual(new Note('E', Accidental.Sharp), new Note('F', Accidental.Natural));
            Assert.AreEqual(new Note('F', Accidental.Flat), new Note('E', Accidental.Natural));
            Assert.AreEqual(new Note('F', Accidental.Sharp), new Note('G', Accidental.Flat));
            Assert.AreEqual(new Note('G', Accidental.Sharp), new Note('A', Accidental.Flat));

        }

        [TestMethod]
        public void ComparisonOperator_Equals()
        {
            // make two identical notes
            var A4 = new Note('A');
            var A4_explicit = new Note('A', Accidental.Natural, 4);
            Assert.IsTrue(A4 == A4_explicit);
            Assert.AreEqual(A4, A4_explicit);

            // Notes of different cents are not equal
            var A4_detuned = A4_explicit with { Cents = 7 };
            Assert.IsFalse(A4 == A4_detuned);
            Assert.IsTrue(A4 != A4_detuned);

            // Notes of different octaves are not equal
            var A5 = A4 with { Octave = 5 };
            Assert.AreNotEqual(A4, A5);

            // Notes of different letters are not equal
            var B4 = new Note('B');
            Assert.AreNotEqual(A4, B4);

            // Enharmonics are treated as equal
            var Ab4 = new Note('A', Accidental.Flat);
            var Gs4 = new Note('G', Accidental.Sharp);
            var Ab4_en = Ab4.Enharmonic();
            Assert.IsTrue(Ab4 == Gs4);
            Assert.AreEqual(Ab4, Gs4);
            Assert.AreEqual(Ab4, Ab4.Enharmonic());
        }

        [TestMethod]
        public void ImplicitString()
        {
            // construct A4 from string
            Note A4 = "A";
            Assert.AreEqual(new Note('A'), A4);

            // Test all possible flats
            Note Bb5 = new Note('B', Accidental.Flat, 5);
            Assert.AreEqual(Bb5, "Bb5");
            Assert.AreEqual(Bb5, " B b   5   ");    // whitespace agnostic regex
            Assert.AreEqual(Bb5, "B flat 5");
            Assert.AreEqual(Bb5, "bB5");            // case agnostic regex
            Assert.AreEqual(Bb5, "B♭5");

            // Test all possible sharps
            Note Cs6 = new Note('C', Accidental.Sharp, 6);
            Assert.AreEqual(Cs6, "C#6");
            Assert.AreEqual(Cs6, "CSharp6");
            Assert.AreEqual(Cs6, "C♯ 6");
        }
    }
}
