using System;
using System.Collections.Generic;
using System.Text;

namespace CodeChallengeAsciiMap.Core
{
    /// <summary>Class <c>Position</c> models a single position in a two-dimensional
    /// array with information from which direction we came to this position.</summary>
    ///
    public class Position
    {
        internal int i;
        internal int j;
        internal DirectionEnum Direction;

        internal char? Character;

        public Position() { }

        public Position(int i, int j, char? character, DirectionEnum direction)
        {
            this.i = i;
            this.j = j;
            this.Character = character;
            this.Direction = direction;
        }

        public bool IsEqual(Position second)
        {
            //Direction intentionally left out from comparison - in travelling through the path this way we avoid returning to the same spot
            return this.i == second.i && j == second.j && Character == second.Character;
        }
    }
}
