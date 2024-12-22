// See https://aka.ms/new-console-template for more information

var lines = File.ReadAllLines("input.txt");
var initialSecrets = lines.Select(long.Parse).ToArray();

long CalculateNewSecret(long secret)
{
    secret = Mix(secret * 64, secret);
    secret = Prune(secret);

    secret = Mix(secret / 32, secret);
    secret = Prune(secret);

    secret = Mix(secret * 2048, secret);
    secret = Prune(secret);

    return secret;
}

long Mix(long value, long secret)
{
    return value ^ secret;
}

long Prune(long value)
{
    return value % 16777216;
}

#region Part1

void Part1()
{
    var sum = 0l;
    foreach (var secret in initialSecrets)
    {
        var newSecret = secret;
        for (int i = 0; i < 2000; i++)
        {
            newSecret = CalculateNewSecret(newSecret);
        }
        Console.WriteLine(newSecret);
        sum += newSecret;
    }
    Console.WriteLine($"Total sum is: {sum}");
}

#endregion

#region Part2

List<List<int>> GetSecretNumsPart2()
{
    var resultList = new List<List<int>>();
    foreach (var secret in initialSecrets)
    {
        var newSecret = secret;
        var list = new List<int>();
        list.Add((int)(newSecret % 10));
        
        for (int i = 0; i < 2000; i++)
        {
            newSecret = CalculateNewSecret(newSecret);
            list.Add((int)(newSecret % 10));
        }
        resultList.Add(list);
    }

    return resultList;
}

void Part2()
{
    var secrets = GetSecretNumsPart2();
    var sumDict = new Dictionary<string, int>();

    foreach (var singeBuyerSecrets in secrets)
    {
        var changeAddedSet = new HashSet<string>();
        for (var i = 4; i < singeBuyerSecrets.Count; i++)
        {
            var changesStr = new int[]
            {
                singeBuyerSecrets[i-3] - singeBuyerSecrets[i-4],
                singeBuyerSecrets[i-2] - singeBuyerSecrets[i-3],
                singeBuyerSecrets[i-1] - singeBuyerSecrets[i-2],
                singeBuyerSecrets[i] - singeBuyerSecrets[i-1]
            }.Aggregate("", (acc, num) => acc + num + ",");

            if (!changeAddedSet.Contains(changesStr))
            {
                sumDict.TryAdd(changesStr, 0);
                sumDict[changesStr] += singeBuyerSecrets[i];
                changeAddedSet.Add(changesStr);
            }
        }
    }

    var keyvalue = sumDict.OrderByDescending(keyvalue => keyvalue.Value).First();
    Console.WriteLine($"Most bananas for seq {keyvalue.Key} and value {keyvalue.Value}");
}

#endregion


// Part1();
Part2();