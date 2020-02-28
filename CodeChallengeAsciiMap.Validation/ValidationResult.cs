using System;
using System.Collections.Generic;
using System.Text;

namespace CodeChallengeAsciiMap.Validation
{
    public class ValidationResult
    {
        public ValidationStatusEnum ValidationStatus;
        public string AdditionalMessage;

        public ValidationResult() { }

        public ValidationResult(ValidationStatusEnum validationStatus, string additionalMessage)
        {
            ValidationStatus = validationStatus;
            AdditionalMessage = additionalMessage;
        }

        public bool IsFailed()
        {
            return ValidationStatus != ValidationStatusEnum.Success;
        }
    }
}
