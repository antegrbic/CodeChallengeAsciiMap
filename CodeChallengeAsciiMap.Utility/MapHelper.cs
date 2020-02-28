using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeChallengeAsciiMap.Utility
{
    public static class MapHelper
    {
        public const char intersectionChar = '+';
        public const char verticalChar = '|';
        public const char horizontalChar = '-';
        public const char endPositionChar = 'x';
        public const char startingPositionChar = '@';

        public static List<char> letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray().ToList();

        public static List<char> PossibleEdgeValues = new List<char> { intersectionChar, verticalChar, horizontalChar, endPositionChar };
    }
}
