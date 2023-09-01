using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ScaleSlayer.Core
{
    public static class Extensions
    {
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
    }
}
