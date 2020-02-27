using NUnit.Framework;
using CodeChallengeAsciiMap.Core;
using System.IO;

namespace CodeChallengeAsciiMapUnitTests
{
    public class Tests
    {
        private string _filePath;
        [SetUp]
        public void Setup()
        {
            _filePath = Path.Combine(Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory), @"Examples/ValidExamples");
        }

        [Test]
        public void ValidExample1()
        {
            AsciiMapSolver asciiMapSolver = new AsciiMapSolver(_filePath + "/ascii_map_unittest1.txt");
            asciiMapSolver.SolveProblem();

            Assert.IsTrue(asciiMapSolver.CollectedLetters == "AB");
            Assert.IsTrue(asciiMapSolver.PathAsString == "@---+|AB|+--x");
        }

        [Test]
        public void ValidExample2()
        {
            AsciiMapSolver asciiMapSolver = new AsciiMapSolver(_filePath + "/ascii_map_unittest2.txt");
            asciiMapSolver.SolveProblem();

            Assert.IsTrue(asciiMapSolver.CollectedLetters == "");
            Assert.IsTrue(asciiMapSolver.PathAsString == "@|x");
        }

        [Test]
        public void ValidExample3()
        {
            AsciiMapSolver asciiMapSolver = new AsciiMapSolver(_filePath + "/ascii_map_unittest3.txt");
            asciiMapSolver.SolveProblem();

            Assert.IsTrue(asciiMapSolver.CollectedLetters == "ACB");
            Assert.IsTrue(asciiMapSolver.PathAsString == "@---A---+|C|+---+|+-B-x");
        }

        [Test]
        public void ValidExample4()
        {
            AsciiMapSolver asciiMapSolver = new AsciiMapSolver(_filePath + "/ascii_map_unittest4.txt");
            asciiMapSolver.SolveProblem();

            Assert.IsTrue(asciiMapSolver.CollectedLetters == "ABCD");
            Assert.IsTrue(asciiMapSolver.PathAsString == "@|A+---B--+|+----C|-||+---D--+|x");
        }

        [Test]
        public void ValidExample5()
        {
            AsciiMapSolver asciiMapSolver = new AsciiMapSolver(_filePath + "/ascii_map_unittest5.txt");
            asciiMapSolver.SolveProblem();

            Assert.IsTrue(asciiMapSolver.CollectedLetters == "BEEFCAKE");
            Assert.IsTrue(asciiMapSolver.PathAsString == "@---+B||E--+|E|+--F--+|C|||A--|-----K|||+--E--Ex");
        }
    }
}