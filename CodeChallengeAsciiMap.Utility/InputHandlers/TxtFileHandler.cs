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
            var lines = LoadFile();
            var arr = new char[FileHelper.NumberOfLines(_fileName)][];

            for (int i = 0; i < lines.Length; i++)
            {
                arr[i] = new char[lines[i].Length];
                for (int j = 0; j < lines[i].Length; j++)
                {
                    arr[i][j] = lines[i][j];
                }
            }

            return arr;
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
