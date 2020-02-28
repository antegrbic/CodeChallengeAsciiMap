using System;
using System.Collections.Generic;
using System.Text;

namespace CodeChallengeAsciiMap.Validation.Interfaces
{
    public interface IValidator
    {
        bool ValidateFileExists(string fileName);
        ValidationResult ValidateStartAndEndCharacterExists(string[] lines);
    }
}
