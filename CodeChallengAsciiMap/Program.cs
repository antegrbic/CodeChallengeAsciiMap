using CodeChallengeAsciiMap.Core;
using System;

namespace CodeChallengAsciiMap
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Ascii Map Coding challenge solver. Enter the full path and filename for a .txt file containing ASCII map:");
            var fileName = Console.ReadLine();

            var solver = new AsciiMapSolver(fileName);
            solver.SolveProblem();

            Console.WriteLine($"Collected letters: {solver.CollectedLetters}");
            Console.WriteLine($"Path as string: {solver.PathAsString}");
        }      
    }
}
