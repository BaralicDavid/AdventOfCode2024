// See https://aka.ms/new-console-template for more information
var warehouseMap  = new Dictionary<(int, int), char>();
int warehouseH;
int warehouseW;
var startPosition = (-1, -1);

var movementString = "";
var lines = File.ReadAllLines("input.txt");

var directionDict = new Dictionary<char, (int, int)>
{
    ['^'] = (-1, 0),
    ['v'] = (1, 0),  
    ['<'] = (0, -1),
    ['>'] = (0, 1)
};

bool isOutsideOfMap((int, int) position)
{
    return position.Item1 < 0 || position.Item1 >= warehouseH
                              || position.Item2 < 0 || position.Item2 >= warehouseW;
}

(int, int) GetNextPosition((int, int) position, char direction)
{
    return (position.Item1 + directionDict[direction].Item1,
        position.Item2 + directionDict[direction].Item2);
}

(int, int) GetFirstEmptyPositionInDirection((int, int) position, char direction)
{
    if (isOutsideOfMap(position))
        return position;
    
    var nextPosition = GetNextPosition(position, direction);
    while (!isOutsideOfMap(nextPosition))
    {
        if (warehouseMap[nextPosition] == '#')
            return (-1, -1);
        if (warehouseMap[nextPosition] == '.')
            return nextPosition;
        nextPosition = GetNextPosition(nextPosition, direction);
    }
    
    return (-1, -1);
}

Dictionary<(int, int), char> GetUpdatedPositionsMapPart1((int, int) position, (int, int) emptyPosition, char direction)
{
    var updatedMapPositions = new Dictionary<(int, int), char>();
    var currPosition = position;
    var nextPosition = (-1, -1);
    char tmpPosChar;
    char currPosChar = warehouseMap[currPosition];
    
    while (nextPosition != emptyPosition)
    {
        nextPosition = GetNextPosition(currPosition, direction);
        tmpPosChar = warehouseMap[nextPosition];
        
        updatedMapPositions[nextPosition] = currPosChar;
        
        currPosition = nextPosition;
        currPosChar = tmpPosChar;
    }
    // Remove @ at starting position
    updatedMapPositions[position] = '.';

    return updatedMapPositions;
}

long CalculateGPSSumOfBoxes(char c)
{
    var gpsSum = 0L;
    for(var i=0; i<warehouseH; i++)
        for(var j=0; j<warehouseW; j++)
            if (warehouseMap[(i, j)] == c)
                gpsSum += 100 * i + j;
    return gpsSum;
}

void PrintMap()
{
    for(var i=0; i<warehouseH; i++, Console.WriteLine(""))
        for (var j = 0; j < warehouseW; j++)
            Console.Write(warehouseMap[(i,j)]);
}

#region Part1

void LoadMapPos1()
{
    var i = 0;
    for (; i < lines.Length; i++)
    {
        if (String.IsNullOrWhiteSpace(lines[i]))
        {
            i++;
            break;
        }
        for (var j = 0; j < lines[i].Length; j++)
        {
            // assign start position
            if (lines[i][j] == '@')
                startPosition = (i, j);
            // assign map
            warehouseMap[(i, j)] = lines[i][j];
        }
    }

    // assign warehouse map sizes
    warehouseH = i-1;
    warehouseW = lines[0].Length;

    for (; i < lines.Length; i++)
    {
        var line = lines[i];
        movementString += line.Trim();
    }
}

void part1()
{
    LoadMapPos1();
    // PrintMap();
    
    var gpsSum = 0L;
    var currPosition = startPosition;
    foreach (var directionChar in movementString)
    {
        var emptyPosition = GetFirstEmptyPositionInDirection(currPosition, directionChar);
        if(emptyPosition == (-1, -1))
            continue;
        
        var updatedMapPositions = GetUpdatedPositionsMapPart1(currPosition, emptyPosition, directionChar);
        foreach (var updatedPosition in updatedMapPositions)
            warehouseMap[updatedPosition.Key] = updatedPosition.Value;
        
        currPosition = GetNextPosition(currPosition, directionChar);
        
        // PrintMap();
        // Console.WriteLine();
    }
    Console.WriteLine($"GPS Sum of boxes: {CalculateGPSSumOfBoxes('O')}");
}
#endregion

#region Part2

void LoadMapPos2()
{
    var i = 0;
    var doubleJ = 0;
    for (; i < lines.Length; i++)
    {
        if (String.IsNullOrWhiteSpace(lines[i]))
        {
            i++;
            break;
        }
        for (var j = 0; j < lines[i].Length; j++)
        {
            doubleJ = 2 * j;
            // assign start position
            if (lines[i][j] == '@')
                startPosition = (i, doubleJ);
            // assign map
            switch (lines[i][j])
            {
                case 'O':
                    warehouseMap[(i, doubleJ)] = '[';
                    warehouseMap[(i, doubleJ + 1)] = ']';
                    break;
                case '@':
                    warehouseMap[(i, doubleJ)] = '@';
                    warehouseMap[(i, doubleJ + 1)] = '.';
                    break;
                default:
                    warehouseMap[(i, doubleJ)] = lines[i][j];
                    warehouseMap[(i, doubleJ + 1)] = lines[i][j];
                    break;
            }
        }
    }
    // assign warehouse map sizes
    warehouseH = i-1;
    warehouseW = 2 * lines[0].Length;

    for (; i < lines.Length; i++)
    {
        var line = lines[i];
        movementString += line.Trim();
    }    
}

List<(int, int)> FindNeighbouringSetOfBoxes((int, int) position, char direction)
{
    // Prim algo, find neighbouring boxes
    var neighbouringBoxes = new List<(int, int)>();
    var visited = new HashSet<(int, int)>();
    var queue = new Queue<(int, int)>(); 

    // Add starting position
    if (warehouseMap[position] == ']')
    {
        position = (position.Item1, position.Item2 - 1);
    }
    visited.Add(position);
    neighbouringBoxes.Add(position);
    queue.Enqueue(position);
    
    // add a processing for ]
    var position2 = (position.Item1, position.Item2 + 1);
    queue.Enqueue(position2);

    // BFS
    while (queue.Count > 0)
    {
        var currPosition = queue.Dequeue();
        var neighborPosition = GetNextPosition(currPosition, direction);
        
        // check neighbour
        if (warehouseMap.ContainsKey(neighborPosition))
        {
            // if ], assign position of [
            if (warehouseMap[neighborPosition] == ']')
            {
                neighborPosition = (neighborPosition.Item1, neighborPosition.Item2 - 1);
            }
            if(warehouseMap[neighborPosition] == '['
               && !visited.Contains(neighborPosition))
            {
                visited.Add(neighborPosition);
                neighbouringBoxes.Add(neighborPosition);
                queue.Enqueue(neighborPosition);
                
                // add position of ] to the queue, for later processing
                var closingParenthesisPosition = (neighborPosition.Item1, neighborPosition.Item2 + 1);
                queue.Enqueue(closingParenthesisPosition);
            }
        }
    }

    return neighbouringBoxes;
}

bool CanMoveAPosition((int, int) position, char direction, List<(int, int)> neighbouringBoxes)
{
    foreach (var boxPosition in neighbouringBoxes)
    {
        // next position for [
        var position1 = GetNextPosition(boxPosition, direction);
        // next position for ]
        var position2 = GetNextPosition((boxPosition.Item1, boxPosition.Item2 + 1), direction);
        if (warehouseMap[position1] == '#' || warehouseMap[position2] == '#')
            return false;
    }
    return true;
}

Dictionary<(int, int), char> GetUpdatedPositionsMap(List<(int, int)> neighbouringBoxes, char direction)
{
    var updatedMapPositions = new Dictionary<(int, int), char>();
    foreach (var boxPosition in neighbouringBoxes)
    {
        // update position for [
        var nextPosition1 = GetNextPosition(boxPosition, direction);
        updatedMapPositions[nextPosition1] = warehouseMap[boxPosition];
        if (!updatedMapPositions.ContainsKey(boxPosition))
            updatedMapPositions[boxPosition] = '.';
                
        // update position for ]
        var boxPosition2 = (boxPosition.Item1, boxPosition.Item2 + 1);
        var nextPosition2 = GetNextPosition(boxPosition2, direction);
        updatedMapPositions[nextPosition2] = warehouseMap[boxPosition2];
        if (!updatedMapPositions.ContainsKey(boxPosition2))
            updatedMapPositions[boxPosition2] = '.';
    }
    return updatedMapPositions;
}

void part2()
{
    LoadMapPos2();
    // PrintMap();
    
    var gpsSum = 0L;
    var currPosition = startPosition;
    foreach (var directionChar in movementString)
    {
        var nextPos = GetNextPosition(currPosition, directionChar);
        if (warehouseMap[nextPos] == '#')
            continue;
        if (warehouseMap[nextPos] == '['
            || warehouseMap[nextPos] == ']')
        {
            var neighbouringBoxes = FindNeighbouringSetOfBoxes(nextPos, directionChar);
            if (!CanMoveAPosition(currPosition, directionChar, neighbouringBoxes))
                continue;
            var updatedMapPositions = GetUpdatedPositionsMap(neighbouringBoxes, directionChar);

            foreach (var updatedMapPosition in updatedMapPositions)
                warehouseMap[updatedMapPosition.Key] = updatedMapPosition.Value;
        }
        
        // update robot position
        warehouseMap[currPosition] = '.';
        warehouseMap[nextPos] = '@';
        currPosition = nextPos;
        
        // PrintMap();
        // Console.WriteLine();
    }
    Console.WriteLine($"GPS Sum of boxes: {CalculateGPSSumOfBoxes('[')}");
}

#endregion

// part1();
part2();

