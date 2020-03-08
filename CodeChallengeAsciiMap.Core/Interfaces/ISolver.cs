using CodeChallengeAsciiMap.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeChallengeAsciiMap.Core.Interfaces
{
    public interface ISolver
    {
        ValidationResult SolveProblem();
    }
}
