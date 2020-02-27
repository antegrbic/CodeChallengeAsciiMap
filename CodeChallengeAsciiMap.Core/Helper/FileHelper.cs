﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CodeChallengeAsciiMap.Core.Helper
{
    class FileHelper
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
    }
}