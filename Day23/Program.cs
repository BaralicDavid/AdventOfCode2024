// See https://aka.ms/new-console-template for more information

using Day23;

Console.WriteLine("Hello, World!");

var lines = File.ReadAllLines("input.txt");
var computerConnectionsDict = new Dictionary<string, HashSet<string>>();
var connections = new List<(string, string)>();

foreach (var line in lines)
{
    var comp1 = line.Split("-")[0];
    var comp2 = line.Split("-")[1];
    computerConnectionsDict.TryAdd(comp1, new HashSet<string>());
    computerConnectionsDict.TryAdd(comp2, new HashSet<string>());
    computerConnectionsDict[comp1].Add(comp2);
    computerConnectionsDict[comp2].Add(comp1);
    connections.Add((comp1, comp2));
}

#region Part1

void PrintGroupsList(List<HashSet<string>> groups)
{
    foreach (var group in groups)
    {
        var str = string.Join(", ", group.OrderBy(str => str));
        Console.WriteLine(str);
    }
}

List<HashSet<string>> CreateGroupsPart1()
{
    var groups = new List<HashSet<string>>();
    var computers = computerConnectionsDict.Keys.ToList();
    
    for (int i = 0; i < computers.Count; i++)
    {
        for (int j = i + 1; j < computers.Count; j++)
        {
            for (int k = j + 1; k < computers.Count; k++)
            {
                var comp1 = computers[i];
                var comp2 = computers[j];
                var comp3 = computers[k];
                if (computerConnectionsDict[comp1].Contains(comp2) &&
                    computerConnectionsDict[comp2].Contains(comp3) &&
                    computerConnectionsDict[comp3].Contains(comp1))
                    groups.Add(new HashSet<string> { comp1, comp2, comp3 });
            }
        }
    }
    return groups;
}

void Part1()
{
    var groups = CreateGroupsPart1();
    var groupsWith3 = groups.Where(set => set.Count == 3).ToList();
    var groupsWith3AndT = groupsWith3
        .Where(set => set.Where(e => e.StartsWith('t')).ToList().Count > 0)
        .ToList();

    PrintGroupsList(groups);
    Console.WriteLine($"{groupsWith3AndT.Count} many fully connected sets of size 3 that start with t.");
}
#endregion

#region Part2

List<HashSet<string>> CreateGroupsPart2()
{
    // initialize groups dict with pair of connections
    var groups = connections.Select(pair => new HashSet<string>() { pair.Item1, pair.Item2 }).ToList();
    
    foreach (var comp in computerConnectionsDict.Keys)
    {
        foreach (var group in groups)
        {
            if (group.Contains(comp))
                continue; 
            
            var isConnectedToAll = true;
            foreach (var groupComp in group)
            {
                if (!computerConnectionsDict[groupComp].Contains(comp))
                {
                    isConnectedToAll = false;
                    break;
                }
            }

            if (isConnectedToAll)
                group.Add(comp);
        }
    }
    
    // Remove duplicates
    var uniqueGroups = new HashSet<HashSet<string>>(new HashSetComparer<string>());
    foreach (var group in groups)
        uniqueGroups.Add(group);
    
    return uniqueGroups.ToList();
}

void Part2()
{
    var groups = CreateGroupsPart2();
    var maxGroup = groups.OrderByDescending(set => set.Count).First();
    var passwd = string.Join(",", maxGroup.OrderBy(str => str));
    
    Console.WriteLine($"Lan password: {passwd}");
}
#endregion

Part2();