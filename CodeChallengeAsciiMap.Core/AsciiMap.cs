using CodeChallengeAsciiMap.Core.Interfaces;
using CodeChallengeAsciiMap.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeChallengeAsciiMap.Core
{
    public class AsciiMap : ITwoDimMap
    {
        private char[][] _collection;

        public char[][] Collection
        {
            get
            {
                return _collection;
            }
        }

        public AsciiMap() { }
        
        public AsciiMap(char[][] collection)
        {
            _collection = collection;
        }

        public void SetMap(char[][] collection)
        {
            _collection = collection;
        }

        public List<Position> FindAvailablePosition(Position previousPosition, Position currentPosition)
        {
            var possiblePosition = new List<Position>
                        {
                            TryAndGetPositionFromMatrix(currentPosition.i, currentPosition.j - 1, DirectionEnum.RightToLeft),
                            TryAndGetPositionFromMatrix(currentPosition.i, currentPosition.j + 1, DirectionEnum.LeftToRight),
                            TryAndGetPositionFromMatrix(currentPosition.i + 1, currentPosition.j, DirectionEnum.UpDown),
                            TryAndGetPositionFromMatrix(currentPosition.i - 1, currentPosition.j, DirectionEnum.DownUp)
                        };

            return possiblePosition.FindAll(x => IsValid(previousPosition, x));
        }

        public Position Next()
        {
            throw new NotImplementedException();
        }

        public Position TryAndGetNextPosition(Position currentPosition)
        {
            switch (currentPosition.Direction)
            {
                case DirectionEnum.RightToLeft:
                    return TryAndGetPositionFromMatrix(currentPosition.i, currentPosition.j - 1, DirectionEnum.RightToLeft);
                case DirectionEnum.LeftToRight:
                    return TryAndGetPositionFromMatrix(currentPosition.i, currentPosition.j + 1, DirectionEnum.LeftToRight);
                case DirectionEnum.UpDown:
                    return TryAndGetPositionFromMatrix(currentPosition.i + 1, currentPosition.j, DirectionEnum.UpDown);
                case DirectionEnum.DownUp:
                    return TryAndGetPositionFromMatrix(currentPosition.i - 1, currentPosition.j, DirectionEnum.DownUp);
                default:
                    break;
            }

            return null;
        }

        public Position TryAndGetPositionFromMatrix(int k, int l, DirectionEnum direction)
        {
            if ((0 <= k && k < _collection.GetLength(0)) && (0 <= l && l < _collection[k].Length))
            {
                return new Position(k, l, _collection[k][l], direction);
            }

            return new Position(k, l, null, direction);
        }

        public Tuple<int, int> FindStartingPosition()
        {
            int k = 0;
            int l = 0;

            for (int i = 0; i < _collection.GetLength(0); i += 1)
            {
                for (int j = 0; j < _collection[i].Length; j += 1)
                {
                    if (_collection[i][j] == MapHelper.startingPositionChar)
                    {
                        k = i;
                        l = j;

                        return new Tuple<int, int>(k, l);
                    }
                }
            }

            return new Tuple<int, int>(k, l);
        }

        public bool IsValid(Position previousPosition, Position currentPosition)
        {
            return !currentPosition.IsEqual(previousPosition)
                && currentPosition.Character != null
                && (MapHelper.PossibleEdgeValues.Contains((char)currentPosition.Character) || IsLetter((char)currentPosition.Character));
        }

        public bool IsPositionValid(Position p)
        {
            return p.Character != null
                && (MapHelper.PossibleEdgeValues.Contains((char)p.Character) || IsLetter((char)p.Character));
        }

        public bool IsLetter(char character)
        {
            return MapHelper.letters.Contains(character);
        }

        public bool IsLetterOnIntersection(Position previousPosition, Position currentPosition)
        {
            var availablePositions = FindAvailablePosition(previousPosition, currentPosition);
            return availablePositions.Count > 1 && IsLetter((char)currentPosition.Character) && availablePositions.Exists(x => x.Direction == previousPosition.Direction);
        }
    }
}
