
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel;
using System.Net;

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

            // test note with weird enum
            var botchedNote = new Note('A', (Accidental)4);
            Assert.ThrowsException<InvalidEnumArgumentException>(() => botchedNote.Enharmonic());

        }

        [TestMethod]
        public void Operator_Equals()
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

            Assert.AreEqual(new Note('C', Accidental.Flat), new Note('B', Accidental.Natural, 3));
        }

        [TestMethod]
        public void DistanceToMiddleC()
        {
            Assert.AreEqual(0, new Note('C', Accidental.Natural).DistanceToMiddleC());
            Assert.AreEqual(1, new Note('C', Accidental.Sharp).DistanceToMiddleC());
            Assert.AreEqual(1, new Note('D', Accidental.Flat).DistanceToMiddleC());
            Assert.AreEqual(2, new Note('D', Accidental.Natural).DistanceToMiddleC());
            Assert.AreEqual(3, new Note('D', Accidental.Sharp).DistanceToMiddleC());
            Assert.AreEqual(3, new Note('E', Accidental.Flat).DistanceToMiddleC());
            Assert.AreEqual(4, new Note('E', Accidental.Natural).DistanceToMiddleC());
            Assert.AreEqual(5, new Note('E', Accidental.Sharp).DistanceToMiddleC());
            Assert.AreEqual(4, new Note('F', Accidental.Flat).DistanceToMiddleC());
            Assert.AreEqual(5, new Note('F', Accidental.Natural).DistanceToMiddleC());
            Assert.AreEqual(6, new Note('F', Accidental.Sharp).DistanceToMiddleC());
            Assert.AreEqual(6, new Note('G', Accidental.Flat).DistanceToMiddleC());
            Assert.AreEqual(7, new Note('G', Accidental.Natural).DistanceToMiddleC());
            Assert.AreEqual(8, new Note('G', Accidental.Sharp).DistanceToMiddleC());
            Assert.AreEqual(8, new Note('A', Accidental.Flat).DistanceToMiddleC());
            Assert.AreEqual(9, new Note('A', Accidental.Natural).DistanceToMiddleC());
            Assert.AreEqual(10, new Note('A', Accidental.Sharp).DistanceToMiddleC());
            Assert.AreEqual(10, new Note('B', Accidental.Flat).DistanceToMiddleC());
            Assert.AreEqual(11, new Note('B', Accidental.Natural).DistanceToMiddleC());
            Assert.AreEqual(12, new Note('B', Accidental.Sharp).DistanceToMiddleC());
            Assert.AreEqual(11, new Note('C', Accidental.Flat).DistanceToMiddleC());


            Assert.AreEqual(-3, new Note('A', octave: 3).DistanceToMiddleC());
            Assert.AreEqual(-48, new Note('C', octave: 0).DistanceToMiddleC());
        }

        [TestMethod]
        public void Operator_Subtract()
        {
            // A note subtracted by itself is 0 semitones apart
            var A4 = new Note('A');
            Assert.AreEqual(0, A4 - A4);

            // B is 2 semitones apart from A
            var B4 = new Note('B');
            Assert.AreEqual(2, B4 - A4);

            // An octave is 12 semitones away
            var A5 = A4 with { Octave = 5 };
            Assert.AreEqual(12, A5 - A4);
            Assert.AreEqual(-12, A4 - A5);

            var As4 = A4 with { Accidental = Accidental.Sharp };
            Assert.AreEqual(1, As4 - A4);
            Assert.AreEqual(-1, A4 - As4);

            var Ab4 = A4 with { Accidental = Accidental.Flat };
            Assert.AreEqual(-1, Ab4 - A4);
            Assert.AreEqual(1, A4 - Ab4);

            var Bs4 = B4 with { Accidental = Accidental.Sharp };
            Assert.AreEqual(3, Bs4 - A4);
            Assert.AreEqual(0, Bs4 - new Note('C', octave: 5));
        }

        [TestMethod]
        public void Operator_Increment()
        {
            var note = new Note('A', Accidental.Flat);
            note++;

            // Flats always increment to natural, regardless of letter or octave
            Assert.AreEqual(new Note('A'), note);

            // Increment should be A sharp
            note++;
            Assert.AreEqual('A', note.Letter);
            Assert.AreEqual(Accidental.Sharp, note.Accidental);

            // test prefix increment
            Assert.AreEqual(new Note('B'), ++note);
            Assert.AreEqual(new Note('C', octave: 5), ++note);

            // check if increment uses natural note name instead of something wacky like B#
            Assert.AreEqual('C', note.Letter);
            Assert.AreEqual(Accidental.Natural, note.Accidental);
            Assert.AreEqual(5, note.Octave);

            // test postfix increment
            Assert.AreEqual(new Note('C', octave: 5), note++);
            Assert.AreEqual(new Note('C', Accidental.Sharp, 5), note);

            // test edge cases
            note = new Note('E');
            Assert.AreEqual(new Note('F'), ++note);

            note = new Note('G', Accidental.Sharp);
            Assert.AreEqual(new Note('A'), ++note);

            var Esharp = new Note('E', Accidental.Sharp);
            Assert.AreEqual(new Note('F', Accidental.Sharp), ++Esharp);

            var Bsharp = new Note('B', Accidental.Sharp);
            Assert.AreEqual(new Note('C', Accidental.Sharp, 5), ++Bsharp);
        }

        [TestMethod]
        public void Operator_Decrement()
        {
            var note = new Note('G', Accidental.Sharp);
            note--;

            // Sharps always decrement to natural, regardless of letter or octave
            Assert.AreEqual(new Note('G'), note);

            // Decrement should be G flat
            note--;
            Assert.AreEqual(new Note('G', Accidental.Flat), note);
            Assert.AreEqual(Accidental.Flat, note.Accidental);

            // test prefix decrement
            Assert.AreEqual(new Note('F'), --note);
            Assert.AreEqual(new Note('E'), --note);

            // check if decrement uses natural note name instead of something wacky like Fb
            Assert.AreEqual('E', note.Letter);
            Assert.AreEqual(Accidental.Natural, note.Accidental);
            Assert.AreEqual(4, note.Octave);

            // test postfix decrement
            Assert.AreEqual(new Note('E'), note--);
            Assert.AreEqual(new Note('E', Accidental.Flat), note);

            // test edge cases
            note = new Note('C');
            Assert.AreEqual(new Note('B'), --note);

            note = new Note('A', Accidental.Flat);
            Assert.AreEqual(new Note('G'), --note);

            var Fflat = new Note('F', Accidental.Flat);
            Assert.AreEqual(new Note('E', Accidental.Flat), --Fflat);

            var Cflat = new Note('C', Accidental.Flat);
            Assert.AreEqual(new Note('B', Accidental.Flat, 3), --Cflat);
        }

        [TestMethod]
        public void Operator_Add()
        {
            var note = new Note('A');
            Assert.AreEqual(new Note('A', Accidental.Sharp), note + 1);
            Assert.AreEqual(new Note('B'), note + 2);
            Assert.AreEqual(new Note('A', octave: 5), note + 12);
            Assert.AreEqual(new Note('A', Accidental.Sharp, octave: 5), note + 13);

            Assert.AreEqual(new Note('A', Accidental.Flat), note + (-1));
            Assert.AreEqual(new Note('E'), note + (-5));
            Assert.AreEqual(new Note('A', octave: 0), note + (-48));

            Assert.AreEqual(new Note('A', Accidental.Flat), note - 1);
            Assert.AreEqual(new Note('E'), note - 5);
            Assert.AreEqual(new Note('A', octave: 0), note - 48);
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

        [TestMethod]
        public void ImplicitDouble()
        {
            Assert.AreEqual(440.0, new Note('A'));
            Assert.AreEqual(261.6, new Note('C'), 0.1);
            Assert.AreEqual(523.3, new Note('C', octave: 5), 0.1);
        }
    }
}
