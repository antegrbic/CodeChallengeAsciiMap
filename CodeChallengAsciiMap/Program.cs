using CodeChallengeAsciiMap.Core;
using CodeChallengeAsciiMap.Utility;
using CodeChallengeAsciiMap.Validation;
using System;

namespace CodeChallengAsciiMap
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Ascii Map Coding challenge solver. Enter the full path and filename for a .txt file containing ASCII map:");
            var fileName = Console.ReadLine();

            try
            {
                var solverFacade = new SolverFacade();
                var result = solverFacade.ProcessFile(fileName);

                if(result.ValidationStatus == ValidationStatusEnum.Success)
                {
                    Console.WriteLine($"Collected letters: {solverFacade.CollectedLetters}");
                    Console.WriteLine($"Path as string: {solverFacade.PathAsString}");
                    return;
                }

                PrintErrorMessage(result);
            }
            catch(Exception e)
            {
                Console.WriteLine("Application encountered an unexpected error - see more detailed error message below");
                Console.WriteLine(e.Message);
            }
        }

        public static void PrintErrorMessage(ValidationResult result)
        {
            Console.WriteLine("Error while processing - see more detailed error message below");
            Console.WriteLine($"{result.ValidationStatus.ToString() } - {result.AdditionalMessage}.");
        }
    }
}
