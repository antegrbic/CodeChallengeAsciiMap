using System;
using System.Collections.Generic;
using System.IO;

namespace CodeChallengeAsciiMap.Utility
{
    public class FileHelper
    {
        public static long NumberOfLines(string fileName)
        {
            var lineCount = 0;
            using (var reader = File.OpenText(fileName))
            {
                while (reader.ReadLine() != null)
                {
                    lineCount++;
                }
            }

            return lineCount;
        }

        public static void CopyFileContentToTwoDimArray(string[] lines, char[][] arr)
        {
            for (int i = 0; i < lines.Length; i++)
            {
                arr[i] = new char[lines[i].Length];
                for (int j = 0; j < lines[i].Length; j++)
                {
                    arr[i][j] = lines[i][j];
                }
            }
        }

        public static string TabSanitizer(string line)
        {
            return line.Replace("\t", "    ");
        }

        public static string TabsToSpaces(string inTxt, int tabLen = 4)
        {
            var outTxt = new List<string>();

            var textValues = inTxt.Split('\t');

            foreach (var val in textValues)
            {
                var lines = val.Split('\r');
                var preTxt = lines[lines.Length - 1];
                preTxt = preTxt.Replace("\n", "");
                var numSpaces = tabLen - preTxt.Length % tabLen;
                if (numSpaces == 0)
                    numSpaces = tabLen;
                outTxt.Add(val + new string(' ', numSpaces));
            }
            return String.Join("", outTxt);
        }
    }
}
