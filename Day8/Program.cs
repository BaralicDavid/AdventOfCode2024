// See https://aka.ms/new-console-template for more information

using System;
using System.Collections.Generic;
using System.IO;

var lines = File.ReadAllLines("input.txt");
var antennasDict = new Dictionary<char, List<(int, int)>>();
var antinodeSet = new HashSet<(int, int)>();

for (int lineIndex = 0; lineIndex < lines.Length; lineIndex++)
{
    var line = lines[lineIndex];
    for (int charIndex = 0; charIndex < line.Length; charIndex++)
    {
        var character = line[charIndex];
        if (character != '.')
        {
            if (!antennasDict.ContainsKey(character))
                antennasDict[character] = new List<(int, int)>();

            antennasDict[character].Add((lineIndex, charIndex));
        }
    }
}

#region Part 1
(int, int) GetAntiNodePositionPart1((int, int) antennaPos1, (int, int) antennaPos2)
{
    var mapH = lines.Length;
    var mapW = lines[0].Length;

    var y = antennaPos2.Item1 + (antennaPos2.Item1 - antennaPos1.Item1);
    var x = antennaPos2.Item2 + (antennaPos2.Item2 - antennaPos1.Item2);
    if (y >= 0 && y < mapH && x >= 0 && x < mapW)
        return (y, x);
    else
        return (-1, -1);
}

void Part1()
{
    foreach (var (_, positions) in antennasDict)
    {
        foreach (var position1 in positions)
        {
            foreach (var position2 in positions)
            {
                if (position1 != position2)
                {
                    var antinodePos = GetAntiNodePositionPart1(position1, position2);
                    if (antinodePos != (-1, -1))
                        antinodeSet.Add(antinodePos);
                }
            }
        }
    }

    Console.WriteLine("Number of antinode locations: " + antinodeSet.Count);   
}
# endregion

#region Part 2

// Doesn't work for final test case
List<(int, int)> GetAntiNodePositionsPart2((int, int) antennaPos1, (int, int) antennaPos2)
{
    var mapH = lines.Length;
    var mapW = lines[0].Length;
    var positions = new List<(int, int)>();

    // line eq
    // y = slope * x + ycross
    var slope = (double)(antennaPos2.Item1 - antennaPos1.Item1) / (antennaPos2.Item2 - antennaPos1.Item2);
    var ycross = antennaPos2.Item1 - slope * antennaPos2.Item2;
    for (var x = 0; x < lines[0].Length; x++)
    {
        var y = Math.Round(x * slope + ycross, 4);
        if (y - Math.Floor(y) == 0 && y >= 0 && y < lines.Length) 
            positions.Add(((int)y, x));
    }

    // if there is no third dot on the line, no positions are antinodes
    if (positions.Count == 2)
    {
        positions.Clear();
    }

    return positions;
}

List<(int, int)> GetAntiNodePositionsPart2SecondTry((int, int) antennaPos1, (int, int) antennaPos2)
{ 
    var mapH = lines.Length;
    var mapW = lines[0].Length;
    var positions = new List<(int, int)>();
    positions.Add(antennaPos1);
    positions.Add(antennaPos2);
    
    var y = antennaPos2.Item1 + (antennaPos2.Item1 - antennaPos1.Item1);
    var x = antennaPos2.Item2 + (antennaPos2.Item2 - antennaPos1.Item2);
    var prevY = antennaPos2.Item1;
    var prevX = antennaPos2.Item2;
    
    while (y >= 0 && y < mapH && x >= 0 && x < mapW)
    {
        positions.Add((y,x));
        var tempY = y;
        var tempX = x;
        y = y + (y - prevY);
        x = x + (x - prevX);
        prevY = tempY;
        prevX = tempX;
    }
    
    if (positions.Count == 2)
        positions.Clear();

    return positions;
}

void Part2()
{
    foreach (var (_, positions) in antennasDict)
    {
        foreach (var position1 in positions)
        {
            foreach (var position2 in positions)
            {
                if (position1 != position2)
                {                
                    antinodeSet.UnionWith(GetAntiNodePositionsPart2SecondTry(position1, position2));
                }
            }
        }
    }

    Console.WriteLine("Number of antinode locations: " + antinodeSet.Count);   
}
#endregion


void PrintAntiNodesMap()
{
    for (var i = 0; i < lines.Length; Console.WriteLine(), i++)
    for (var j = 0; j < lines[0].Length; j++)
    {
        if( antinodeSet.Contains((i,j)))
            Console.Write('#');
        else
            Console.Write('.');
    }
}

Part2();
PrintAntiNodesMap();

// GetAntiNodePositionsPart2((4, 4), (3, 7));


