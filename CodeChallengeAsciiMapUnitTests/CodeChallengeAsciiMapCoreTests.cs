using CodeChallengeAsciiMap.Core;
using CodeChallengeAsciiMap.Tests.Utility;
using CodeChallengeAsciiMap.Utility;
using NUnit.Framework;

namespace CodeChallengeAsciiMap.Tests
{
    class CodeChallengeAsciiMapCoreTests
    {
        public object TestUtility { get; private set; }

        [SetUp]
        public void Setup()
        {
           
        }   

        [Test]
        public void Position_IsEqual_True()
        {
            var p = new Position(0, 0, 'C', DirectionEnum.DownUp);
            var q = new Position(0, 0, 'C', DirectionEnum.RightToLeft);

            Assert.IsTrue(p.IsEqual(q));
        }

        [Test]
        public void Position_IsEqual_False()
        {
            var p = new Position(0, 0, 'C', DirectionEnum.DownUp);
            var q = new Position(0, 2, 'C', DirectionEnum.RightToLeft);

            Assert.IsFalse(p.IsEqual(q));
        }

        [Test]
        public void AsciiMap_FindStartingPosition()
        {
            string[] lines = {"x--+",
                              "   |", 
                              "   @"};

            var asciiMap = new AsciiMap(FileHelper.LoadToCharMatrix(lines));

            var startingPosition = asciiMap.FindStartingPosition();

            Assert.IsTrue(startingPosition.Item1 == 2);
            Assert.IsTrue(startingPosition.Item2 == 3);
        }

        [Test]
        public void AsciiMap_LetterOnIntersection()
        {
            TestHelper.LetterOnIntersectionExample(out AsciiMap asciiMap, out Position previousPosition, out Position currentPosition);

            Assert.IsTrue(asciiMap.IsLetterOnIntersection(previousPosition, currentPosition));
        }

        [Test]
        public void AsciiMap_FindAvailablePositions_Count()
        {
            TestHelper.LetterOnIntersectionExample(out AsciiMap asciiMap, out Position previousPosition, out Position currentPosition);

            var availablePositions = asciiMap.FindAvailablePosition(previousPosition, currentPosition);

            Assert.IsTrue(availablePositions.Count == 3);
        }

        public void AsciiMap_FindAvailablePositions_IsPositionValid()
        {
            TestHelper.LetterOnIntersectionExample(out AsciiMap asciiMap, out Position previousPosition, out Position currentPosition);

            Assert.IsTrue(asciiMap.IsPositionValid(currentPosition));
        }
    }
}
