using CodeChallengeAsciiMap.Core.Interfaces;
using CodeChallengeAsciiMap.Utility;
using CodeChallengeAsciiMap.Utility.Interfaces;
using CodeChallengeAsciiMap.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeChallengeAsciiMap.Core
{
    public class SolverFacade
    {
        AsciiMapSolver _solver;
        TxtFileHandler _txtFileHandler;

        public string CollectedLetters
        {
            get
            {
                return _solver.CollectedLetters;
            }
        }

        public string PathAsString
        {
            get
            {
                return _solver.PathAsString;
            }
        }

        public SolverFacade()
        {
            _txtFileHandler = new TxtFileHandler();
            _solver = new AsciiMapSolver(new Validator(), new AsciiMap());
        }

        public ValidationResult ProcessFile(string fileName)
        {
            _txtFileHandler.SetFile(fileName);
            _solver._asciiMap.SetMap(_txtFileHandler.LoadToCharMatrix());

            return _solver.SolveProblem();
        }
    }
}
