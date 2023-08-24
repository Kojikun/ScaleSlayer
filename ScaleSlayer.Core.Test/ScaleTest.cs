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
            var CMajorScale = Scales.Major.Generate(new Note('C')).Take(8).ToArray();
        }
    }
}
