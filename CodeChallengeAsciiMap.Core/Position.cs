using System;
using System.Collections.Generic;
using System.Text;

namespace CodeChallengeAsciiMap.Core
{
    public class Position
    {
        internal int i;
        internal int j;
        internal DirectionEnum Direction;

        internal char? Character;

        public Position() { }

        public Position(int i, int j, char? Character, DirectionEnum direction)
        {
            this.i = i;
            this.j = j;
            this.Character = Character;
            this.Direction = direction;
        }

        public bool IsEqual(Position second)
        {
            return this.i == second.i && j == second.j && Character == second.Character;
        }
    }
}
