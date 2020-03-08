using System;
using System.Collections.Generic;
using System.Text;

namespace CodeChallengeAsciiMap.Core.Interfaces
{
    interface ITwoDimMap
    {
        Position Next();
        Position TryAndGetNextPosition(Position currentPosition);
        Position TryAndGetPositionFromMatrix(int k, int l, DirectionEnum direction);
        Tuple<int, int> FindStartingPosition();
        List<Position> FindAvailablePosition(Position previousPosition, Position currentPosition);
        bool IsPositionValid(Position p);
        bool IsLetter(char character);
    }
}
