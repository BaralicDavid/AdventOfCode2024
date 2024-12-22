// See https://aka.ms/new-console-template for more information

using Day16;

var directionDict = new Dictionary<string, (int, int)>
{
    ["up"] = (-1, 0),
    ["down"] = (1, 0),  
    ["left"] = (0, -1),
    ["right"] = (0, 1)
};

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

string RotateDirection270(string direction)
{
    return direction switch
    {
        "up" => "left",
        "right" => "up",
        "down" => "right",
        "left" => "down",
        _ => "left"
    };
}

bool IsInsideTheMap((int, int) position, string[] lines)
{
    return position.Item1 >= 0 && position.Item1 < lines.Length
                                      && position.Item2 >= 0 && position.Item2 < lines[0].Length;
}

(int, int) GetNextVertex((int, int) vertex, string direction)
{
    return (vertex.Item1 + directionDict[direction].Item1,
        vertex.Item2 + directionDict[direction].Item2);
}

// program start

var lines = File.ReadAllLines("input.txt");
var graph = new Dictionary<(int, int), List<(int, int)>>();
var startVertex = (-1, -1);
var endVertex = (-1, -1);

for (var i = 0; i < lines.Length; i++)
{
    for (var j = 0; j < lines[0].Length; j++)
    {
        if (lines[i][j] != '#')
        {
            graph.Add((i,j), ReadNeighbours(i,j));
            if (lines[i][j] == 'S')
                startVertex = (i, j);
            if (lines[i][j] == 'E')
                endVertex = (i, j);
        }
    }
}

List<(int, int)> ReadNeighbours(int i, int j)
{
    var result = new List<(int, int)>();
    if( IsInsideTheMap((i-1,j), lines)
       && lines[i-1][j] != '#')
        result.Add((i-1,j));
    if( IsInsideTheMap((i+1,j), lines)
        && lines[i+1][j] != '#')
        result.Add((i+1,j));
    if( IsInsideTheMap((i,j-1), lines)
        && lines[i][j-1] != '#')
        result.Add((i,j-1));
    if( IsInsideTheMap((i,j+1), lines)
        && lines[i][j+1] != '#')
        result.Add((i,j+1));
    return result;
}

// get weight based on the direction
// if straight, return 1
// if rotate, return 1000
int GetDistance((int, int) vertex1, (int, int) vertex2, string direction)
{
    if (vertex2 == GetNextVertex(vertex1, direction))
        return 1;
    if (vertex2 == GetNextVertex(vertex1, RotateDirection90(direction))
        || vertex2 == GetNextVertex(vertex1, RotateDirection270(direction)))
        return 1000;
    return int.MaxValue;
}

Dictionary<(int, int), (int, int)> GetShortestPath(
    (int, int) startVertex, (int, int) endVertex, string startDirection)
{
    // mapping of vertex and it's shortest path neighbour
    var pathDict = new Dictionary<(int, int), (int, int)>();
    // distances for every vertex to the start vertex
    var distanceDict = new Dictionary<(int, int), long>();
    // priority queue for vertices to visit
    // direction is also stored
    // vertices with shortest path will get dequeued first
    var verticesPQ = new PriorityQueue<((int, int), string), VertexPriority>();
    
    // init startVertex with 0 distance
    distanceDict[startVertex] = 0;
    verticesPQ.Enqueue((startVertex, startDirection), new VertexPriority(startVertex, distanceDict[startVertex]));

    while (verticesPQ.Count > 0)
    {
        var vertexDirPair = verticesPQ.Dequeue();
        var vertex = vertexDirPair.Item1;
        var direction = vertexDirPair.Item2;
        
        var neighbours = graph[vertex];
        foreach (var neighbourVertex in neighbours)
        {
            var newDist = GetDistance(vertex, neighbourVertex, direction)
                + distanceDict[vertex];
            if (newDist < distanceDict[neighbourVertex])
            {
                distanceDict[neighbourVertex] = newDist;
            }
        }

    }

    return pathDict;
}


#region Part1

void part1()
{
    
    
    
}


#endregion


part1();