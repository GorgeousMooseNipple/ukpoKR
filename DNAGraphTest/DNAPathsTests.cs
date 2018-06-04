using System;
using UKPO2;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace DNAGraphTest
{
    [TestFixture]
    public class GraphUnitTest
    {
        //Проверяется правильность количества найденных путей
        [Test]
        public void FoundPathsCountTest()
        {
            String molecule = "АГЦЦГГУААЦЦ";
            String[] fragmentsArray = { "АГЦЦ", "ЦГГУ", "ГГУАА", "УААЦЦ" };
            DNAGraph graph = new DNAGraph(molecule, fragmentsArray);
            List<List<String>> paths = graph.GetPaths();

            Assert.AreEqual(3, paths.Count);
        }

        //Проверяется ход выполнения при задании пустого графа
        [Test]
        public void EmptyGraphTest()
        {
            String molecule = "АГЦЦГГУААЦЦ";
            String[] fragmentsArray = { };
            DNAGraph graph = new DNAGraph(molecule, fragmentsArray);
            var verticles = graph.VerticleList;

            Assert.AreEqual(0, verticles.Count);
        }

        //Проверка корректности работы класса, разбивающего исходную молекулу на фрагменты
        [Test]
        public void FragmentGeneratorTest()
        {
            var fragments = FragmentsCreator.GetFragments("ADDCCFFDSSDSDSFDFDF", 10);
            Assert.AreEqual(fragments.Length, 10);
        }

        // Проверка правильности нахождения путей в графе
        [Test]
        public void PathFindingTest()
        {
            String molecule = "АГЦЦГГУААЦЦ";
            String[] fragmentsArray = { "АГЦЦ", "ЦГГУ", "ГГУАА", "УААЦЦ" };
            DNAGraph graph = new DNAGraph(molecule, fragmentsArray);
            List<List<String>> paths = graph.GetPaths();

            List<String> path1 = new List<String>() { "АГЦЦ", "ЦГГУ", "ГГУАА", "УААЦЦ" };
            List<String> path2 = new List<String>() { "АГЦЦ", "ЦГГУ", "УААЦЦ" };
            List<String> path3 = new List<String>() { "АГЦЦ", "ГГУАА", "УААЦЦ" };

            Assert.IsTrue(path1.SequenceEqual(paths[0]));
            Assert.IsTrue(path2.SequenceEqual(paths[1]));
            Assert.IsTrue(path3.SequenceEqual(paths[2]));
        }

        // Проверка правильности построения графа
        [Test]
        public void GraphBuildingTest()
        {
            String molecule = "АГЦЦГГУАП";
            String[] fragmentsArray = { "АГЦЦ", "ЦГГУ", "ЦГГУ", "ГГУЦ", "ГУА" };
            DNAGraph graph = new DNAGraph(molecule, fragmentsArray);
            var verticlesList = graph.VerticleList;
            var verticlesNeighboursFragments = new List<List<String>>() {
                new List<String>() { "ЦГГУ" },
                new List<String>() { "ГУА" },
                new List<String>(),
                new List<String>()
               };

            for (var i = 0; i < verticlesList.Count; ++i)
            {
                var neighboursFragments = new List<String>();
                foreach (var neighbour in verticlesList[i].GetNeighbours())
                {
                    neighboursFragments.Add(neighbour.Fragment);
                }
                Assert.IsTrue(verticlesNeighboursFragments[i].SequenceEqual(neighboursFragments));
            }
        }
    }
}
