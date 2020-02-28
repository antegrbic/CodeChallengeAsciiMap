using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CodeChallengeAsciiMap.Core.Interfaces;
using CodeChallengeAsciiMap.Utility;
using CodeChallengeAsciiMap.Validation;
using CodeChallengeAsciiMap.Validation.Interfaces;

namespace CodeChallengeAsciiMap.Core
{
    /// <summary>Class <c>AsciiMapSolver</c> solves the problem of finding path given in textual file 
    /// that begins with @ and ends with x. Output is given through properties PathAsString and CollectedLetters.
    /// </summary>
    ///
    public class AsciiMapSolver : ISolver
    {
        private IValidator _validator;

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

        private string _fileName;

        public AsciiMapSolver() { }

        public AsciiMapSolver(IValidator validator)
        {
            _validator = validator;
        }

        public ValidationResult SolveProblem()
        {
            var initValidationResult = Init();

            if (initValidationResult.IsFailed())
                return initValidationResult;

            while (currentPosition.Character != MapHelper.endPositionChar)
            {
                //numerous validations are done in every step
                //processing is stopped in case of errors detected in data 
                var validationResult = DecideNextMove();

                if (validationResult.ValidationStatus != ValidationStatusEnum.Success)
                    return validationResult;

                AdjustPath();
            }

            FinishPath();

            return new ValidationResult(ValidationStatusEnum.Success, "Success");
        }

        private ValidationResult Init()
        {
            var validationResult = LoadFileToArray();

            // we immediately stop processing and notify SolveProblem about issue
            if (validationResult.IsFailed())
                return validationResult;

            var startingCoordinates = FindStartingPosition();

            InitializeStartingPosition(startingCoordinates);

            return validationResult;
        }


        /// <summary> 
        /// Method decides next position based on position type - letters, starting position and + are considered nodes.
        /// - and | are considered as bridges. In nodes case we consider all directions around current position. 
        /// In case of edges we consider we continue to traverse in same direction as in the previous position.
        /// </summary>
        ///
        private ValidationResult DecideNextMove()
        {
            //with ValidationResult we notify caller method if any unrecoverable errors were encountered during processing
            var validationResult = new ValidationResult();

            switch (currentPosition.Character)
            {

                case MapHelper.startingPositionChar:
                case MapHelper.intersectionChar:
                case var c when MapHelper.letters.Contains((char)currentPosition.Character):

                    validationResult = HandleNodes();

                    break;

                case MapHelper.verticalChar:
                case MapHelper.horizontalChar:

                    validationResult = HandleEdges();

                    break;
                default:
                    return new ValidationResult(ValidationStatusEnum.UnknownCharacterEncountered, $"Invalid field {currentPosition.Character} encountered at position ({currentPosition.i},{currentPosition.j})");
            }

            return validationResult;
        }

        private ValidationResult HandleNodes()
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
                return new ValidationResult(ValidationStatusEnum.NoAvailableDirections, $"No available directions from ({currentPosition.i},{currentPosition.j}) position!");

            // special case where alphabetic letter is in middle of an intersection
            // travelling must continue in same direction as in the previous position
            if (validPositions.Count > 1 && IsLetter((char)currentPosition.Character) && validPositions.Exists(x => x.Direction == previousPosition.Direction))
            {
                var position = validPositions.FindAll(x => x.Direction == previousPosition.Direction);
                if (position.Count != 1)
                    return new ValidationResult(ValidationStatusEnum.NoAvailableDirections, $"No available directions from ({currentPosition.i},{currentPosition.j}) position!");

                previousPosition = currentPosition;
                currentPosition = position[0];
            }
            else if (validPositions.Count > 1)
                return new ValidationResult(ValidationStatusEnum.MultipleDirection, $"Multiple directions from ({ currentPosition.i },{ currentPosition.j}) position!");
            else
            {
                previousPosition = currentPosition;
                currentPosition = validPositions[0];
            }

            return new ValidationResult(ValidationStatusEnum.Success, "Success");
        }

        private ValidationResult HandleEdges()
        {
            previousPosition = currentPosition;
            currentPosition = TryAndGetNextPosition(currentPosition.i, currentPosition.j, currentPosition.Direction);

            if (!IsValid(currentPosition))
                return new ValidationResult(ValidationStatusEnum.UnknownCharacterEncountered, $"Invalid field {currentPosition.Character} encountered at position ({currentPosition.i},{currentPosition.j})");

            return new ValidationResult(ValidationStatusEnum.Success, "Success");
        }

        private ValidationResult LoadFileToArray()
        {
            if (!_validator.ValidateFileExists(_fileName))
                return new ValidationResult(ValidationStatusEnum.FileMissing, "File is missing from given location!");

            var lines = File.ReadAllLines(_fileName);

            var validationResult = _validator.ValidateStartAndEndCharacterExists(lines);
            if (validationResult.ValidationStatus != ValidationStatusEnum.Success)
                return validationResult;

            textAsMatrix = new char[FileHelper.NumberOfLines(_fileName)][];

            FileHelper.CopyFileContentToTwoDimArray(lines, textAsMatrix);
            return validationResult;
        }

        public void SetFile(string fileName)
        {
            _fileName = fileName;
        }

        private void AdjustPath()
        {
            _pathAsString += previousPosition.Character;
        }

        private void FinishPath()
        {
            _pathAsString += MapHelper.endPositionChar;
        }

        private bool PreviouslyAddedLetter(Position p)
        {
            return _collectedLetters.Exists(x => x.i == p.i && x.j == p.j && x.Character == p.Character);
        }

        private void InitializeStartingPosition(Tuple<int, int> coordinates)
        {
            previousPosition = new Position(-1, -1, null, DirectionEnum.StartingPosition);
            currentPosition = new Position(coordinates.Item1, coordinates.Item2, MapHelper.startingPositionChar, DirectionEnum.StartingPosition);
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

        private Position TryAndGetPositionFromMatrix(int k, int l, DirectionEnum direction)
        {
            if ((0 <= k && k < textAsMatrix.GetLength(0)) && (0 <= l && l < textAsMatrix[k].Length))
            {
                return new Position(k, l, textAsMatrix[k][l], direction);
            }

            return new Position(k, l, null, direction);
        }

        private Tuple<int, int> FindStartingPosition()
        {
            int k = 0;
            int l = 0;

            for (int i = 0; i < textAsMatrix.GetLength(0); i += 1)
            {
                for (int j = 0; j < textAsMatrix[i].Length; j += 1)
                {
                    if (textAsMatrix[i][j] == MapHelper.startingPositionChar)
                    {
                        k = i;
                        l = j;

                        return new Tuple<int, int>(k, l);
                    }
                }
            }

            return new Tuple<int, int>(k, l);
        }
    }
}
