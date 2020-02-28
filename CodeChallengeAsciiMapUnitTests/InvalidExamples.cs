using CodeChallengeAsciiMap.Core;
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
            _filePath = Path.Combine(Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory), @"Examples/InvalidExamples");
        }

        [Test]
        public void InvalidExample1()
        {
            var asciiMapSolver = new AsciiMapSolver(new Validator());
            asciiMapSolver.SetFile(_filePath + "\\ascii_map_unittest1_invalid.txt");
            var validationResult = asciiMapSolver.SolveProblem();

            Assert.That(validationResult.AdditionalMessage, Is.EqualTo("Couldn't find end position!"));
        }

        [Test]
        public void InvalidExample2()
        {
            var asciiMapSolver = new AsciiMapSolver(new Validator());
            asciiMapSolver.SetFile(_filePath + "\\ascii_map_unittest2_invalid.txt");
            var validationResult = asciiMapSolver.SolveProblem();

            Assert.That(validationResult.AdditionalMessage, Is.EqualTo("Couldn't find start position!"));
        }

        [Test]
        public void InvalidExample3()
        {
            var asciiMapSolver = new AsciiMapSolver(new Validator());

            asciiMapSolver.SetFile(_filePath + "\\ascii_map_unittest3_invalid.txt");
            ValidationResult vr = asciiMapSolver.SolveProblem();
            Assert.That(vr.AdditionalMessage, Is.EqualTo("Invalid field   encountered at position (5,4)"));
        }

        [Test]
        public void InvalidExample4()
        {
            var asciiMapSolver = new AsciiMapSolver(new Validator());
            asciiMapSolver.SetFile(_filePath + "\\ascii_map_unittest4_invalid.txt");

            ValidationResult vr = asciiMapSolver.SolveProblem();
            Assert.That(vr.AdditionalMessage, Is.EqualTo("Multiple directions from (5,4) position!"));
        }

        [Test]
        public void InvalidExample5()
        {
            var asciiMapSolver = new AsciiMapSolver(new Validator());
            asciiMapSolver.SetFile(_filePath + "\\ascii_map_unittest5_invalid.txt");
            var validationResult = asciiMapSolver.SolveProblem();

            Assert.That(validationResult.AdditionalMessage, Is.EqualTo("Multiple end position detected!"));
        }

        [Test]
        public void InvalidExample6()
        {
            var asciiMapSolver = new AsciiMapSolver(new Validator());
            asciiMapSolver.SetFile(_filePath + "\\ascii_map_unittest6_invalid.txt");
            var validationResult = asciiMapSolver.SolveProblem();

            Assert.That(validationResult.AdditionalMessage, Is.EqualTo("Multiple start position detected!"));

        }
    }


}
