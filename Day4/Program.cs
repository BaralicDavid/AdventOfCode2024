// See https://aka.ms/new-console-template for more information

using System;
using System.IO;

var lines  = File.ReadAllLines("input.txt");

#region part1
void part1()
{
    var count = 0;
    for (var i = 0; i < lines.Length; i++)
    {
        for (var j = 0; j < lines[0].Length; j++)
        {
            // horizontally/vertically
            if (j + 3 < lines[0].Length
                && lines[i][j] == 'X'
                && lines[i][j + 1] == 'M'
                && lines[i][j + 2] == 'A'
                && lines[i][j + 3] == 'S')
                count++;
            if (j - 3 >= 0
                && lines[i][j] == 'X'
                && lines[i][j - 1] == 'M'
                && lines[i][j - 2] == 'A'
                && lines[i][j - 3] == 'S')
                count++;
            if (i + 3 < lines.Length
                && lines[i][j] == 'X'
                && lines[i + 1][j] == 'M'
                && lines[i + 2][j] == 'A'
                && lines[i + 3][j] == 'S')
                count++;
            if (i - 3 >= 0
                && lines[i][j] == 'X'
                && lines[i - 1][j] == 'M'
                && lines[i - 2][j] == 'A'
                && lines[i - 3][j] == 'S')
                count++;
            
            // diagonally
            if (j + 3 < lines[0].Length && i + 3 < lines.Length
                                        && lines[i][j] == 'X'
                                        && lines[i + 1][j + 1] == 'M'
                                        && lines[i + 2][j + 2] == 'A'
                                        && lines[i + 3][j + 3] == 'S')
                count++;
            if (j + 3 < lines[0].Length && i - 3 >= 0
                                        && lines[i][j] == 'X'
                                        && lines[i - 1][j + 1] == 'M'
                                        && lines[i - 2][j + 2] == 'A'
                                        && lines[i - 3][j + 3] == 'S')
                count++;
            if (j - 3 >= 0 && i + 3 < lines.Length
                           && lines[i][j] == 'X'
                           && lines[i + 1][j - 1] == 'M'
                           && lines[i + 2][j - 2] == 'A'
                           && lines[i + 3][j - 3] == 'S')
                count++;
            if (j - 3 >= 0 && i - 3 >= 0
                           && lines[i][j] == 'X'
                           && lines[i - 1][j - 1] == 'M'
                           && lines[i - 2][j - 2] == 'A'
                           && lines[i - 3][j - 3] == 'S')
                count++;
        }
    }
    Console.WriteLine("Total number is " + count);
}
#endregion

#region Part2
void part2()
{
    var count = 0;
    for (var i = 0; i < lines.Length; i++)
    {
        for (var j = 0; j < lines[0].Length; j++)
        {
            // diagonally
            if (i - 1 >= 0 && i+1 < lines.Length 
                && j - 1 >= 0 && j + 1 < lines[0].Length
                && ((lines[i][j] == 'A'
                     && lines[i - 1][j - 1] == 'M'
                     && lines[i + 1][j + 1] == 'S') ||
                    (lines[i][j] == 'A'
                     && lines[i - 1][j - 1] == 'S'
                     && lines[i + 1][j + 1] == 'M'))
                && ((lines[i][j] == 'A'
                     && lines[i - 1][j + 1] == 'M'
                     && lines[i + 1][j - 1] == 'S') || 
                    (lines[i][j] == 'A'
                     && lines[i - 1][j + 1] == 'S'
                     && lines[i + 1][j - 1] == 'M')))
                count++;
        }
    }
    Console.WriteLine("Total number is " + count);
}
#endregion

part2();