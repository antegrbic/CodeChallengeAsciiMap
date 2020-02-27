using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CodeChallengeAsciiMap.Core.Helper;
using CodeChallengeAsciiMap.Core.Interfaces;

namespace CodeChallengeAsciiMap.Core
{
    public class AsciiMapSolver : ISolver
    {
        private List<Position> _collectedLetters = new List<Position>();
        private string _pathAsString = String.Empty;

        public string CollectedLetters
        {
            get
            {
                return string.Join("", _collectedLetters.Select(p => p.Character));
            }
        }

        public string PathAsString => _pathAsString;

        private char[][] textAsMatrix;

        private Position currentPosition;
        private Position previousPosition;

        public AsciiMapSolver() { }

        public AsciiMapSolver(string fileName)
        {
            string[] lines = File.ReadAllLines(fileName);

            ValidateStartAndEndCharacterExists(lines);

            textAsMatrix = new char[FileHelper.NumberOfLines(fileName)][];

            FileHelper.CopyFileContentToTwoDimArray(lines, textAsMatrix);
        }

        public void SolveProblem()
        {
            FindStartingPosition(out int k, out int l);

            InitializeStartingPosition(k, l);

            while (currentPosition.Character != MapHelper.endPositionChar)
            {
                DecideNextMove();
         
                AdjustPath();
            }

            FinishPath();
        }

        private void DecideNextMove()
        {
            switch (currentPosition.Character)
            {

                case MapHelper.startingPositionChar:
                case MapHelper.intersectionChar:
                case var c when MapHelper.letters.Contains((char)currentPosition.Character):

                    HandleNodes();

                    break;

                case MapHelper.verticalChar:
                case MapHelper.horizontalChar:

                    HandleEdges();

                    break;
                default:
                    throw new Exception($"Invalid field {currentPosition.Character} encountered at position ({currentPosition.i},{currentPosition.j})");
            }
        }

        public void AdjustPath()
        {
            _pathAsString += previousPosition.Character;
        }

        public void FinishPath()
        {
            _pathAsString += MapHelper.endPositionChar;
        }

        private void HandleNodes()
        {
            var possiblePosition = new List<Position>
                        {
                            TryAndGetPositionFromMatrix(currentPosition.i, currentPosition.j - 1, DirectionEnum.RightToLeft),
                            TryAndGetPositionFromMatrix(currentPosition.i, currentPosition.j + 1, DirectionEnum.LeftToRight),
                            TryAndGetPositionFromMatrix(currentPosition.i + 1, currentPosition.j, DirectionEnum.UpDown),
                            TryAndGetPositionFromMatrix(currentPosition.i - 1, currentPosition.j, DirectionEnum.DownUp)
                        };

            var validPositions = possiblePosition.FindAll(x => IsValid(previousPosition, x));

            if (IsLetter((char)currentPosition.Character) && !PreviouslyAddedLetter(currentPosition))
                _collectedLetters.Add(currentPosition);

            if (validPositions.Count == 0)
                throw new Exception($"No available directions from ({currentPosition.i},{currentPosition.j}) position!");

            // special case where alphabetic letter is in middle of an intersection
            // travelling must continue in same direction as in the previous position
            if (validPositions.Count > 1 && IsLetter((char)currentPosition.Character) && validPositions.Exists(x => x.Direction == previousPosition.Direction))
            {
                var position = validPositions.FindAll(x => x.Direction == previousPosition.Direction);
                if (position.Count != 1)
                    throw new Exception($"No available directions from ({currentPosition.i},{currentPosition.j}) position!");

                previousPosition = currentPosition;
                currentPosition = position[0];
            }
            else if (validPositions.Count > 1)
                throw new Exception($"Multiple directions from  ({currentPosition.i},{currentPosition.j}) position!");
            else
            {
                previousPosition = currentPosition;
                currentPosition = validPositions[0];
            }
        }

        private void HandleEdges()
        {
            previousPosition = currentPosition;
            currentPosition = TryAndGetNextPosition(currentPosition.i, currentPosition.j, currentPosition.Direction);

            if (!IsValid(currentPosition))
                throw new Exception($"Invalid field {currentPosition.Character} encountered at position ({currentPosition.i},{currentPosition.j})");
        }

        private bool PreviouslyAddedLetter(Position p)
        {
            return _collectedLetters.Exists(x => x.i == p.i && x.j == p.j && x.Character == p.Character);
        }

        private void InitializeStartingPosition(int k, int l)
        {
            previousPosition = new Position(-1, -1, null, DirectionEnum.StartingPosition);
            currentPosition = new Position(k, l, MapHelper.startingPositionChar, DirectionEnum.StartingPosition);
        }

        private bool IsValid(Position previousPosition, Position currentPosition)
        {
            return (!currentPosition.IsEqual(previousPosition) && currentPosition.Character != null && ((MapHelper.PossibleEdgeValues.Contains((char)currentPosition.Character) || IsLetter((char)currentPosition.Character))));
        }

        private bool IsValid(Position p)
        {
            return (p.Character != null && ((MapHelper.PossibleEdgeValues.Contains((char)p.Character) || IsLetter((char)p.Character))));
        }

        private bool IsLetter(char character)
        {
            return MapHelper.letters.Contains(character);
        }

        private Position TryAndGetNextPosition(int i, int j, DirectionEnum direction)
        {
            switch (direction)
            {
                case DirectionEnum.RightToLeft:
                    return TryAndGetPositionFromMatrix(i, j - 1, DirectionEnum.RightToLeft);
                case DirectionEnum.LeftToRight:
                    return TryAndGetPositionFromMatrix(i, j + 1, DirectionEnum.LeftToRight);
                case DirectionEnum.UpDown:
                    return TryAndGetPositionFromMatrix(i + 1, j, DirectionEnum.UpDown);
                case DirectionEnum.DownUp:
                    return TryAndGetPositionFromMatrix(i - 1, j, DirectionEnum.DownUp);
                default:
                    break;
            }

            return null;
        }

        internal Position TryAndGetPositionFromMatrix(int k, int l, DirectionEnum direction)
        {
            if ((0 <= k && k < textAsMatrix.GetLength(0)) && (0 <= l && l < textAsMatrix[k].Length))
            {
                return new Position(k, l, textAsMatrix[k][l], direction);
            }

            return new Position(k, l, null, direction);
        }

        internal void FindStartingPosition(out int k, out int l)
        {
            k = 0; l = 0;

            for (int i = 0; i < textAsMatrix.GetLength(0); i += 1)
            {
                for (int j = 0; j < textAsMatrix[i].Length; j += 1)
                {
                    if (textAsMatrix[i][j] == MapHelper.startingPositionChar)
                    {
                        k = i;
                        l = j;

                        return;
                    }
                }
            }
        }

        public static void ValidateStartAndEndCharacterExists(string[] lines)
        {
            int startCharNum = 0;
            int endCharNum = 0;

            foreach (var line in lines)
            {
                if (line.Contains(MapHelper.startingPositionChar)) startCharNum++;
                if (line.Contains(MapHelper.endPositionChar)) endCharNum++;
            }

            if (startCharNum == 0)
                throw new Exception("Couldn't find start position!");
            else if (startCharNum > 1)
                throw new Exception("Multiple start position detected!");

            if (endCharNum == 0)
                throw new Exception("Couldn't find end position!");
            else if (endCharNum > 1)
                throw new Exception("Multiple end position detected!");
        }
    }
}
