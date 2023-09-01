using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScaleSlayer.Core.Test
{
    public static class TestUtilities
    {
        public static string Letters(this IEnumerable<Note> scale) =>
            string.Join("", scale.Select(note => note.Letter));
    }
}
