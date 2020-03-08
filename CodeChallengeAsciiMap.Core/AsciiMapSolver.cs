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

        public AsciiMap _asciiMap;

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

        public AsciiMapSolver(IValidator validator, AsciiMap asciiMap)
        {
            _validator = validator;
            _asciiMap = asciiMap;
        }

        public ValidationResult SolveProblem()
        {
            var initValidationResult = Init();
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

        private ValidationResult Init()
        {
            var validationResult = ValidateStartEndExist();
            if (validationResult.IsFailed()) return validationResult;

            InitializeStartingPosition();

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
            var availablePositions = _asciiMap.FindAvailablePosition(previousPosition, currentPosition);

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
            UpdatePositions(currentPosition, _asciiMap.TryAndGetNextPosition(currentPosition));

            if (!_asciiMap.IsPositionValid(currentPosition))
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

        private ValidationResult AdjustPositions(List<Position> availablePositions)
        {
            if (_asciiMap.IsLetterOnIntersection(previousPosition, currentPosition))
                return HandleLetterIntersection(availablePositions);

            UpdatePositions(currentPosition, availablePositions[0]);
            return new ValidationResult(ValidationStatusEnum.Success, "Success");
        }

        private ValidationResult ValidateAvailablePositions(List<Position> availablePositions)
        {
            if (availablePositions.Count == 0)
                return new ValidationResult(ValidationStatusEnum.NoAvailableDirections, $"No available directions from ({currentPosition.i},{currentPosition.j}) position!");

            if (availablePositions.Count > 1 && !(_asciiMap.IsLetterOnIntersection(previousPosition, currentPosition)))
                return new ValidationResult(ValidationStatusEnum.MultipleDirection, $"Multiple directions from ({ currentPosition.i },{ currentPosition.j}) position!");

            return new ValidationResult(ValidationStatusEnum.Success, "Success");
        }

        private void UpdatePositions(Position oldCurrentPosition, Position newCurrentPosition)
        {
            previousPosition = oldCurrentPosition;
            currentPosition = newCurrentPosition;
        }

        private ValidationResult ValidateStartEndExist()
        {
            return _validator.ValidateStartAndEndCharacterExists(_asciiMap.Collection);
        }

        private void AdjustCollectedLetters()
        {
            if (_asciiMap.IsLetter((char)currentPosition.Character) && !PreviouslyAddedLetter(currentPosition))
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

        private void InitializeStartingPosition()
        {
            var startingCoordinates = _asciiMap.FindStartingPosition();

            previousPosition = new Position(-1, -1, null, DirectionEnum.StartingPosition);
            currentPosition = new Position(startingCoordinates.Item1, startingCoordinates.Item2, MapHelper.startingPositionChar, DirectionEnum.StartingPosition);
        }
    }
}
