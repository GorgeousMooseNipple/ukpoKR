using System;
using System.Collections.Generic;
using System.Linq;

namespace UKPO2
{
    class Program
    {

        static void Main(string[] args)
        {
            String originMolecule = "АГЦЦГГУА";
            String[] fragmentsArray = { "АГЦЦ", "ЦГГУ", "ЦГУУ", "ГГУЦ", "ГУА" };

            var graph = new DNAGraph(originMolecule, fragmentsArray);

            for (var i = 0; i < 10000; ++i)
            {
                graph = new DNAGraph(originMolecule, fragmentsArray);
            }

            List<List<String>> paths;

            for (var i = 0; i < 10000; ++i)
            {
                paths = graph.GetPaths();
            }
        }
    }
}
