using CodeChallengeAsciiMap.Core;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeChallengeAsciiMap.Tests
{
    class CodeChallengeAsciiMapCoreTests
    {
        [SetUp]
        public void Setup()
        {
           
        }

        [Test]
        public void Position_IsEqual_Valid1()
        {
            var p = new Position(0, 0, 'C', DirectionEnum.DownUp);
            var q = new Position(0, 0, 'C', DirectionEnum.RightToLeft);

            Assert.IsTrue(p.IsEqual(q));
        }

        [Test]
        public void Position_IsEqual_Valid()
        {
            var p = new Position(0, 0, 'C', DirectionEnum.DownUp);
            var q = new Position(0, 2, 'C', DirectionEnum.RightToLeft);

            Assert.IsFalse(p.IsEqual(q));
        }
    }
}
