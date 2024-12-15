// See https://aka.ms/new-console-template for more information

using Day14;

var lines = File.ReadAllLines("input.txt");
var robots = new List<Robot>();
var robotsDict = new Dictionary<(int, int), List<Robot>>();

foreach (var line in lines)
{
    var posStr = line.Split(" ")[0];
    var velStr = line.Split(" ")[1];

    var posx = int.Parse(posStr.Split("=")[1].Split(",")[0]);
    var posy = int.Parse(posStr.Split("=")[1].Split(",")[1]);
    var velx = int.Parse(velStr.Split("=")[1].Split(",")[0]);
    var vely = int.Parse(velStr.Split("=")[1].Split(",")[1]);
    robots.Add(new Robot((posy, posx), (vely, velx)));
}

var tileH = 103;
var tileW = 101;

long CalculateSafetyFactor()
{
    long factor = 1;

    int count = 0;
    for (var i = 0; i < tileH / 2; i++)
    for (var j = 0; j < tileW / 2; j++)
        if (robotsDict.ContainsKey((i, j)))
            count+=robotsDict[(i,j)].Count;
    factor *= count;

    count = 0;
    for (var i = 0; i < tileH / 2; i++)
    for (var j = tileW / 2 + 1; j < tileW; j++)
        if (robotsDict.ContainsKey((i, j)))
            count+=robotsDict[(i,j)].Count;
    factor *= count;

    count = 0;
    for (var i = tileH / 2 + 1; i < tileH; i++)
    for (var j = 0; j < tileW / 2; j++)
        if (robotsDict.ContainsKey((i, j)))
            count+=robotsDict[(i,j)].Count;
    factor *= count;

    count = 0;
    for (var i = tileH / 2 + 1; i < tileH; i++)
        for (var j = tileW / 2 + 1; j < tileW; j++)
            if (robotsDict.ContainsKey((i, j)))
                count+=robotsDict[(i,j)].Count;
    factor *= count;
    
    return factor;
}

void PrintMap(HashSet<(int, int)> positionsSet)
{
    for(var i =0; i<tileH; i++, Console.WriteLine(""))
        for (var j = 0; j < tileW; j++)
            if (positionsSet.Contains((i, j)))
                Console.Write("*");
            else 
                Console.Write(".");
}

void part1()
{
    var time = 100;
    foreach (var t in Enumerable.Range(0, time))
    {
        foreach (var robot in robots)
        {
            var newPos = (robot.Position.Item1 + robot.Velocity.Item1,
                robot.Position.Item2 + robot.Velocity.Item2);
            newPos.Item1 = newPos.Item1 % tileH;
            if (newPos.Item1 < 0)
                newPos.Item1 = tileH + newPos.Item1;
            newPos.Item2 = newPos.Item2 % tileW;
            if (newPos.Item2 < 0)
                newPos.Item2 = tileW + newPos.Item2;
            robot.Position = newPos;
        }
    }

    foreach (var robot in robots)
    {
        if (!robotsDict.ContainsKey(robot.Position))
            robotsDict[robot.Position] = new List<Robot>();
        robotsDict[robot.Position].Add(robot);
    }
    PrintMap([..robotsDict.Keys]);
    Console.WriteLine($"Safety factor: {CalculateSafetyFactor()}");
}

void part2()
{
    int timeMax = 1000000000;
    var robotPositions = new HashSet<(int, int)>();
    foreach (var t in Enumerable.Range(0, timeMax))
    {
        foreach (var robot in robots)
        {
            var newPos = (robot.Position.Item1 + robot.Velocity.Item1,
                robot.Position.Item2 + robot.Velocity.Item2);
            newPos.Item1 = newPos.Item1 % tileH;
            if (newPos.Item1 < 0)
                newPos.Item1 = tileH + newPos.Item1;
            newPos.Item2 = newPos.Item2 % tileW;
            if (newPos.Item2 < 0)
                newPos.Item2 = tileW + newPos.Item2;
            robot.Position = newPos;
        }

        robotPositions = robots.Select(r => r.Position).ToHashSet();
        var countInTheMiddleX = robotPositions.Select(pos =>
            pos.Item2 > tileW / 2 - 3 && pos.Item2 < tileW / 2 + 3
        ).Count();
        if (countInTheMiddleX / 5.0 > tileH * 0.97)
        {
            Console.WriteLine($"Candidate time: {t+1}");
            PrintMap(robotPositions);
            Console.WriteLine("=====");
            return;
        }
        robotPositions.Clear();
    }
}

// part1();
part2();
