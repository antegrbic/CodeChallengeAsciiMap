using CodeChallengeAsciiMap.Core;
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
                InitializeSolver(fileName, out AsciiMapSolver solver);

                var result = solver.SolveProblem();

                if(result.ValidationStatus == ValidationStatusEnum.Success)
                {
                    Console.WriteLine($"Collected letters: {solver.CollectedLetters}");
                    Console.WriteLine($"Path as string: {solver.PathAsString}");
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

        public static void InitializeSolver(string fileName, out AsciiMapSolver solver)
        {
            solver = new AsciiMapSolver(new Validator());
            solver.SetFile(fileName);
        }

        public static void PrintErrorMessage(ValidationResult result)
        {
            Console.WriteLine("Error while processing - see more detailed error message below");
            Console.WriteLine($"{result.ValidationStatus.ToString() } - {result.AdditionalMessage}.");
        }
    }
}
