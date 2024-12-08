// See https://aka.ms/new-console-template for more information


using System;
using System.Collections.Generic;
using System.IO;

var directionDict = new Dictionary<string, (int, int)>
{
    ["up"] = (-1, 0),
    ["down"] = (1, 0),  
    ["left"] = (0, -1),
    ["right"] = (0, 1)
};

var lines = File.ReadAllLines("./input.txt");

(int, int) GetStartPosition()
{
    for (var i = 0; i < lines.Length; i++)
    {
        for (var j=0; j < lines[i].Length; j++)
            if (lines[i][j] == '^')
                return (i, j);
    }
    return (0, 0);
}

bool IsInsideTheMap((int, int) currentPosition)
{
    if (currentPosition.Item1 < 0 || currentPosition.Item1 >= lines.Length
                                  || currentPosition.Item2 < 0 || currentPosition.Item2 >= lines[0].Length)
        return false;
    return true;
}

string RotateDirection90(string direction)
{
    return direction switch
    {
        "up" => "right",
        "right" => "down",
        "down" => "left",
        "left" => "up",
        _ => "right"
    };
}

(int, int) GetNextPosition((int, int) currentPosition, string currentDirection)
{
    return (currentPosition.Item1 + directionDict[currentDirection].Item1,
        currentPosition.Item2 + directionDict[currentDirection].Item2);
}
Console.WriteLine(GetStartPosition());

void part1()
{
    var distinctPositionsSet = new HashSet<(int, int)>();
    
    (int, int) nextPosition = (0, 0);
    var currentPosition = GetStartPosition();
    var currentDirection = "up";

    while (IsInsideTheMap(currentPosition))
    {
        distinctPositionsSet.Add(currentPosition);
    
        nextPosition = GetNextPosition(currentPosition, currentDirection);
        if (!IsInsideTheMap(nextPosition))
            break;
        if (lines[nextPosition.Item1][nextPosition.Item2] == '#')
        {
            currentDirection = RotateDirection90(currentDirection);
            continue;
        }
        else
        {
            currentPosition = nextPosition;
            distinctPositionsSet.Add(currentPosition);
        }
        Console.WriteLine(currentPosition + " " + currentDirection);
    }

    Console.WriteLine("Distinct positions: " + distinctPositionsSet.Count);
}

bool isObstacle(HashSet<(int, int)> visitedPositionsSet, (int, int) position, string direction)
{
    var nextPosition = (0, 0);
    var currentPosition = position;
    var currentDirection = direction;
    
    while (IsInsideTheMap(currentPosition))
    {
        nextPosition = GetNextPosition(currentPosition, currentDirection);
        if (!IsInsideTheMap(nextPosition))
            return false;
        if (visitedPositionsSet.Contains(currentPosition)
            && visitedPositionsSet.Contains(nextPosition))
            return true;
        if (lines[nextPosition.Item1][nextPosition.Item2] == '#')
        {
            currentDirection = RotateDirection90(currentDirection);
            continue;
        }
        else
        {
            currentPosition = nextPosition;
        }
        // Console.WriteLine("IsObstacle: " + currentPosition + " " + currentDirection);
    }
    return false;
}

void part2()
{
    var numOfObstacles = 0;
    var visitedPositionsSet = new HashSet<(int, int)>();
    
    (int, int) nextPosition = (0, 0);
    var currentPosition = GetStartPosition();
    var currentDirection = "up";

    while (IsInsideTheMap(currentPosition))
    {
        visitedPositionsSet.Add(currentPosition);

        nextPosition = GetNextPosition(currentPosition, currentDirection);
        if (!IsInsideTheMap(nextPosition))
            break;
        
        // SEARCHING FOR OBSTACLE
        var searchStartPosition = GetNextPosition(currentPosition, RotateDirection90(currentDirection));
        if (IsInsideTheMap(searchStartPosition) &&
            isObstacle(visitedPositionsSet, searchStartPosition, RotateDirection90(currentDirection)))
            numOfObstacles++;
        
        
        if (lines[nextPosition.Item1][nextPosition.Item2] == '#')
        {
            currentDirection = RotateDirection90(currentDirection);
            continue;
        }
        else
        {
            currentPosition = nextPosition;
            visitedPositionsSet.Add(currentPosition);
        }

        // Console.WriteLine(currentPosition + " " + currentDirection);
        Console.WriteLine("Number of nodes visited: " + visitedPositionsSet.Count);
    }
    Console.WriteLine("Number of obstacles: " + numOfObstacles);
}

// part1();
part2();
    








