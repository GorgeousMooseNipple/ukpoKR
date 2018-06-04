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

        //Одинаковые фрагменты молекулы
        [Test]
        public void SameFragmentsTest()
        {
            String molecule = "АГЦЦАГЦЦАГЦЦ";
            String[] fragmentsArray = { "АГЦЦ", "АГЦЦ", "АГЦЦ" };
            DNAGraph graph = new DNAGraph(molecule, fragmentsArray);
            var verticles = graph.VerticleList;

            List<String> neighbours = new List<string>() { "АГЦЦ" };

            Assert.AreEqual(verticles[0].GetNeighbours(), neighbours);
            Assert.AreEqual(verticles[1].GetNeighbours(), neighbours);
            Assert.AreEqual(verticles[2].GetNeighbours(), new List<String>());
        }
    }
}
