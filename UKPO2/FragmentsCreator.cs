using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UKPO2
{
    public class FragmentsCreator
    {
        public static String[] GetFragments(String molecule, int fragNum)
        {
            var fragments = new String[fragNum];
            var random = new Random();

            for (var i = 0; i < fragNum; ++i)
            {
                var fragmentStart = random.Next(0, molecule.Length - 1);
                var fragmentEnd = random.Next(fragmentStart, molecule.Length - 1) + 1;
                fragments[i] = molecule.Substring(fragmentStart, fragmentEnd - fragmentStart);
            }

            return fragments;
        }
    }
}
