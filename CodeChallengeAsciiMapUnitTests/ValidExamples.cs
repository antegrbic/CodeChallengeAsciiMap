using NUnit.Framework;
using CodeChallengeAsciiMap.Core;
using System.IO;
using CodeChallengeAsciiMap.Validation;
using CodeChallengeAsciiMap.Utility;

namespace CodeChallengeAsciiMapUnitTests
{
    public class Tests
    {
        private string _filePath;
        [SetUp]
        public void Setup()
        {
            _filePath = Path.Combine(Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory), @"Examples\ValidExamples\\");
        }

        [TestCase("ascii_map_unittest1.txt", "AB", "@---+|AB|+--x")]
        [TestCase("ascii_map_unittest2.txt", "", "@|x")]
        [TestCase("ascii_map_unittest3.txt", "ACB", "@---A---+|C|+---+|+-B-x")]
        [TestCase("ascii_map_unittest4.txt", "ABCD", "@|A+---B--+|+----C|-||+---D--+|x")]
        [TestCase("ascii_map_unittest5.txt", "BEEFCAKE", "@---+B||E--+|E|+--F--+|C|||A--|-----K|||+--E--Ex")]
        [TestCase("ascii_map_unittest6.txt", "A", "@----+||+--A--x")]
        [TestCase("ascii_map_unittest7.txt", "AB", "@|+--A--B-+||+---+||x")]
        public void ValidExampleCheck(string fileName, string collectedLetters, string pathAsString)
        {
            var solverFacade = new SolverFacade();
            var result = solverFacade.ProcessFile(_filePath + fileName);

            Assert.IsTrue(solverFacade.CollectedLetters == collectedLetters);
            Assert.IsTrue(solverFacade.PathAsString == pathAsString);
        }

    }
}