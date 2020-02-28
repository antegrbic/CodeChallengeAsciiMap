using NUnit.Framework;
using CodeChallengeAsciiMap.Core;
using System.IO;
using CodeChallengeAsciiMap.Validation;

namespace CodeChallengeAsciiMapUnitTests
{
    public class Tests
    {
        private string _filePath;
        [SetUp]
        public void Setup()
        {
            _filePath = Path.Combine(Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory), @"Examples\ValidExamples");
        }

        [Test]
        public void ValidExample1()
        {
            AsciiMapSolver asciiMapSolver = new AsciiMapSolver(new Validator());

            asciiMapSolver.SolveProblem(_filePath + "\\ascii_map_unittest1.txt");

            Assert.IsTrue(asciiMapSolver.CollectedLetters == "AB");
            Assert.IsTrue(asciiMapSolver.PathAsString == "@---+|AB|+--x");
        }

        [Test]
        public void ValidExample2()
        {
            AsciiMapSolver asciiMapSolver = new AsciiMapSolver(new Validator());

            asciiMapSolver.SolveProblem(_filePath + "\\ascii_map_unittest2.txt");

            Assert.IsTrue(asciiMapSolver.CollectedLetters == "");
            Assert.IsTrue(asciiMapSolver.PathAsString == "@|x");
        }

        [Test]
        public void ValidExample3()
        {
            AsciiMapSolver asciiMapSolver = new AsciiMapSolver(new Validator());

            asciiMapSolver.SolveProblem(_filePath + "\\ascii_map_unittest3.txt");

            Assert.IsTrue(asciiMapSolver.CollectedLetters == "ACB");
            Assert.IsTrue(asciiMapSolver.PathAsString == "@---A---+|C|+---+|+-B-x");
        }

        [Test]
        public void ValidExample4()
        {
            AsciiMapSolver asciiMapSolver = new AsciiMapSolver(new Validator());

            asciiMapSolver.SolveProblem(_filePath + "\\ascii_map_unittest4.txt");

            Assert.IsTrue(asciiMapSolver.CollectedLetters == "ABCD");
            Assert.IsTrue(asciiMapSolver.PathAsString == "@|A+---B--+|+----C|-||+---D--+|x");
        }

        [Test]
        public void ValidExample5()
        {
            AsciiMapSolver asciiMapSolver = new AsciiMapSolver(new Validator());

            asciiMapSolver.SolveProblem(_filePath + "\\ascii_map_unittest5.txt");

            Assert.IsTrue(asciiMapSolver.CollectedLetters == "BEEFCAKE");
            Assert.IsTrue(asciiMapSolver.PathAsString == "@---+B||E--+|E|+--F--+|C|||A--|-----K|||+--E--Ex");
        }
    }
}