using CodeChallengeAsciiMap.Core;
using CodeChallengeAsciiMap.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeChallengeAsciiMap.Tests.Utility
{
    public static class TestHelper
    {
        public static void LetterOnIntersectionExample(out AsciiMap asciiMap, out Position previousPosition, out Position currentPosition)
        {
            string[] lines = {"x--+ ",
                              "   | ",
                              "+--A--x ",
                              "|  |    ",
                              "+--+  "};

            asciiMap = new AsciiMap(FileHelper.LoadToCharMatrix(lines));

            previousPosition = new Position(1, 3, '|', DirectionEnum.UpDown);
            currentPosition = new Position(2, 3, 'A', DirectionEnum.UpDown);
        }
    }
}
