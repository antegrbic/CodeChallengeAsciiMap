using System;
using System.Collections.Generic;
using System.Text;

namespace CodeChallengeAsciiMap.Validation
{
    public enum ValidationStatusEnum
    {
        Success = 1,
        FileMissing = 2,
        UnknownCharacterEncountered = 3,
        MultipleStartPositions = 4,
        MultipleEndPositions = 5,
        MissingStartPosition = 6,
        MissingEndPosition = 7,
        MultipleDirection = 8,
        NoAvailableDirections = 9

    }
}
