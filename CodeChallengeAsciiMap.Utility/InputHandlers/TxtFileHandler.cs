using CodeChallengeAsciiMap.Utility.Interfaces;
using System;
using System.IO;


namespace CodeChallengeAsciiMap.Utility
{
    public class TxtFileHandler : IInputHandler
    {
        string _fileName;

        public string FileName
        {
            get
            {
                return _fileName;
            }

            set
            {
                _fileName = value;
            }
        }

        public TxtFileHandler() { }

        public TxtFileHandler(string fileName)
        {
            SetFile(fileName);
        }

        public void SetFile(string fileName)
        {
            FileName = fileName;

            if (!File.Exists(fileName))
                throw new Exception("File missing from given location!");
        }

        public char[][] LoadToCharMatrix()
        {
            if(String.IsNullOrEmpty(FileName))
                throw new Exception("File name was not provided!"); 

            var lines = LoadFile();
            return FileHelper.LoadToCharMatrix(lines);
        }

        private string[] LoadFile()
        {
            var lines = File.ReadAllLines(_fileName);

            TabsToSpaces(lines);

            return lines;
        }

        private void TabsToSpaces(string[] lines)
        {
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = FileHelper.TabsToSpaces(lines[i]);
            }
        }
    }
}
