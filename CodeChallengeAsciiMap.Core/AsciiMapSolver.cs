using System;
using System.Collections.Generic;
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
    public class AsciiMapSolver : ISolver
    {
        private IValidator _validator;

        private List<Position> _collectedLetters = new List<Position>();
        private string _pathAsString = String.Empty;

        private char[][] textAsMatrix;

        private Position currentPosition;
        private Position previousPosition;

        public AsciiMapSolver() { }

        public string CollectedLetters
        {
            get
            {
                return string.Join("", _collectedLetters.Select(p => p.Character));
            }
        }

        public string PathAsString => _pathAsString;  

        public AsciiMapSolver(IValidator validator)
        {
            _validator = validator;
        }

        public ValidationResult SolveProblem(char[][] data)
        {
            var initValidationResult = Init(data);
            if (initValidationResult.IsFailed())
                return initValidationResult;

            while (currentPosition.Character != MapHelper.endPositionChar)
            {
                var validationResult = DecideNextMove();

                if (validationResult.IsFailed())
                    return validationResult;

                AdjustPath();
            }

            FinishPath();

            return new ValidationResult(ValidationStatusEnum.Success, "Success");
        }

        private ValidationResult Init(char[][] data)
        {
            textAsMatrix = data;

            var validationResult = ValidateStartEndExist();
            if (validationResult.IsFailed()) return validationResult;

            var startingCoordinates = FindStartingPosition();

            InitializeStartingPosition(startingCoordinates);

            return new ValidationResult(ValidationStatusEnum.Success, "Success");
        }


        /// <summary> 
        /// Method decides next position based on position type - letters, starting position and + are considered nodes.
        /// - and | are considered as bridges. In nodes case we consider all directions around current position. 
        /// In case of edges we consider we continue to traverse in same direction as in the previous position.
        /// </summary>
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
            var availablePositions = FindAvailablePosition();

            AdjustCollectedLetters();

            var validationResult = ValidateAvailablePositions(availablePositions);
            if (validationResult.IsFailed())
                return validationResult;

            var adjustValidationResult = AdjustPositions(availablePositions);
            if (adjustValidationResult.IsFailed())
                return adjustValidationResult;

            return new ValidationResult(ValidationStatusEnum.Success, "Success");
        }

        private ValidationResult HandleEdges()
        {
            UpdatePositions(currentPosition, TryAndGetNextPosition(currentPosition.i, currentPosition.j, currentPosition.Direction));

            if (!IsValid(currentPosition))
                return new ValidationResult(ValidationStatusEnum.UnknownCharacterEncountered, $"Invalid field {currentPosition.Character} encountered at position ({currentPosition.i},{currentPosition.j})");

            return new ValidationResult(ValidationStatusEnum.Success, "Success");
        }

        private ValidationResult HandleLetterIntersection(List<Position> validPositions)
        {
            var position = validPositions.FindAll(x => x.Direction == previousPosition.Direction);

            var validationResult = ValidateAvailablePositions(position);

            UpdatePositions(currentPosition, position[0]);
            return validationResult;
        }

        private List<Position> FindAvailablePosition()
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

        private ValidationResult AdjustPositions(List<Position> availablePositions)
        {
            if (IsLetterOnIntersection(availablePositions))
                return HandleLetterIntersection(availablePositions);

            UpdatePositions(currentPosition, availablePositions[0]);
            return new ValidationResult(ValidationStatusEnum.Success, "Success");
        }

        private ValidationResult ValidateAvailablePositions(List<Position> availablePositions)
        {
            if (availablePositions.Count == 0)
                return new ValidationResult(ValidationStatusEnum.NoAvailableDirections, $"No available directions from ({currentPosition.i},{currentPosition.j}) position!");

            if (availablePositions.Count > 1 && !(IsLetter((char)currentPosition.Character) && availablePositions.Exists(x => x.Direction == previousPosition.Direction)))
                return new ValidationResult(ValidationStatusEnum.MultipleDirection, $"Multiple directions from ({ currentPosition.i },{ currentPosition.j}) position!");

            return new ValidationResult(ValidationStatusEnum.Success, "Success");
        }

        private bool IsLetterOnIntersection(List<Position> availablePositions)
        {
            return availablePositions.Count > 1 && IsLetter((char)currentPosition.Character) && availablePositions.Exists(x => x.Direction == previousPosition.Direction);
        }

        private void UpdatePositions(Position oldCurrentPosition, Position newCurrentPosition)
        {
            previousPosition = oldCurrentPosition;
            currentPosition = newCurrentPosition;
        }

        private ValidationResult ValidateStartEndExist()
        {
            return _validator.ValidateStartAndEndCharacterExists(textAsMatrix);
        }

        private void AdjustCollectedLetters()
        {
            if (IsLetter((char)currentPosition.Character) && !PreviouslyAddedLetter(currentPosition))
                _collectedLetters.Add(currentPosition);
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
            return _collectedLetters.Exists(x => x.IsEqual(p) );
        }

        private void InitializeStartingPosition(Tuple<int, int> coordinates)
        {
            previousPosition = new Position(-1, -1, null, DirectionEnum.StartingPosition);
            currentPosition = new Position(coordinates.Item1, coordinates.Item2, MapHelper.startingPositionChar, DirectionEnum.StartingPosition);
        }

        private bool IsValid(Position previousPosition, Position currentPosition)
        {
            return !currentPosition.IsEqual(previousPosition) 
                && currentPosition.Character != null 
                && (MapHelper.PossibleEdgeValues.Contains((char)currentPosition.Character) || IsLetter((char)currentPosition.Character));
        }

        private bool IsValid(Position p)
        {
            return p.Character != null 
                && (MapHelper.PossibleEdgeValues.Contains((char)p.Character) || IsLetter((char)p.Character));
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
