using CodeChallengeAsciiMap.Core;
using CodeChallengeAsciiMap.Utility;
using CodeChallengeAsciiMap.Validation;
using NUnit.Framework;
using System;
using System.IO;

namespace CodeChallengeAsciiMapUnitTests.Examples
{
    class InvalidExamples
    {
        private string _filePath;
        [SetUp]
        public void Setup()
        {
            _filePath = Path.Combine(Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory), @"Examples/InvalidExamples\\");
        }

        [TestCase("ascii_map_unittest1_invalid.txt", "Couldn't find end position!")]
        [TestCase("ascii_map_unittest2_invalid.txt", "Couldn't find start position!")]
        [TestCase("ascii_map_unittest3_invalid.txt", "Invalid field   encountered at position (5,4)")]
        [TestCase("ascii_map_unittest4_invalid.txt", "Multiple directions from (5,4) position!")]
        [TestCase("ascii_map_unittest5_invalid.txt", "Multiple end position detected!")]
        [TestCase("ascii_map_unittest6_invalid.txt", "Multiple start position detected!")]
        public void InvalidExampleTest(string fileName, string errorMessage)
        {
            var solverFacade = new SolverFacade();
            var result = solverFacade.ProcessFile(_filePath + fileName);

            Assert.That(result.AdditionalMessage, Is.EqualTo(errorMessage));
        }
    }


}
