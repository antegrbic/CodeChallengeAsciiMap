using CodeChallengeAsciiMap.Utility;
using CodeChallengeAsciiMap.Validation.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CodeChallengeAsciiMap.Validation
{
    public class Validator : IValidator
    {        
        public Validator() { }

        public bool ValidateFileExists(string fileName)
        {
            if (File.Exists(fileName))
            {
                return true;
            }
            else return false;
        }

        public ValidationResult ValidateStartAndEndCharacterExists(string[] lines)
        {
            int startCharNum = 0;
            int endCharNum = 0;

            foreach (var line in lines)
            {
                if (line.Contains(MapHelper.startingPositionChar)) startCharNum++;
                if (line.Contains(MapHelper.endPositionChar)) endCharNum++;
            }

            if (startCharNum == 0)
                return new ValidationResult(ValidationStatusEnum.MissingStartPosition, "Couldn't find start position!");
            else if (startCharNum > 1)
                return new ValidationResult(ValidationStatusEnum.MultipleStartPositions, "Multiple start position detected!");

            if (endCharNum == 0)
                return new ValidationResult(ValidationStatusEnum.MissingEndPosition, "Couldn't find end position!");
            else if (endCharNum > 1)
                return new ValidationResult(ValidationStatusEnum.MultipleEndPositions, "Multiple end position detected!");

            return new ValidationResult(ValidationStatusEnum.Success, "Success");
        }
    }
}
